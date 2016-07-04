using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //public GameObject gun;
    LaserGun lGun;

    public CharacterController player;
    public bool ADS;
    public bool switchADS;
    private bool CanCover;
    private bool Covering;
    private float crouchingSpeed;
    private float CspeedUp;
    private float crouchHeight;
    private float TimerCover;
    private float hSliderValue = 0;

    public int health = 100;
    public float recoverTimer;
   
    // Use this for initialization
    void Start()
    {
        lGun = new LaserGun();

        crouchingSpeed = 0.1f;
        CspeedUp = crouchingSpeed;
        recoverTimer = 0.0f;

        switchADS = false;
        ADS = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().name == "Cover")
        {
            HUD.showText = true;
            TimerCover = 0.0f;
            CanCover = true;
        }

        if (col.GetComponent<Collider>().name == "Uncover")
        {
            CanCover = false;
        }

        if (col.GetComponent<Collider>().name == "FirePickUp")
        {
            lGun.rapidFire = true;
            lGun.PowerUpTimer = 0.1f;
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

        if (recoverTimer >= 3.0f)
        {
            health += 1;

            if (health >= 100)
            {
                health = 100;
            }
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
        TimerCover += Time.deltaTime;

        if (TimerCover > 3.0)
        {
            HUD.showText = false;
        }

        //Aim mechanics
        if (Input.GetMouseButtonDown(1))
        {
            if (!ADS && !lGun.shooting && !switchADS)
            {
                //singleShotP.GetComponent<Animator>().Play("GunToADS");
                switchADS = true;
                ADS = true;
            }
            else if (ADS && !lGun.shooting && !switchADS)
            {
                //singleShotP.GetComponent<Animator>().Play("GunFromADS");
                switchADS = true;
                ADS = false;
            }
        }
    }
}
