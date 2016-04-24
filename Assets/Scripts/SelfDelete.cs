using UnityEngine;
using System.Collections;

public class SelfDelete : MonoBehaviour {

    private float Timer;
    public float maxTime;

	// Use this for initialization
	void Start () {
        Timer = 0.0f;
        maxTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        GameObject FirstPersonCharacter = GameObject.Find("FirstPersonCharacter");
        LaserGun lasergun = FirstPersonCharacter.GetComponent<LaserGun>();

        if (lasergun.gunRange == 0)
        {
            maxTime = 0.15f;
        }
        if (lasergun.gunRange == 1)
        {
            maxTime = 0.5f;
        }
        if (lasergun.gunRange == 2)
        {
            maxTime = 2.5f;
        }

        Timer += Time.deltaTime;

        if (Timer >= maxTime && this.name == "Bullet(Clone)")
        {
            Destroy(this.gameObject);
        }
	}
}
