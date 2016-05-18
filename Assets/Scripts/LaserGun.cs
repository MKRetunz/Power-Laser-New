using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class LaserGun : MonoBehaviour
{
    public Camera c;
    GameObject Parent;
    public GameObject laserparticles;
    public Transform particlerotation;
    public GameObject gunPos;
    public Rigidbody projectile;

    LineRenderer line;
    Vector3 shotPoint;
    bool laserShot;
    float shotDelay;
    float speed;
    float alpha;
    float shootTimer;
    public float bSpeed;
    public int gunDamage;
    public int gunRange; //0 = short 1 = medium 2 = long
    int maxGuns;

    float killTimer;

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

    void Start()
    {
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


        gunArray = new GameObject[maxGuns];

        gunArray[0] = singleShotP;
        gunArray[1] = burstShotP;
        gunArray[2] = revolverP;
        gunArray[3] = semiAutoR;
        gunArray[4] = fullAutoR;
        gunArray[5] = boltActionR;
        gunArray[6] = pumpActionS;
        gunArray[7] = semiAutoS;

        Parent = singleShotP;
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

        for (int i = 0; i < maxGuns; i++)
        {
            gunArray[i].SetActive(false);

            if (i == playercontroller.currentGun)
            {
                gunArray[i].SetActive(true);
            } 
        }

        shotPoint = gunPos.transform.position;
        //shotPoint.y -= 0.8f;
        if (Input.GetMouseButton(0) && !PlayerController.shooting && !PlayerController.switchADS && !PlayerController.OverHeat && !PlayerController.noAmmo && !PlayerController.rapidFire) {
            StopCoroutine("ShootLaser");
            StartCoroutine("ShootLaser");
        }

        if (Input.GetMouseButton(0) && !PlayerController.shooting && !PlayerController.switchADS && !PlayerController.OverHeat && !PlayerController.noAmmo && PlayerController.rapidFire)
        {
            StopCoroutine("RapidLaser");
            StartCoroutine("RapidLaser");
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
            if (PlayerController.AmmoCD == false)
            {
                Rigidbody bullet = Instantiate(projectile, Parent.transform.position, Parent.transform.rotation) as Rigidbody;

                bullet.velocity = transform.TransformDirection(new Vector3(0, 0, bSpeed));
            }
            if (PlayerController.AmmoCD == true)
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

        if (Input.GetMouseButton(0))
        {
            if (PlayerController.AmmoCD == false)
            {
                Rigidbody bullet = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;

                bullet.velocity = transform.TransformDirection(new Vector3(0, 0, bSpeed));
            }
            if (PlayerController.AmmoCD == true)
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

    int ChangeGun (int i)
    {
        for(int w = 0; w < maxGuns; w++)
        {
            if (w == i)
            {
                Parent = gunArray[w];
            }
        }
        return 0;
    }
}
