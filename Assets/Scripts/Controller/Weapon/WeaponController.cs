using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private float rotateOffset = 180f;

    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.2f;
    private float nextShot;
    [SerializeField] private int maxAmmo = 12;
    public int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        RotateWeapon();
        Shoot();
        Reload();
    }

    void RotateWeapon()
    {
        //Check trỏ chuột có trong màn hình không
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return;

        Vector3 dispalcement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(dispalcement.y, dispalcement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        if (angle < -90 || angle > 90)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(1, -1, 1);

    }

    void Shoot()
    {
        if (currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
        }
    }

    void Reload()
    {
        if (Input.GetKey(KeyCode.R) && currentAmmo < maxAmmo)
            currentAmmo = maxAmmo;
    }
}
