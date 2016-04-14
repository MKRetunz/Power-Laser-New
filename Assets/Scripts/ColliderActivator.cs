using UnityEngine;
using System.Collections;

public class ColliderActivator : MonoBehaviour {


    Vector3 location; 
    float timer = 0.0f;
    bool down = false;
	// Use this for initialization
	void Start () {
        location = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        Vector3 newLocation = location;

        newLocation.y -= 105;


        if (timer >= 1.5f && down == false)
        {
            transform.position = new Vector3(location.x, newLocation.y, location.z);

            down = true;

        }
        if (timer >= 3.0f && down == true)
        {
            transform.position = location;

            down = false;

            timer = 0.0f;
        }
	}
}
