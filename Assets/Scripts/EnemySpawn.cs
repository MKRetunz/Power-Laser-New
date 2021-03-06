﻿using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

    private float timer;
    private float teleportTimer;
    private float waveTimer;
    public int wave;
    private int maxEnemies;
    private int waveTime;
    private bool waveactivator;
    private int i;
    private int number;
    private int recentpos;
    private bool enemyplaced;
    private bool waveRefresh;
    private bool qPressed;
    private float targetTime;
    private float waveduration;
    private int timerString;
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
        waveRefresh = false;
        teleporter.Stop();
        teleportTimer = 0.0f;
        waveTimer = 0.0f;
        waveTime = 180;
        targetTime = 3.0f;

        waveduration = waveTime;
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
        waveduration -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
        {
            qPressed = true;
        }
        if (!Input.GetKey(KeyCode.Q))
        {
            qPressed = false;
        }

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
            waveRefresh = false;

            number = Random.Range(1, spawnpoints.Length);
        }

        if (teleportTimer >= 0.5f)
        {
            teleporter.Stop();
        }

        timerString = (int)waveduration;
        //waveRefresh = false;

    }

    void newWave ()
    {
        if (waveactivator)
        {
            wave++;
            i = 0;
            maxEnemies = wave * 5;
            waveactivator = false;
            waveTimer = 0.0f;
            waveTime = waveTime + 60;
            waveduration = waveTime + 10.0f;
            waveRefresh = true;
        }
    }
    void OnGUI ()
    {
        if (qPressed)
        {
            GUIStyle Coverstyle = new GUIStyle();
            Coverstyle.alignment = TextAnchor.MiddleCenter;
            Coverstyle.fontSize = 40;
            Coverstyle.normal.textColor = Color.blue;

            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 30, 400, 30), "Wave: " + wave.ToString(), Coverstyle);
            if (waveRefresh == false)
            {
                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 70, 400, 30), "Time remaining: " + timerString.ToString(), Coverstyle);
            }
        }
    }
}

