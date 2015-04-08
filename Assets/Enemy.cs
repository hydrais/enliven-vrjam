using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject Player;

	void Start () {
		
	}

	void Update () {
		var playerPosition = Player.transform.position;
		this.transform.position = playerPosition + new Vector3 (0, 5, 5);
	}
}
