using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	public GameObject Player;
    private NavMeshAgent EnemyMeshAgent;
    private bool dead = false;

    void Start()
    {
        EnemyMeshAgent = GetComponent<NavMeshAgent>();
    }
	void Update () {
        if (!dead)
        {
            EnemyMeshAgent.destination = Player.transform.position;
        }
        else
        {
            EnemyMeshAgent.Stop();
        }
	}

	public void Kill () {
        dead = true;
        GetComponent<Animator>().Play("dead");
        GetComponent<AudioSource>().Stop();
		//Destroy(this.gameObject);
	}
}
