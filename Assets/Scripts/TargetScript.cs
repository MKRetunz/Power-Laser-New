using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {
    public int hp;
    public bool isAlive;
    public Transform target;
    //public GameObject player;
    Vector3 distance;

    void OnTriggerEnter(Collider col)
    {
        GameObject FirstPersonCharacter = GameObject.Find("FirstPersonCharacter");
        LaserGun lasergun = FirstPersonCharacter.GetComponent<LaserGun>();

        if (col.GetComponent<Collider>().name == "Bullet(Clone)")
        {
            this.GetHit(lasergun.gunDamage);
            Destroy(col.gameObject);
        }
    }

            // Use this for initialization
            void Start () {
        hp = 100;
        isAlive = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0)
        {
            isAlive = false;
        }

        if(isAlive && EnemyLaser.seePlayer)
        {
            transform.LookAt(target);
        }
	}

    public void GetHit(int damage)
    {
        hp -= damage;
        if(hp > 1)
        {
            HUD.score += 10;
            HUD.enemyHit = true;
        }
        else if(hp <= 0)
        {
            HUD.score += 80;
            HUD.enemyDie = true;
            Destroy(this.gameObject);
        }
        StartCoroutine("TargetHit");
    }

    IEnumerator TargetHit()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(90, 0, 0, 100);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 100);
    }
}
