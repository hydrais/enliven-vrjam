using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {
	private RaycastHit hit;

	public string EnemyTag = "Enemy";

	public int Distance = 50;

    private Light Light;

    void Start()
    {
        Light = GetComponent<Light>();
    }
	
	void Update () {

        if (Input.GetButtonDown("Mouse 0"))
        {
            Light.enabled = true;
        }
        if (Input.GetButtonUp("Mouse 0"))
        {
            Light.enabled = false;
        }
        if (Input.GetButton("Mouse 1"))
        {
            Application.LoadLevel(0);
        }
        if (Light.enabled)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(this.transform.position, forward, out hit, Distance))
            {
                if (hit.transform && hit.transform.gameObject)
                {
                    this.handleHit(hit.transform.gameObject);
                }
            }
        }
	}
	
	private void handleHit (GameObject gameObject) {
		if (gameObject.tag == EnemyTag) {
			gameObject.GetComponent<Enemy>().Vanquish();
		}
	}
}
