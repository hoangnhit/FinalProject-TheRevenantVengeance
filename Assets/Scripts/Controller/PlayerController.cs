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
    [SerializeField] public int level = 1;

    [SerializeField] private GameManager gameManager;
    [SerializeField] public AttackDetector attackDetector;

    private GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    public float fireballSpeed = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField] private GameObject auraPrefab;
    private GameObject activeAura;
    private bool isAttacking = false;

    [SerializeField] private GameObject attackHitbox;

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

    // === GOD MODE ===
    [HideInInspector] private bool isGodMode = false;

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
        if (auraPrefab != null)
        {
            Instantiate(auraPrefab, transform.position, Quaternion.identity, transform);
        }
    }
    void Update()
    {
        // === GOD MODE TOGGLE ===
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.M))
        {
            isGodMode = !isGodMode;
            Debug.Log("⚡ GOD MODE: " + (isGodMode ? "ON" : "OFF"));
        }

        UpdateHealthBar();
        UpdateEnergyBar();
        UpdateExpBar();
        gameManager.UpdateLevelUI(level);
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            Attack();
        }
        AutoShootFireball();

        if (PlayerState.acquiredSwordSpin)
        {
            ShowCircleEffect();
            ActivateSwordSpin();
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

        if (!isAttacking)
        {
            rb.linearVelocity = playerInput * moveSpeed;

            if (playerInput.x < 0)
                spriteRenderer.flipX = true;
            else if (playerInput.x > 0)
                spriteRenderer.flipX = false;

            animator.SetBool("isMove", playerInput != Vector2.zero);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip);
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void EnableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
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
            attackHitbox.SetActive(false);
            Debug.Log("Attack Hitbox: DISABLED!");
        }
        else
        {
            Debug.LogError("Attack Hitbox is not assigned in PlayerController!");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isGodMode)
        {
            Debug.Log("TakeDamage ignored because God Mode is ON");
            return;
        }

        if (Time.time - lastHitTime < hitCooldown) return;

        lastHitTime = Time.time;
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();

        if (damageTextPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 1.2f, 0);
            GameObject textObj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);
            DamageText dmgText = textObj.GetComponent<DamageText>();
            if (dmgText != null) dmgText.SetDamage(damage);
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
        if (isGodMode)
        {
            Debug.Log("Die() ignored because God Mode is ON");
            return;
        }

        animator.SetTrigger("Die");
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }
        animator.SetBool("isDead", true);

        enabled = false;
        rb.linearVelocity = Vector2.zero;

        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        if (attackHitbox != null && attackHitbox.activeSelf)
        {
            attackHitbox.SetActive(false);
        }

        Destroy(gameObject, 3f);
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
        }

        Time.timeScale = ultimateTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(ultimateDuration);

        if (ultimateVideoPlayer != null) ultimateVideoPlayer.Stop();
        if (ultimateCanvas != null) ultimateCanvas.enabled = false;

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
            currentCircleEffect.transform.SetParent(transform);
            currentCircleEffect.transform.localPosition = new Vector3(0, -0.7f, 0);
        }
    }

    float fireRate = 0.5f;
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

    public void ApplyPoison(int damagePerSecond, float duration)
    {
        if (isGodMode)
        {
            Debug.Log("Poison ignored because God Mode is ON");
            return;
        }

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
        if (hasSwordSpin) return;

        hasSwordSpin = true;
        int swordCount = 3;

        for (int i = 0; i < swordCount; i++)
        {
            GameObject sword = Instantiate(swordSpinPrefab, spinCenter.position, Quaternion.identity);
            sword.transform.SetParent(spinCenter);
            SwordSpin spin = sword.GetComponent<SwordSpin>();
            spin.ownerTag = "Player";
            spin.bossCenter = spinCenter;
            spin.angleOffset = i * 360f / swordCount;
            spin.InitPosition();
        }
    }
}
