using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class EnemyLaser : MonoBehaviour
{
    public float sSpread = 1;
    public Camera eC;
    public GameObject eLaserParticles;
    public GameObject player;
    public GameObject enemy;
    public Transform eParticleRotation;
    public static bool seePlayer;

    LineRenderer eLine;
    Vector3 eShotPoint;
    bool eLaserShot;
    float distance;

    private float shotDelay;
    private bool eReload;
    private bool eShoot;
    private float speed;
    private float alpha;


    void Start()
    {
        distance = Vector3.Distance(enemy.transform.position, player.transform.position);

        eLine = GetComponent<LineRenderer>();
        eLine.enabled = false;
        eLaserShot = false;
        eShoot = false;
        eReload = false;
        seePlayer = false;
        speed = 2;
        alpha = 1;
        shotDelay = 0;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(distance < 100)
        {
            seePlayer = true;
        }

        if(seePlayer && enemy.GetComponent<TargetScript>().isAlive)
        {
            shotDelay += 1 * Time.deltaTime;
            if(shotDelay >= 3)
            {
                eShoot = true;
                shotDelay = 0;
            }
        }

        eShotPoint = transform.position;
        if (eShoot)
        {
            StopCoroutine("ShootELaser");
            StartCoroutine("ShootELaser");
            eShoot = false;
        }

        if (eLine.enabled && eLaserShot && shotDelay < 0.5)
        {
            alpha -= Time.deltaTime * speed;
            shotDelay += 1 * Time.deltaTime;
            Color start = Color.white;
            start.a = alpha;
            Color end = Color.black;
            end.a = alpha;
            eLine.SetColors(start, end);
        }
        if (eLine.enabled && eLaserShot && shotDelay >= 0.5)
        {
            eLine.enabled = false;
            alpha = 1;
            eLaserShot = false;
            shotDelay = 0;
        }
    }

    IEnumerator ShootELaser()
    {
        eLine.enabled = true;

        eLine.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);

        //Vector2 centerPoint = ;
        Vector3 pos = Random.insideUnitCircle * sSpread / 2.0f;

        Ray ray = new Ray();
        ray = eC.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0) + pos);
        RaycastHit hit;

        eLine.SetPosition(0, eShotPoint);

        if (Physics.Raycast(ray, out hit, 100))
        {
            eLine.SetPosition(1, hit.point);
            if (hit.collider.tag == "Player")
            {
                HUD.playerHit = true;
                HUD.score -= 100;
            }
            Instantiate(eLaserParticles, hit.point, eParticleRotation.transform.rotation);
        }
        else
        {
            eLine.SetPosition(1, ray.GetPoint(100));
        }
        eLaserShot = true;
        yield return null;
    }
}
