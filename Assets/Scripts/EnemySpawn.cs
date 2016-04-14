using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

    float timer;
    public int wave;
    public int maxEnemies;
    bool waveactivator;
    int i;
    int number;
    bool enemyplaced;
    public float targetTime = 0.1f;
    public Rigidbody blueprint;
    public Transform[] spawnpoints;
    public Transform player;


    // Use this for initialization
    void Start () {
        wave = 1;
        maxEnemies = 10;
        number = Random.Range(1, spawnpoints.Length);
        enemyplaced = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().name == "Target(Clone)")
        {
            
        }

        else
        {
            newWave();
        }
    }

        // Update is called once per frame
        void Update () {

        float dist = Vector3.Distance(player.position, spawnpoints[0].position);
        float dist1 = Vector3.Distance(player.position, spawnpoints[1].position);
        float dist2 = Vector3.Distance(player.position, spawnpoints[2].position);
        float dist3 = Vector3.Distance(player.position, spawnpoints[3].position);

        timer += Time.deltaTime;

        if (timer >= targetTime && i < maxEnemies)
        {
            Rigidbody enemy = Instantiate(blueprint, transform.position, transform.rotation) as Rigidbody;

                if (dist < dist1 && dist < dist2 && dist < dist3)
                {
                    enemy.position = spawnpoints[0].position;
                    enemyplaced = true;
                }
                if (dist1 < dist && dist1 < dist2 && dist1 < dist3)
                {
                    enemy.position = spawnpoints[1].position;
                    enemyplaced = true;
                }
                if (dist2 < dist && dist2 < dist1 && dist2 < dist3)
                {
                    enemy.position = spawnpoints[2].position;
                    enemyplaced = true;
                }
                if (dist3 < dist && dist3 < dist1 && dist3 < dist2)
                {
                    enemy.position = spawnpoints[3].position;
                    enemyplaced = true;
                }
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

        

    }

    void newWave ()
    {
        if (waveactivator)
        {
            wave++;
            i = 0;
            maxEnemies = wave * 10;
            waveactivator = false;
        }
    }
}
