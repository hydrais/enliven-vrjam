using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	public GameObject Player;

	void Update () {
		NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent> ();
		navMeshAgent.destination = Player.transform.position;
	}
}
