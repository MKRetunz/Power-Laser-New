using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

    float timer;
    float teleportTimer;
    float waveTimer;
    public int wave;
    public int maxEnemies;
    int waveTime;
    bool waveactivator;
    int i;
    int number;
    int recentpos;
    bool enemyplaced;
    public float targetTime = 0.1f;
    public Rigidbody blueprint;
    public Transform[] spawnpoints;
    public Transform player;
    public ParticleSystem teleporter;


    // Use this for initialization
    void Start () {
        wave = 1;
        maxEnemies = 10;
        number = Random.Range(1, spawnpoints.Length);
        enemyplaced = false;
        teleporter.Stop();
        teleportTimer = 0.0f;
        waveTimer = 0.0f;
        waveTime = 60;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().name == "Target(Clone)")
        {
            //Left empty to do nothing
        }

        else
        {
            newWave();
        }
    }

        // Update is called once per frame
        void Update () {

        //Calculate distance between player and spawn
        float dist = Vector3.Distance(player.position, spawnpoints[0].position);
        float dist1 = Vector3.Distance(player.position, spawnpoints[1].position);
        float dist2 = Vector3.Distance(player.position, spawnpoints[2].position);
        float dist3 = Vector3.Distance(player.position, spawnpoints[3].position);

        timer += Time.deltaTime;
        teleportTimer += Time.deltaTime;
        waveTimer += Time.deltaTime;

        if (waveTimer >= waveTime)
        {
            newWave();
        }

        if (timer >= targetTime && i < maxEnemies && waveTimer >= 10)
        {
            Rigidbody enemy = Instantiate(blueprint, transform.position, transform.rotation) as Rigidbody;

                if (dist < dist1 && dist < dist2 && dist < dist3)
                {
                    enemy.position = spawnpoints[0].position;
                    enemyplaced = true;
                    recentpos = 0;
                }
                if (dist1 < dist && dist1 < dist2 && dist1 < dist3)
                {
                    enemy.position = spawnpoints[1].position;
                    enemyplaced = true;
                     recentpos = 1;
                }
                if (dist2 < dist && dist2 < dist1 && dist2 < dist3)
                {
                    enemy.position = spawnpoints[2].position;
                    enemyplaced = true;
                    recentpos = 2;
                }
                if (dist3 < dist && dist3 < dist1 && dist3 < dist2)
                {
                    enemy.position = spawnpoints[3].position;
                    enemyplaced = true;
                    recentpos = 3;
                }
            teleporter.Play();
            teleportTimer = 0.0f;
            teleporter.transform.position = spawnpoints[recentpos].position;

            if (enemyplaced == false)
            {
                enemy.position = spawnpoints[number].position;
            }

            timer = 0.0f;
            i++;
            enemyplaced = false;
            waveactivator = true;

            number = Random.Range(1, spawnpoints.Length);
        }

        if (teleportTimer >= 0.5f)
        {
            teleporter.Stop();
        }

    }

    void newWave ()
    {
        if (waveactivator)
        {
            wave++;
            i = 0;
            maxEnemies = wave * 10;
            waveactivator = false;
            waveTimer = 0.0f;
            waveTime = waveTime + 10;
        }
    }
    void OnGUI()
    {
        GUIStyle Coverstyle = new GUIStyle();
        Coverstyle.alignment = TextAnchor.MiddleCenter;
        Coverstyle.fontSize = 50;
        GUI.Label(new Rect(Screen.width / 16 - 200, Screen.height / 8 - 40 , 400, 30), "Wave: " + wave.ToString(), Coverstyle);
    }
}

