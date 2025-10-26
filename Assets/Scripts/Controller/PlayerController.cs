using System.Collections;
using Assets.Scripts.Controller;
using Assets.Scripts.Controller.Enemy.EnemyLv2;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;

    [SerializeField] public float maxHp = 100f;
    public float currentHp;
    [SerializeField] private Image healthBar;

    [SerializeField] private int maxEnergy = 10;
    public float currentEnergy;
    [SerializeField] private Image energyBar;
    private bool isPoisoned = false;
    private float poisonDamagePerSecond;
    private float poisonTimer = 0f;

    [SerializeField] public int maxExp = 10;
    public float currentExp;
    //[SerializeField] private Image expBar;
    [SerializeField] public int level = 1;

    [SerializeField] private GameManager gameManager;
    [SerializeField] public AttackDetector attackDetector;

    private GameObject fireballPrefab;  // Prefab quả cầu (sẽ gán khi nhặt item)
    public Transform fireballSpawnPoint;  // Điểm bắn quả cầu (thường là 1 empty object ở tay hoặc trước mặt)
    public float fireballSpeed = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField] private GameObject auraPrefab;  // Gán prefab hào quang từ Inspector
    private GameObject activeAura;  // Hào quang đang active
    // Biến để kiểm tra trạng thái tấn công
    private bool isAttacking = false;

    // --- THÊM DÒNG NÀY ---
    [SerializeField] private GameObject attackHitbox; // Kéo GameObject AttackHitbox của bạn vào đây từ Inspector

    private float lastHitTime = -999f;
    [SerializeField] private float hitCooldown = 0.5f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip takeHitClip;
    [SerializeField] private AudioClip ultiClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip levelUpClip;

    [SerializeField] private GameObject swordSpinPrefab;
    [SerializeField] private Transform spinCenter;

    [SerializeField] public SwordSpin swordSpin;
    [SerializeField] public FireballDamage fireBall;


    [SerializeField] private GameObject circleEffectPrefab;
    private GameObject currentCircleEffect;

    private bool hasSwordSpin = false;

    [SerializeField] private VideoPlayer ultimateVideoPlayer;
    [SerializeField] private Canvas ultimateCanvas;
    [SerializeField] private float ultimateRadius = 7.5f;
    [SerializeField] private float ultimateTimeScale = 0.1f;
    [SerializeField] private float ultimateDuration = 3f;

    [SerializeField] private GameObject damageTextPrefab;

    private bool isUsingUltimate = false;

    private Coroutine poisonCoroutine;

    [SerializeField] private AudioClip fireballShootClip;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = rb.GetComponent<Animator>();
    }

    void Start()
    {
        ultimateCanvas.enabled = false;


        level = PlayerState.Level;
        maxHp = PlayerState.MaxHp;
        currentHp = PlayerState.CurrentHp;
        maxExp = PlayerState.MaxExp;
        currentExp = PlayerState.CurrentExp;
        moveSpeed = PlayerState.MoveSpeed;
        attackDetector.attackDamage = PlayerState.NormalDamge;
        if (fireBall != null) fireBall.damage = PlayerState.Skill1Damge;
        if (swordSpin != null) swordSpin.damage = PlayerState.Skill2Damge;

        currentHp = maxHp;
        UpdateHealthBar();

        currentEnergy = 0;
        UpdateEnergyBar();

        currentExp = 0;
        UpdateExpBar();
        gameManager.UpdateLevelUI(level);

        // --- TẮT HITBOX KHI BẮT ĐẦU GAME (Nếu bạn chưa tắt nó trong Editor) ---
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }

        
        if (PlayerState.acquiredFireball && PlayerState.savedFireballPrefab != null)
        {
            SetFireballPrefab(PlayerState.savedFireballPrefab);
        }

        if (PlayerState.acquiredAura && PlayerState.savedAuraPrefab != null)
        {
            ActivateAura(PlayerState.savedAuraPrefab);
        }
    }

    public void ActivateAura(GameObject auraPrefab)
    {
        if (auraPrefab != null && activeAura == null)  // Chỉ tạo 1 lần
        {
            activeAura = Instantiate(auraPrefab, transform.position, Quaternion.identity);
            activeAura.transform.SetParent(transform);  // Gắn theo Player
            activeAura.transform.localPosition = Vector3.zero;  // Ở giữa Player
            Debug.Log("Hào quang đã được kích hoạt!");
        }
    }

    void Update()
    {
        UpdateHealthBar();
        UpdateEnergyBar();
        UpdateExpBar();
        gameManager.UpdateLevelUI(level);
        Movement();
        //currentEnergy = maxEnergy;
        //UpdateEnergyBar(); //test ultimate
        // Kiểm tra input tấn công
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            Attack();
        }
        AutoShootFireball();

        if (PlayerState.acquiredSwordSpin)
        {
            ShowCircleEffect();
            ActivateSwordSpin();
            //PlayerState.acquiredSwordSpin = false;
        }

        if (!isUsingUltimate && Input.GetKeyDown(KeyCode.R) && currentEnergy >= maxEnergy)
        {
            StartCoroutine(UseUltimate());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            TakeDamage(30f);
        }
    }

    void Movement()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        playerInput.Normalize();

        // Chỉ di chuyển khi không tấn công
        if (!isAttacking)
        {
            rb.linearVelocity = playerInput * moveSpeed;

            // Xoay mặt khi move
            if (playerInput.x < 0)
                spriteRenderer.flipX = true;
            else if (playerInput.x > 0)
                spriteRenderer.flipX = false;

            //Set animation khi move
            if (playerInput != Vector2.zero)
                animator.SetBool("isMove", true); // Điều khiển biến isMove trong Animator
            else
                animator.SetBool("isMove", false); // Điều khiển biến isMove trong Animator
        }
        else // Khi đang tấn công, dừng di chuyển
        {
            rb.linearVelocity = Vector2.zero;
            // Có thể giữ animation idle hoặc animation đang chạy nếu bạn muốn nhân vật vẫn hiển thị tư thế đó khi chém
            // Tuy nhiên, thường thì khi chém, animation chém sẽ ưu tiên.
        }
    }

    void Attack()
    {
        isAttacking = true; // Đặt cờ đang tấn công là true
        animator.SetTrigger("Attack"); // Kích hoạt Trigger "AttackTrigger" trong Animator
                                       // Animator sẽ tự động chọn HeroAtt1 hoặc HeroAtt2 dựa vào điều kiện isMove
        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip);
        }
    }

    // Hàm này sẽ được gọi từ Animation Event của cả HeroAtt1 và HeroAtt2
    public void EndAttack()
    {
        isAttacking = false;
        // Sau khi attack kết thúc, nếu người chơi vẫn giữ phím di chuyển,
        // hàm Movement() trong Update sẽ tự động đặt lại isMove = true
        // và nhân vật sẽ quay về HeroRun.
        // Bạn KHÔNG nên gọi DisableHitbox() ở đây nếu bạn đã đặt nó làm một Animation Event riêng.
        // Gọi ở đây có thể tắt hitbox quá muộn hoặc không đồng bộ với animation.
    }

    // --- DÁN ĐOẠN CODE NÀY VÀO ĐÂY (ngoài các hàm Start, Update, v.v., nhưng trong class PlayerController) ---
    public void EnableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true); // Bật GameObject hitbox
                                          // Nếu cần, điều chỉnh vị trí và kích thước hitbox tại đây
                                          // Ví dụ: attackHitbox.transform.localPosition = new Vector3(0.5f, 0, 0); // Di chuyển hitbox tương đối với Player
                                          // attackHitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.8f, 0.8f); // Thay đổi kích thước collider
            Debug.Log("Attack Hitbox: ENABLED!");
        }
        else
        {
            Debug.LogError("Attack Hitbox is not assigned in PlayerController!");
        }
    }

    public void DisableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false); // Tắt GameObject hitbox
            Debug.Log("Attack Hitbox: DISABLED!");
        }
        else
        {
            Debug.LogError("Attack Hitbox is not assigned in PlayerController!");
        }
    }
    // --- KẾT THÚC DÁN ---

    public void TakeDamage(float damage)
    {
        if (Time.time - lastHitTime < hitCooldown) return; // Nếu chưa đủ thời gian thì bỏ qua

        lastHitTime = Time.time;
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();
        if (damageTextPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 1.2f, 0); // bay lên đầu player
            GameObject textObj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);

            DamageText dmgText = textObj.GetComponent<DamageText>();
            if (dmgText != null)
            {
                dmgText.SetDamage(damage);
            }
        }
        if (currentHp <= 0)
        {
            animator.ResetTrigger("TakeHit");
            animator.SetTrigger("Die");
            animator.SetBool("isDead", true);
            Die();
        }
        else
        {
            animator.SetTrigger("TakeHit");
            if (audioSource != null && takeHitClip != null)
            {
                audioSource.PlayOneShot(takeHitClip);
            }
        }
    }

    public void Heal(float healValue)
    {
        if (currentHp < maxHp)
        {
            currentHp += healValue;
            currentHp = Mathf.Min(currentHp, maxHp);
            UpdateHealthBar();
        }
    }

    public void GetEnergy(float energy)
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energy;
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            UpdateEnergyBar();
        }
    }

    public void GetExp(float exp)
    {
        if (currentExp < maxExp)
        {
            currentExp += exp;
            currentExp = Mathf.Min(currentExp, maxExp);
            UpdateExpBar();
        }
        else
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        if (audioSource != null && levelUpClip != null)
        {
            audioSource.PlayOneShot(levelUpClip);
        }

        level += 1;
        moveSpeed += 0.2f;
        maxHp += 30;
        currentHp = maxHp;
        maxExp += 100;
        currentExp = 0;
        attackDetector.attackDamage += 3;
        if (fireBall) fireBall.damage += 2;
        if (swordSpin) swordSpin.damage += 1;

        UpdateHealthBar();
        UpdateExpBar();
        gameManager.UpdateHealthBarUI(currentHp, maxHp);
        gameManager.UpdateExpBarUI(currentExp, maxExp);
        gameManager.UpdateLevelUI(level);
    }

    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
            gameManager.UpdateHealthBarUI(currentHp, maxHp);
        }
    }

    protected void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.fillAmount = currentEnergy / maxEnergy;
            gameManager.UpdateEnergyBarUI(currentEnergy, maxEnergy);
        }
    }

    protected void UpdateExpBar()
    {
        gameManager.UpdateExpBarUI(currentExp, maxExp);
    }

    private void Die()
    {
        // Kích hoạt animation chết
        animator.SetTrigger("Die");
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }
        animator.SetBool("isDead", true);

        // Ngăn chặn các hành động khác của người chơi sau khi chết
        // Vô hiệu hóa input hoặc script này
        enabled = false; // Tắt script này
        rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động

        // Vô hiệu hóa collider để không còn va chạm nữa
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        // Tắt hitbox tấn công nếu nó đang bật
        if (attackHitbox != null && attackHitbox.activeSelf)
        {
            attackHitbox.SetActive(false);
        }

        Destroy(gameObject, 3f); // Hủy sau 2 giây (đảm bảo animation có thời gian để phát)

        Invoke(nameof(HandleDeath), 2f);
    }

    private void HandleDeath()
    {
        gameManager.Dead();
    }


    public void SetFireballPrefab(GameObject prefab)
    {
        fireballPrefab = prefab;
        Debug.Log("Đã kích hoạt khả năng bắn quả cầu!");
    }

    void ShootFireball()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position);
        direction.Normalize();

        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);

        // Xoay đầu đạn về đúng hướng:
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * fireballSpeed;
        }
        if (audioSource != null && fireballShootClip != null)
        {
            audioSource.PlayOneShot(fireballShootClip);
        }

        Debug.Log("Đã bắn quả cầu tự động!");
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }
    private IEnumerator UseUltimate()
    {
        isUsingUltimate = true;
        currentEnergy = 0;
        UpdateEnergyBar();

        Debug.Log("Using Ultimate Ability");

        if (ultimateCanvas != null) ultimateCanvas.enabled = true;
        if (ultimateVideoPlayer != null && ultimateVideoPlayer.clip != null)
        {
            if (!ultimateVideoPlayer.isPrepared)
            {
                ultimateVideoPlayer.Prepare();
                yield return new WaitUntil(() => ultimateVideoPlayer.isPrepared);
            }

            ultimateVideoPlayer.Play();
            if (audioSource != null && ultiClip != null)
            {
                audioSource.PlayOneShot(ultiClip);
            }
            Debug.Log("Ultimate video started playing");
        }
        else
        {
            Debug.LogWarning("Ultimate video player or clip is not set!");
        }

        Time.timeScale = ultimateTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;


        yield return new WaitForSecondsRealtime(ultimateDuration);

        if (ultimateVideoPlayer != null)
        {
            ultimateVideoPlayer.Stop();
        }

        if (ultimateCanvas != null)
        {
            ultimateCanvas.enabled = false;
            Debug.Log("Ultimate canvas disabled");
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, ultimateRadius);
        foreach (Collider2D obj in hitObjects)
        {
            if (obj.CompareTag("Enemy"))
            {
                EnemyController ec = obj.GetComponent<EnemyController>();
                if (ec != null)
                {
                    Vector2 knockbackDir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
                    ec.TakeDamage(ec.currentHp * 0.5f, knockbackDir);
                }
            }
        }

        isUsingUltimate = false;
    }
    public void ShowCircleEffect()
    {
        if (circleEffectPrefab == null) return;

        if (currentCircleEffect == null)
        {
            currentCircleEffect = Instantiate(circleEffectPrefab, transform.position, Quaternion.identity);
            currentCircleEffect.transform.SetParent(transform); // Gắn vòng tròn vào Player
            currentCircleEffect.transform.localPosition = new Vector3(0, -0.7f, 0); // điều chỉnh xuống chân
        }
    }
    float fireRate = 0.5f;  // Tốc độ bắn (0.5s/viên)
    float nextFireTime = 0f;

    void AutoShootFireball()
    {
        if (fireballPrefab == null) return;

        if (Time.time >= nextFireTime)
        {
            ShootFireball();
            nextFireTime = Time.time + fireRate;
        }
    }
    //public void ApplyPoison(float damagePerSecond, float duration)
    //{
    //    poisonDamagePerSecond = damagePerSecond;
    //    poisonTimer = duration;
    //    isPoisoned = true;
    //    Debug.Log("Player bị dính độc!");
    //}
    public void ApplyPoison(int damagePerSecond, float duration)
    {
        if (poisonCoroutine != null)
            StopCoroutine(poisonCoroutine);

        poisonCoroutine = StartCoroutine(PoisonEffect(damagePerSecond, duration));
    }
    private IEnumerator PoisonEffect(int damagePerSecond, float duration)
    {
        float elapsed = 0f;
        isPoisoned = true;

        while (elapsed < duration)
        {
            TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        isPoisoned = false;
        poisonCoroutine = null;
        Debug.Log("Hết hiệu ứng độc.");
    }
    public void ActivateSwordSpin()
    {
        if (hasSwordSpin) return; // Không cho kích hoạt lại nếu đã có

        hasSwordSpin = true;
        int swordCount = 3;

        for (int i = 0; i < swordCount; i++)
        {
            GameObject sword = Instantiate(swordSpinPrefab, spinCenter.position, Quaternion.identity);
            sword.transform.SetParent(spinCenter); // quay quanh player
            SwordSpin spin = sword.GetComponent<SwordSpin>();
            spin.ownerTag = "Player";
            spin.bossCenter = spinCenter;
            spin.angleOffset = i * 360f / swordCount;
            spin.InitPosition();
        }
    }
}