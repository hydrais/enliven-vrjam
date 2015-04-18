using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour {
	private RaycastHit hit;

	public string EnemyTag = "Enemy";

	public int Distance = 50;
    private Light Flashlight;

    void Start()
    {
        Flashlight = GetComponent<Light>();
    }
	
	void Update () {

        if (Input.GetButtonDown("Mouse 0"))
        {
            Flashlight.enabled = true;
        }
        if (Input.GetButtonUp("Mouse 0"))
        {
            Flashlight.enabled = false;
        }
        if (Input.GetButton("Mouse 1"))
        {
            Application.LoadLevel(0);
        }
        if (Flashlight.enabled)
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
            
			// TODO: Run custom code to kill the enemy, as demonstrated...
			gameObject.GetComponent<FollowPlayer>().Kill();
		}
	}
}
