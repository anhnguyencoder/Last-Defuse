using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [Header("Cài Đặt Tầm Bắn")]
    [Tooltip("Tầm bắn của vũ khí")]
    public float range;

    [Header("Tham Chiếu")]
    [Tooltip("Transform của camera (điểm bắn)")]
    public Transform cam;

    [Tooltip("Layer mask cho các object hợp lệ để bắn")]
    public LayerMask validLayers;

    [Header("Hiệu Ứng")]
    [Tooltip("Hiệu ứng khi bắn trúng môi trường")]
    public GameObject impactEffect;
    [Tooltip("Hiệu ứng khi bắn trúng enemy")]
    public GameObject damageEffect;

    [Header("Muzzle Flare")]
    [Tooltip("GameObject hiển thị lửa nòng súng")]
    public GameObject muzzleFlare;

    [Tooltip("Thời gian hiển thị muzzle flare (giây)")]
    public float flareDisplayTime = .1f;
    private float flareCounter;

    [Header("Cài Đặt Bắn")]
    [Tooltip("Có thể bắn tự động không? (giữ chuột để bắn liên tục)")]
    public bool canAutoFire;
    [Tooltip("Thời gian giữa các lần bắn (giây)")]
    public float timeBetweenShots = .1f;
    private float shotCounter;

    [Header("Cài Đặt Đạn")]
    [Tooltip("Số đạn hiện tại trong băng")]
    public int currentAmmo = 100;
    [Tooltip("Sức chứa của băng đạn")]
    public int clipSize = 15;
    [Tooltip("Số đạn còn lại trong kho")]
    public int remainingAmmo = 300;

    private UIController UIcon;

    [Tooltip("Số đạn nhận được khi nhặt ammo pickup")]
    public int pickupAmount;

    [Header("Cài Đặt Sát Thương")]
    [Tooltip("Sát thương mỗi viên đạn")]
    public float damageAmount = 15f;

    [Header("Danh Sách Vũ Khí")]
    [Tooltip("Mảng các vũ khí có thể sử dụng")]
    public Weapon[] weapons;

    private int currentWeapon, previousWeapon;

    private int sfxIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIcon = FindFirstObjectByType<UIController>();

        SetWeapon(currentWeapon);

        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        if(flareCounter > 0)
        {
            flareCounter -= Time.deltaTime;

            if(flareCounter <= 0)
            {
                muzzleFlare.SetActive(false);
            }
        }
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {

            //Debug.Log("I shot");

            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, range, validLayers))
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.tag == "Enemy")
                {
                    Instantiate(damageEffect, hit.point, Quaternion.identity);

                    hit.transform.GetComponent<EnemyController>().TakeDamage(damageAmount);

                    AudioManager.instance.PlaySFX(0);
                }
                else
                {
                    Instantiate(impactEffect, hit.point, Quaternion.identity);

                    AudioManager.instance.PlaySFX(1);
                }
            }

            muzzleFlare.SetActive(true);
            flareCounter = flareDisplayTime;

            shotCounter = timeBetweenShots;

            currentAmmo--;

            UIcon.UpdateAmmoText(currentAmmo, remainingAmmo);

            AudioManager.instance.PlaySFX(sfxIndex);
        }
    }

    public void ShootHeld()
    {
        if (canAutoFire == true)
        {

            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                Shoot();
            }
        }
    }

    public void Reload()
    {
        Debug.Log("Reloading");

        remainingAmmo += currentAmmo;

        if (remainingAmmo >= clipSize)
        {

            currentAmmo = clipSize;

            remainingAmmo -= clipSize;
        } else
        {
            currentAmmo = remainingAmmo;

            remainingAmmo = 0;
        }

        UIcon.UpdateAmmoText(currentAmmo, remainingAmmo);
    }

    public void GetAmmo()
    {
        Debug.Log("Give Me Ammo");

        remainingAmmo += pickupAmount;

        UIcon.UpdateAmmoText(currentAmmo, remainingAmmo);
    }

    public void SetWeapon(int weaponToSet)
    {
        if (previousWeapon != currentWeapon)
        {
            weapons[previousWeapon].currentAmmo = currentAmmo;
            weapons[previousWeapon].remainingAmmo = remainingAmmo;
        }


        range = weapons[weaponToSet].range;
        flareDisplayTime = weapons[weaponToSet].flareDisplayTime;
        canAutoFire = weapons[weaponToSet].canAutoFire;
        timeBetweenShots = weapons[weaponToSet].timeBetweenShots;
        currentAmmo = weapons[weaponToSet].currentAmmo;
        clipSize = weapons[weaponToSet].clipSize;
        remainingAmmo = weapons[weaponToSet].remainingAmmo;
        pickupAmount = weapons[weaponToSet].pickupAmount;
        damageAmount = weapons[weaponToSet].damageAmount;

        muzzleFlare = weapons[weaponToSet].muzzleFlare;

        sfxIndex = weapons[weaponToSet].sfxIndex;

        foreach(Weapon w in weapons)
        {
            w.gameObject.SetActive(false);
        }

        weapons[weaponToSet].gameObject.SetActive(true);

        UIcon.UpdateAmmoText(currentAmmo, remainingAmmo);

        previousWeapon = currentWeapon;
    }

    public void NextWeapon()
    {
        currentWeapon++;
        if(currentWeapon >= weapons.Length)
        {
            currentWeapon = 0;
        }

        SetWeapon(currentWeapon);
    }

    public void PreviousWeapon()
    {
        currentWeapon--;
        if(currentWeapon < 0)
        {
            currentWeapon = weapons.Length - 1;
        }

        SetWeapon(currentWeapon);
    }
}
