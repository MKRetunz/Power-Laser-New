using UnityEngine;
using System.Collections;

public class SelfDelete : MonoBehaviour {

    LaserGun lGun;

    private float Timer;
    public float maxTime;

	// Use this for initialization
	void Start () {
        lGun = new LaserGun();

        Timer = 0.0f;
        maxTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (lGun.gunRange == 0)
        {
            maxTime = 0.15f;
        }
        if (lGun.gunRange == 1)
        {
            maxTime = 0.5f;
        }
        if (lGun.gunRange == 2)
        {
            maxTime = 2.5f;
        }

        Timer += Time.deltaTime;

        if (Timer >= maxTime )
        {
            Destroy(this.gameObject);
        }
	}
}
