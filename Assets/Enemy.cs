using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour {
	public event EventHandler Vanquished;

	public Layer CurrentLayer;

	private NavMeshAgent navMeshAgent;
	
	void Start ()
	{
		this.navMeshAgent = GetComponent<NavMeshAgent> ();
	}

	public void Vanquish ()
	{
		this.Vanquished (this, null);
		Destroy (this.gameObject);
	}
	
	public void MoveTo (Vector3 destination)
	{
		this.navMeshAgent.destination = destination;
	}
}
