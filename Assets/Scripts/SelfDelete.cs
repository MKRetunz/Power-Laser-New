using UnityEngine;
using System.Collections;

public class SelfDelete : MonoBehaviour {

    private float timer;
    public float maxTime;

    // Use this for initialization
    void Start () {
        timer = 0.0f;
        maxTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if (timer >= maxTime)
        {
            Destroy(this.gameObject);
        }
	}

    public void SetBulletTime(float time)
    {
        maxTime = time;
    }
}
