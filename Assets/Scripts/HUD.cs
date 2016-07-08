using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    PlayerController player;
    LaserGun lGun;
    EnemySpawn spawn;

    public Texture2D crosshairSprite;
    public Texture2D plus80;
    public Texture2D plus10;
    public Texture2D plus;
    public Texture2D min100;
    public Texture2D Gun;
    public Texture2D Score;
    public Texture2D Life;
    public Texture2D Ammo;
    public Rect minusPos;
    public Rect plusPos;
    public Rect crossPos;
    public Rect scorePos;
    public Rect gunPos;
    public Rect ammoPos;
    public Rect lifePos;
    public Rect boonPos;
    public GUIStyle textstyle;
    public float scaleTrigger = 0;
    public float scaleDelay = 0;
    public float plusDelay = 0;
    public float plusStay = 0;
    public float plusUp = 0;
    public float minusDelay = 0;
    public float minusStay = 0;
    public float minusUp = 0;
    public static bool crosshairTrigger = true;
    public bool scaleToADS = false;
    public static bool scaleFromADS = false;
    public static bool playerHit = false;
    public bool plusTrigger = false;
    public bool minusTrigger = false;
    public static bool enemyHit = false;
    public static bool enemyDie = false;
    public static bool showText;
    public static int score = 0;

    //UI
    public Slider HeatSlider;
    public Slider BoonSlider;
    public Slider HealthSlider;
    public Text CoverPopUp;
    public Text AmmoText;

    void Start()
    {
        player = new PlayerController();
        lGun = new LaserGun();
        spawn = new EnemySpawn();

        crossPos = new Rect((Screen.width - crosshairSprite.width / 2) / 2, (Screen.height - crosshairSprite.height / 2) / 2, crosshairSprite.width / 2, crosshairSprite.height / 2);
        scorePos = new Rect((Screen.width - 200) / 24 * 23, (Screen.height - 100) / 12, 200, 100);
        plusPos = new Rect(scorePos.x + 90, scorePos.y - 3, plus80.width * 1.3f, plus80.height * 1.3f);
        minusPos = new Rect(scorePos.x + 90, scorePos.y + 6, plus80.width * 1.3f, plus80.height * 1.3f);

        //boonPos = new Rect(Screen.width / 16, Screen.height / 12 * 1.5f, );
        lifePos = new Rect((Screen.width / 16 - Life.width / 2), (Screen.height - Life.height / 2), Life.width / 2, Life.height / 2);

        ammoPos = new Rect((Screen.width - Ammo.width / 2), (Screen.height - Ammo.height), Ammo.width / 2, Ammo.height / 2);
        gunPos = new Rect((Screen.width - Gun.width / 2), (Screen.height - Gun.height / 2), Gun.width / 2, Gun.height / 2);


        BoonSlider.transform.position = new Vector3(Screen.width / 16, Screen.height / 12 * 1.5f, HeatSlider.transform.position.z);
        HealthSlider.transform.position = new Vector3(Screen.width / 16, Screen.height / 12, HeatSlider.transform.position.z);

        HeatSlider.transform.position = new Vector3(Screen.width / 12 * 11, Screen.height / 12 * 3, HeatSlider.transform.position.z);
        AmmoText.transform.position = new Vector3(Screen.width / 12 * 11, Screen.height / 12 * 3, HeatSlider.transform.position.z);

        showText = false;
    }

    void OnGUI()
    {
        GUI.DrawTexture(lifePos, Life);
        GUI.DrawTexture(ammoPos, Ammo);
        GUI.DrawTexture(gunPos, Gun);
        GUI.DrawTexture(scorePos, Score);
        //Score GUI
        GUI.Label(scorePos, "Score: " + score, textstyle);

        GUIStyle Coverstyle = new GUIStyle();

        Coverstyle.alignment = TextAnchor.MiddleCenter;
        Coverstyle.fontSize = 50;
        GUI.Label(new Rect(Screen.width / 16 - 200, Screen.height / 8 - 40, 400, 30), "Wave: " + spawn.wave.ToString(), Coverstyle);

        if (plusTrigger)
        {
            GUI.DrawTexture(plusPos, plus);
        }
        if (minusTrigger)
        {
            GUI.DrawTexture(minusPos, min100);
        }

        if (showText)
        {
            Coverstyle.alignment = TextAnchor.MiddleCenter;
            Coverstyle.fontSize = 50;
            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height - 40, 400, 30), "Hold Z for cover.", Coverstyle);
        }

        //Score handling
        if (enemyHit)
        {
            plusPos = new Rect(scorePos.x + 130, scorePos.y - 3, plus80.width * 1.3f, plus80.height * 1.3f);
            plusDelay = 0;
            plusStay = 0;
            plusTrigger = true;
            plus = plus10;
            enemyHit = false;
        }
        else if (enemyDie)
        {
            plusPos = new Rect(scorePos.x + 130, scorePos.y - 3, plus80.width * 1.3f, plus80.height * 1.3f);
            plusDelay = 0;
            plusStay = 0;
            plusTrigger = true;
            plus = plus80;
            enemyDie = false;
        }
        else if (playerHit)
        {
            minusPos = new Rect(scorePos.x + 130, scorePos.y + 50, plus80.width * 1.3f, plus80.height * 1.3f);
            minusDelay = 0;
            minusStay = 0;
            minusTrigger = true;
            playerHit = false;
            player.health -= 50;
            player.recoverTimer = 0.0f;
        }

        //Run/ADS/Idle controller
        if (!player.ADS && Input.GetMouseButtonDown(1) && !player.switchADS && !lGun.shooting)
        {
            scaleToADS = true;
        }
        else if (player.ADS && Input.GetMouseButtonDown(1) && !player.switchADS && !lGun.shooting)
        {
            scaleFromADS = true;
        }

        //Crosshair
        if (crosshairTrigger)
        {
            GUI.DrawTexture(crossPos, crosshairSprite);
        }
    }

    void Update()
    {
        //toADS
        if (scaleToADS && scaleDelay < 0.2 && !scaleFromADS)
        {
            scaleTrigger += 2.5f;
            scaleDelay += 1 * Time.deltaTime;
            crossPos = new Rect((Screen.width - crosshairSprite.width / 2 + scaleTrigger) / 2, (Screen.height - crosshairSprite.height / 2 + scaleTrigger) / 2, crosshairSprite.width / 2 - scaleTrigger, crosshairSprite.height / 2 - scaleTrigger);
        }
        if (scaleDelay >= 0.2 && scaleToADS && !scaleFromADS)
        {
            scaleToADS = false;
            scaleDelay = 0;
            crosshairTrigger = false;
            player.switchADS = false;
        }

        //fromADS
        if (scaleFromADS && scaleDelay < 0.2 && !scaleToADS)
        {
            scaleTrigger -= 2.5f;
            scaleDelay += 1 * Time.deltaTime;
            crosshairTrigger = true;
            crossPos = new Rect((Screen.width - crosshairSprite.width / 2 + scaleTrigger) / 2, (Screen.height - crosshairSprite.height / 2 + scaleTrigger) / 2, crosshairSprite.width / 2 - scaleTrigger, crosshairSprite.height / 2 - scaleTrigger);
        }
        if (scaleFromADS && !scaleToADS && scaleDelay >= 0.2)
        {
            scaleTrigger = 0;
            scaleFromADS = false;
            scaleDelay = 0;
            player.switchADS = false;
        }

        //Plus score
        if (plusTrigger && plusDelay < 0.5)
        {
            plusUp = 1.5f;
            plusDelay += 1 * Time.deltaTime;
            plusPos = new Rect(plusPos.x, plusPos.y - plusUp, plus80.width * 1.3f, plus80.height * 1.3f);
        }
        if (plusTrigger && plusDelay >= 0.5)
        {
            plusStay += 1 * Time.deltaTime;
        }
        if (plusStay >= 1.5)
        {
            plusDelay = 0;
            plusTrigger = false;
            plusStay = 0;
        }

        //Minus score
        if (minusTrigger && minusDelay < 0.5)
        {
            minusUp = 1.5f;
            minusDelay += 1 * Time.deltaTime;
            minusPos = new Rect(minusPos.x, minusPos.y + minusUp, plus80.width * 1.3f, plus80.height * 1.3f);
        }
        if (minusTrigger && minusDelay >= 0.5)
        {
            minusStay += 1 * Time.deltaTime;
        }
        if (minusStay >= 1.5)
        {
            minusDelay = 0;
            minusTrigger = false;
            minusStay = 0;
        }

        AmmoText.text = lGun.currentAmmo.ToString();
        HeatSlider.value = lGun.GunHeat;
        BoonSlider.value = lGun.PowerUpTimer;
        HealthSlider.value = player.health;
    }
}