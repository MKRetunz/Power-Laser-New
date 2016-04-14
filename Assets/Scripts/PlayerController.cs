using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //public GameObject gun;
    public CharacterController player;
    public static bool shooting;
    public static bool ADS;
    public static bool switchADS;
    private float shootDelay;
    public static bool OverHeat;
    public static bool rapidFire;
    public static bool noAmmo;
    public static bool AmmoCD;
    private bool CanCover;
    private bool Covering;
    private bool showText;
    public float crouchingSpeed;
    public float CspeedUp;
    public float crouchHeight;
    public static float GunHeat;
    public float TimerCover;
    public float hSliderValue = 0;
    public float fireRate;
    private float PowerUpTimer;
    private float ReloadTimer;

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

    public int currentGun;
    public int maxGuns;
    public int AmmoClip;
    private int currentAmmo;

    //UI
    public Slider HeatSlider;
    public Slider BoonSlider;
    public Slider AmmoSlider;
    public Text CoverPopUp;

    // Use this for initialization
    void Start()
    {
        HeatSlider.transform.position = new Vector3(Screen.width / 12 * 3, Screen.height / 12, HeatSlider.transform.position.z);
        shootDelay = 0;
        crouchingSpeed = 0.1f;
        CspeedUp = crouchingSpeed;
        GunHeat = 0.0f;
        PowerUpTimer = 0.0f;
        ReloadTimer = 0.0f;

        currentGun = 0;
        maxGuns = 8;
     
        showText = false;
        shooting = false;
        switchADS = false;
        ADS = false;
        rapidFire = false;

        gunArray = new GameObject[maxGuns];

        gunArray[0] = singleShotP;
        gunArray[1] = burstShotP;
        gunArray[2] = revolverP;
        gunArray[3] = semiAutoR;
        gunArray[4] = fullAutoR;
        gunArray[5] = boltActionR;
        gunArray[6] = pumpActionS;
        gunArray[7] = semiAutoS;


        //Starting gun
        AmmoCD = false;
        AmmoClip = 6;
        currentAmmo = AmmoClip;

        for (int i = 1; i < maxGuns; i++)
        {
            gunArray[i].SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().name == "Cover")
        {
            Debug.Log("I'm Working");
            showText = true;
            TimerCover = 0.0f;
            CanCover = true;
        }
        if (col.GetComponent<Collider>().name == "Uncover")
        {
            CanCover = false;
        }

        if (col.GetComponent<Collider>().name == "FirePickUp")
        {
            rapidFire = true;
            PowerUpTimer = 0.1f;
            Destroy(col.gameObject);
        }
        if (col.GetComponent<Collider>().name == "LasergateHit")
        {
            HUD.score -= 100;
            HUD.playerHit = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.F))
        {
            HUD.enemyDie = true;
            HUD.score += 80;
        }*/

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

        //Shooting mechanics
        if (Input.GetMouseButtonDown(0) && !shooting && !switchADS && !OverHeat && noAmmo == false && rapidFire == false)
        {
            if (!ADS)
            {
                Debug.Log("Pressed left click.");
                singleShotP.GetComponent<Animator>().Play("Gun_Shoot");
            }
            else if (ADS)
            {
                Debug.Log("Pressed left click.");
                singleShotP.GetComponent<Animator>().Play("GunADS_Shoot");
            }
            if (AmmoCD == true)
            {
                GunHeat += Time.deltaTime * 20;
            }
            if (AmmoCD == false)
            {
                currentAmmo--;
            }
        }

        if (Input.GetMouseButton(0) && !shooting && !switchADS && !OverHeat && !noAmmo && rapidFire == true)
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switchGun(0);
            AmmoCD = false;
            AmmoClip = 6;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            switchGun(1);
            AmmoCD = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            switchGun(2);
            AmmoClip = 6;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            switchGun(3);
            AmmoCD = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            switchGun(4);
            AmmoCD = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            switchGun(5);
            AmmoClip = 5;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            switchGun(6);
            AmmoClip = 2;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            switchGun(7);
            AmmoClip = 3;
            if (currentAmmo > AmmoClip) { currentAmmo = AmmoClip; }
        }

        //Crouching
        if (Input.GetKey("x"))
        {
            crouchHeight -= 0.1f * (Time.deltaTime * 10);
            CspeedUp -= crouchingSpeed * (Time.deltaTime * 10);
            if (crouchHeight < -0.6f)
            {
                crouchHeight = -0.6f;
            }
            if (CspeedUp < -0.3f)
            {
                CspeedUp = -0.3f;
            }
        }


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

        //Cover
        else if (CanCover && Input.GetKey("z"))
        {
            CspeedUp = -0.6f;
        }
        else
        {
            crouchHeight += 0.1f * (Time.deltaTime * 10);
            CspeedUp += crouchingSpeed * (Time.deltaTime * 10);
            if (crouchHeight > -0.1f)
            {
                crouchHeight = 0.0f;
            }
            if (CspeedUp > -0.2f)
            {
                CspeedUp = 0.0f;
            }
        }
        transform.localPosition = new Vector3(0, CspeedUp, 0);
        player.height = crouchHeight + 2.5f;
        GunHeat -= Time.deltaTime / 2;
        TimerCover += Time.deltaTime;

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

        if (TimerCover > 3.0)
        {
            showText = false;
        }

        if (GunHeat < 0.0f)
        {
            GunHeat = 0.0f;
            OverHeat = false;
        }
        if (GunHeat > 1.5f) { OverHeat = true; }

        AmmoSlider.value = currentAmmo;
        HeatSlider.value = GunHeat;
        BoonSlider.value = PowerUpTimer;

        //Aim mechanics
        if (Input.GetMouseButtonDown(1))
        {
            if (!ADS && !shooting && !switchADS)
            {
                Debug.Log("Pressed right click.");
                singleShotP.GetComponent<Animator>().Play("GunToADS");
                switchADS = true;
                ADS = true;
            }
            else if (ADS && !shooting && !switchADS)
            {
                Debug.Log("Pressed right click.");
                singleShotP.GetComponent<Animator>().Play("GunFromADS");
                switchADS = true;
                ADS = false;
            }
        }
    }

    //Weapon changing
    void switchGun(int newWeapon)
    {
        for (int i = 0; i < maxGuns; i++)
        {
            gunArray[i].SetActive(false);

            if(i == newWeapon)
            {
                gunArray[i].SetActive(true);
                currentGun = i;
            }
        }        
    }

    void resetGun(int CG)
    {
        for (int i = 0; i < maxGuns; i++)
            {
            gunArray[i].SetActive(false);

            if (i == CG)
            {
                gunArray[i].SetActive(true);
            }
        }
    }

    void OnGUI()
    {
        if (showText)
        {
            GUIStyle Coverstyle = new GUIStyle();
            Coverstyle.alignment = TextAnchor.MiddleCenter;
            Coverstyle.fontSize = 50;
            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height - 40, 400, 30), "Hold Z for cover.", Coverstyle);
        }
    }
}
