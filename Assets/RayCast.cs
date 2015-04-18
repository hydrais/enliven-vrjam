using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour {
	private RaycastHit hit;

	public string EnemyTag = "Enemy";

	public int Distance = 50;
	
	void Update () {
		Vector3 forward = transform.TransformDirection (Vector3.forward);

		if (Physics.Raycast (this.transform.position, forward, out hit, Distance)) {
			if ( hit.transform && hit.transform.gameObject) {
				this.handleHit(hit.transform.gameObject);
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
