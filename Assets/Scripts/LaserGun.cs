using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class LaserGun : MonoBehaviour
{
    public Camera c;
    Transform bulletPos;
    public GameObject laserparticles;
    public Transform particlerotation;
    public GameObject gunPos;
    public Rigidbody projectile;

    LineRenderer line;
    Vector3 shotPoint;
    bool damagetype;
    bool laserShot;
    bool burstfire;
    float shotDelay;
    float speed;
    float alpha;
    float shootTimer;
    float burstTimer;
    float fireRate;
    float bulletTime;

    //float shortRange = 0.5f;
    //float mediumRange = 1.5f;
    //float longRange = 2.5f;

    //float currentRange;

    public float bSpeed;
    public int gunDamage;
    public int gunRange; //0 = short 1 = medium 2 = long
    int maxGuns;
    int burstcounter;

    float killTimer;

    //public GameObject[] weapons;

    public Transform bulletInstancePos;

    int currentWeapon;
    /*
    //pistol
    public GameObject singleShotP;
    public GameObject burstShotP;
    public GameObject revolverP;

    //rifles
    public GameObject semiAutoR;
    public GameObject fullAutoR;
    public GameObject boltActionR;

    //Shotguns
    public GameObject pumpActionS;
    public GameObject semiAutoS;

    //array
    private GameObject[] gunArray;
    */
    void Start()
    {
        /*currentWeapon = 0;

        for(int i = 0; i < weapons.Length; i++)
        {
            if(i != currentWeapon)
            {
                weapons[i].SetActive(false);
            }
            else
            {
                ChangeWeapon(0);
            }
        }*/

        line = GetComponent<LineRenderer>();
        line.enabled = false;
        laserShot = false;
        speed = 10;
        alpha = 1;
        shotDelay = 0;
        shootTimer = 0.0f;

        bSpeed = 80;

        Cursor.lockState = CursorLockMode.Locked;

        maxGuns = 8;
        damagetype = false;

        /*gunArray = new GameObject[maxGuns];

        gunArray[0] = singleShotP;
        gunArray[1] = burstShotP;
        gunArray[2] = revolverP;
        gunArray[3] = semiAutoR;
        gunArray[4] = fullAutoR;
        gunArray[5] = boltActionR;
        gunArray[6] = pumpActionS;
        gunArray[7] = semiAutoS;

        ChangeGun(0);*/
    }

    void Update()
    {
        GameObject FirstPersonCharacter = GameObject.Find("FirstPersonCharacter");
        PlayerController playercontroller = FirstPersonCharacter.GetComponent<PlayerController>();

        //Gun statistics
        //single shot pistol
        if (playercontroller.currentGun == 0)
        {
            gunDamage = 17;
            playercontroller.fireRate = 0.4f;
            gunRange = 1;
            ChangeGun(0);
        }

        //burst fire pistol
        if (playercontroller.currentGun == 1)
        {
            gunDamage = 20;
            playercontroller.fireRate = 0.9f;
            gunRange = 1;
            ChangeGun(1);
        }

        //revolver
        if (playercontroller.currentGun == 2)
        {
            gunDamage = 80;
            playercontroller.fireRate = 1.2f;
            gunRange = 1;
            ChangeGun(2);
        }

        //semi automatic rifle
        if (playercontroller.currentGun == 3)
        {
            gunDamage = 40;
            playercontroller.fireRate = 0.6f;
            gunRange = 2;
            ChangeGun(3);
        }

        //full automatic rifle
        if (playercontroller.currentGun == 4)
        {
            gunDamage = 20;
            playercontroller.fireRate = 0.1f;
            gunRange = 2;
            ChangeGun(4);

        }

        //Bolt action rifle
        if (playercontroller.currentGun == 5)
        {
            gunDamage = 100;
            playercontroller.fireRate = 1.5f;
            gunRange = 2;
            ChangeGun(5);
        }

        //pump action shotgun
        if (playercontroller.currentGun == 6)
        {
            gunDamage = 100;
            playercontroller.fireRate = 1.3f;
            gunRange = 0;
            ChangeGun(6);
        }

        //semi automatic shotgun
        if (playercontroller.currentGun == 7)
        {
            gunDamage = 70;
            playercontroller.fireRate = 0.8f;
            gunRange = 0;
            ChangeGun(7);
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

        shotPoint = gunPos.transform.position;
        //shotPoint.y -= 0.8f;
        if (Input.GetMouseButton(0) && !PlayerController.shooting && !PlayerController.switchADS && !PlayerController.rapidFire)
        {
            StopCoroutine("ShootLaser");
            StartCoroutine("ShootLaser");
        }

        if (Input.GetMouseButton(0) && !PlayerController.shooting && !PlayerController.switchADS && PlayerController.rapidFire)
        {
            StopCoroutine("RapidLaser");
            StartCoroutine("RapidLaser");
        }

        if (PlayerController.AmmoCD == false && !PlayerController.noAmmo)
        {
            if (Input.GetMouseButton(0) && !PlayerController.shooting && !PlayerController.switchADS && !PlayerController.rapidFire)
            {
                Debug.Log("Bullet Pos " + bulletInstancePos.position);
                Debug.Log("Bullet Rot " + bulletInstancePos.rotation);

                Rigidbody bullet = Instantiate(projectile, bulletInstancePos.position, bulletInstancePos.rotation) as Rigidbody;
                if (damagetype == false)
                {
                    bullet.velocity = transform.TransformDirection(new Vector3(0, 0, bSpeed));
                }
                if (damagetype == true)
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
            if (PlayerController.AmmoCD == true && !PlayerController.OverHeat)
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

    PlayerController.shooting = true;
    }

    IEnumerator RapidLaser()
    {
        line.enabled = true;

        if (Input.GetMouseButton(0) && !PlayerController.noAmmo)
        {

            if (PlayerController.AmmoCD == true && !PlayerController.OverHeat)
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

        PlayerController.shooting = true;
    }

    /*void ChangeWeapon(int id)
    {
        if (id == 0)
        {
            gunDamage = 17;
            fireRate = 0.4f;
            gunRange = 1;
        }

        //burst fire pistol
        if (id == 1)
        {
            gunDamage = 20;
            fireRate = 0.9f;
            gunRange = 1;
        }

        //revolver
        if (id == 2)
        {
            gunDamage = 80;
            fireRate = 1.2f;
            gunRange = 1;
        }

        //semi automatic rifle
        if (id == 3)
        {
            gunDamage = 40;
            fireRate = 0.6f;
            gunRange = 2;
        }

        //full automatic rifle
        if (id == 4)
        {
            gunDamage = 20;
            fireRate = 0.1f;
            gunRange = 2;

        }

        //Bolt action rifle
        if (id == 5)
        {
            gunDamage = 100;
            fireRate = 1.5f;
            gunRange = 2;
        }

        //pump action shotgun
        if (id == 6)
        {
            gunDamage = 100;
            fireRate = 1.3f;
            gunRange = 0;
        }

        //semi automatic shotgun
        if (id == 7)
        {
            gunDamage = 70;
            fireRate = 0.8f;
            gunRange = 0;
        }
        Debug.Log(currentRange);
        bulletInstancePos = weapons[id].transform.FindChild("BulletPos").transform;
        for (int i = 0; i < maxGuns; i++)
        {
            if (i == id)
            {
                damagetype = false;

                if (id == 1 || id == 6 || id == 7)
                {
                    damagetype = true;
                }
            }
        }
    }*/

    int ChangeGun (int i)
    {
        for (int w = 0; w < maxGuns; w++)
        {
            if (w == i)
            {
                damagetype = false;

                if (i == 1 || i == 6 || i == 7)
                {
                    damagetype = true;
                }
            }
        }
        return 0;
    }
}
