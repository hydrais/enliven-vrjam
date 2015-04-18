using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	public GameObject Player;

	void Update () {
		NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent> ();
		navMeshAgent.destination = Player.transform.position;
	}

	public void Kill () {
		this.Destroy (this.gameObject);
	}
}
