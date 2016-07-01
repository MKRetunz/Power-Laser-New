using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class LaserGun : MonoBehaviour
{
    public Camera c;
    Transform bulletPos;
    public GameObject laserparticles;
    public Transform particlerotation;
    public Rigidbody projectile;

    LineRenderer line;
    Vector3 shotPoint;
    public GameObject laserPoint; 
    bool damagetype;
    bool laserShot;
    bool burstfire;
    bool autofire;
    float shotDelay;
    float speed;
    float alpha;
    float shootTimer;
    float burstTimer;
    float bulletTime;

    public float fireRate;
    static public float PowerUpTimer;
    private float ReloadTimer;
    public static float shootDelay;
    public static float GunHeat;

    public static bool OverHeat;
    public static bool rapidFire;
    public static bool noAmmo;
    public static bool AmmoCD;
    public static bool shooting;

    public float bSpeed;
    public int gunDamage;
    public int gunRange; //0 = short 1 = medium 2 = long
    int maxGuns;
    int burstcounter;

    float killTimer;

    public GameObject[] weapons;

    public Transform bulletInstancePos;

    int currentWeapon;

    public int currentGun;
    public int AmmoClip;
    static public int currentAmmo;

    void Start()
    {
        GunHeat = 0.0f;
        PowerUpTimer = 0.0f;
        ReloadTimer = 0.0f;
        shootDelay = 0;

        shooting = false;
        rapidFire = false;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        laserShot = false;

        speed = 10;
        alpha = 1;
        shotDelay = 0;
        shootTimer = 0.0f;

        bSpeed = 80;

        Cursor.lockState = CursorLockMode.Locked;

        AmmoCD = false;
        maxGuns = 8;
        currentGun = 0;
        damagetype = false;

        //Starting gun
        AmmoClip = 6;
        currentAmmo = AmmoClip;
        gunDamage = 17;
        fireRate = 0.4f;
        gunRange = 1;

        changeGun(0);
    }

    void Update()
    {
        // Shoot delay
        if (shooting && shootDelay < fireRate)
        {
            shootDelay += 1 * Time.deltaTime;
        }
        else if (shooting && shootDelay >= fireRate)
        {
            shootDelay = 0;
            shooting = false;
        }


        //Gun statistics
        //single shot pistol
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeGun(0);
            AmmoCD = false;
            AmmoClip = 6;
            gunDamage = 17;
            fireRate = 0.4f;
            gunRange = 1;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
        }

        //burst fire pistol
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeGun(1);
            AmmoCD = false;
            AmmoClip = 8;
            gunDamage = 20;
            fireRate = 0.9f;
            gunRange = 1;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
        }

        //revolver
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeGun(2);
            AmmoClip = 6;
            gunDamage = 80;
            fireRate = 1.2f;
            gunRange = 1;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
        }

        //semi automatic rifle
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            changeGun(3);
            gunDamage = 40;
            fireRate = 0.6f;
            gunRange = 2;
            AmmoCD = true;
        }

        //full automatic rifle
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            changeGun(4);
            gunDamage = 20;
            fireRate = 0.1f;
            gunRange = 2;
            AmmoCD = true;
        }

        //Bolt action rifle
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            changeGun(5);
            AmmoClip = 5;
            gunDamage = 100;
            fireRate = 1.5f;
            gunRange = 2;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
            AmmoCD = false;
        }

        //pump action shotgun
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            changeGun(6);
            AmmoClip = 2;
            gunDamage = 100;
            fireRate = 1.3f;
            gunRange = 0;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
            AmmoCD = false;
        }

        //semi automatic shotgun
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            changeGun(7);
            AmmoClip = 3;
            gunDamage = 70;
            fireRate = 0.8f;
            gunRange = 0;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
        }

        /*if(Input.GetKeyDown(KeyCode.T))
        {
            if(currentWeapon < weapons.Length)
            {
                currentWeapon++;

                ChangeWeapon(currentWeapon);
            }
            else
            {
                currentWeapon = 0;

                ChangeWeapon(currentWeapon);
            }
        }*/

        if (PowerUpTimer >= 0.0f && rapidFire == true)
        {
            PowerUpTimer += Time.deltaTime;
            fireRate = 0.1f;
        }

        if (PowerUpTimer >= 30.0f)
        {
            rapidFire = false;
            resetGun(currentGun);
            PowerUpTimer = 0.0f;
        }

        if (Input.GetMouseButton(0) && !shooting && !PlayerController.switchADS && !OverHeat && !noAmmo && rapidFire == true)
        {
            if (AmmoCD == true)
            {
                GunHeat += Time.deltaTime;
            }
            else
            {
                currentAmmo--;
            }
        }

        if (currentAmmo == 0)
        {
            ReloadTimer += Time.deltaTime;
            noAmmo = true;
        }

        if (ReloadTimer >= 3.0f)
        {
            currentAmmo = AmmoClip;
            ReloadTimer = 0.0f;
            noAmmo = false;
        }

        if (GunHeat < 0.0f)
        {
            GunHeat = 0.0f;
            OverHeat = false;
        }
        if (GunHeat > 1.5f) { OverHeat = true; }


        if (AmmoCD == true)
        {
            GunHeat += Time.deltaTime * 20;
        }
        if (AmmoCD == false)
        {
            currentAmmo--;
            if (currentGun == 1)
            {
                currentAmmo -= 2;
            }
        }

        GunHeat -= Time.deltaTime / 2;
        shotPoint = laserPoint.transform.position;

        //shotPoint.y -= 0.8f;
        if (Input.GetMouseButton(0) && !shooting && !PlayerController.switchADS && !rapidFire && currentWeapon != 4)
        {
            StopCoroutine("ShootLaser");
            StartCoroutine("ShootLaser");
        }

        if (Input.GetMouseButton(0) && !shooting && !PlayerController.switchADS && rapidFire || currentWeapon == 4 && Input.GetMouseButton(0) && !shooting && !PlayerController.switchADS && !rapidFire)
        {
            StopCoroutine("RapidLaser");
            StartCoroutine("RapidLaser");
        }

        if (AmmoCD == false && !noAmmo)
        {
            if (Input.GetMouseButtonDown(0) && !shooting && !PlayerController.switchADS && !rapidFire)
            {
                /*Debug.Log("Bullet Pos " + bulletInstancePos.position);
                Debug.Log("Bullet Rot " + bulletInstancePos.rotation);*/

                Rigidbody bullet = Instantiate(projectile, bulletInstancePos.position, bulletInstancePos.rotation) as Rigidbody;
                if (damagetype == false)
                {
                    bullet.velocity = transform.TransformDirection(new Vector3(0, 0, bSpeed));
                }
                if (damagetype == true && burstfire == false)
                {
                    burstfire = true;
                    burstcounter = 0;
                }
            }
            burstTimer += Time.deltaTime;
            if (burstfire == true && burstTimer >= 0.5F && burstcounter < 3)
            {
                Rigidbody bullet = Instantiate(projectile, bulletInstancePos.position, bulletInstancePos.rotation) as Rigidbody;
                bullet.velocity = transform.TransformDirection(new Vector3(0, 0, bSpeed));
                burstcounter++;
                burstTimer = 0.0f;
                Debug.Log("works");
                if (burstcounter == 3)
                {
                    burstfire = false;
                }
            }
        }

        if (line.enabled && laserShot && shotDelay < 0.5)
        {
            alpha -= Time.deltaTime * speed;
            shotDelay += 1 * Time.deltaTime;
            Color start = Color.white;
            start.a = alpha;
            Color end = Color.black;
            end.a = alpha;
            line.SetColors(start, end);
        }
        if (line.enabled && laserShot && shotDelay >= 0.5)
        {
            line.enabled = false;
            alpha = 1;
            laserShot = false;
            shotDelay = 0;
        }
    }

    IEnumerator ShootLaser()
    {
        line.enabled = true;

        if (Input.GetMouseButtonDown(0))
        {
            if (AmmoCD == true && !OverHeat)
            {
                line.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);

                Ray ray = new Ray();
                ray = c.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;

                line.SetPosition(0, shotPoint);

                if (Physics.Raycast(ray, out hit, 100) && shootTimer == 0.0f)
                {
                    line.SetPosition(1, hit.point);
                    if (hit.rigidbody && !hit.transform.GetComponent<TargetScript>().isAlive)
                    {
                        hit.rigidbody.AddForceAtPosition(transform.forward * 1000, hit.point);
                    }
                    else if (hit.rigidbody)
                    {
                        hit.rigidbody.AddForceAtPosition(transform.forward * 150, hit.point);
                        hit.transform.GetComponent<TargetScript>().GetHit(gunDamage);
                    }
                    Instantiate(laserparticles, hit.point, particlerotation.transform.rotation);
                    shootTimer += 0.1f;
                }
                else
                {
                    line.SetPosition(1, ray.GetPoint(100));
                    shootTimer = 0.0f;
                }
            }
            laserShot = true;
         yield return null;
        }

    shooting = true;
    }

    IEnumerator RapidLaser()
    {
        line.enabled = true;

        if (currentWeapon == 4)
        {
            GunHeat += Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && !noAmmo)
        {

            if (AmmoCD == true && !OverHeat)
            {
                line.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);

                Ray ray = new Ray();
                ray = c.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;

                line.SetPosition(0, shotPoint);

                if (Physics.Raycast(ray, out hit, 100))
                {
                    line.SetPosition(1, hit.point);
                    if (hit.rigidbody && !hit.transform.GetComponent<TargetScript>().isAlive)
                    {
                        hit.rigidbody.AddForceAtPosition(transform.forward * 1000, hit.point);
                    }
                    else if (hit.rigidbody)
                    {
                        hit.rigidbody.AddForceAtPosition(transform.forward * 150, hit.point);
                        hit.transform.GetComponent<TargetScript>().GetHit(gunDamage);
                    }
                    Instantiate(laserparticles, hit.point, particlerotation.transform.rotation);
                }
                else
                {
                    line.SetPosition(1, ray.GetPoint(100));
                }
                laserShot = true;
                yield return null;
            }
        }

        shooting = true;
    }

    void changeGun (int i)
    {
        bulletInstancePos = weapons[i].transform.FindChild("BulletPos").transform;
        currentWeapon = i;
        //PlayerController.muzzleflash[0].transform.position = bulletInstancePos.position;
        for (int w = 0; w < weapons.Length; w++)
        {
            if (w == i)
            {
                damagetype = false;
                weapons[w].SetActive(true);

                if (i == 1 || i == 4)
                {
                    damagetype = true;
                }
            }
            if (w != i)
            {
                weapons[w].SetActive(false);
            }
        }
    }


    void resetGun(int CG)
    {
        for (int i = 0; i < maxGuns; i++)
        {
            weapons[i].SetActive(false);

            if (i == CG)
            {
                weapons[i].SetActive(true);
            }
        }
    }
}
