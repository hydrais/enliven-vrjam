using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	public GameObject Player;
	public bool debugMode;
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
            var distance = Vector3.Distance(Player.transform.position, transform.position);
            if (distance < 5f)
            {
                GetComponent<Animation>().Play("runBite");
            }
        }
        else
        {
            EnemyMeshAgent.Stop();
        }
	}

	public void Kill () {
        if (!dead)
        {
            dead = true;
            GetComponent<Animation>().Play("death");
            GetComponent<AudioSource>().Stop();
        }
	}

	public void AttackPlayer()
	{
		if (debugMode) {
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Cart" || collision.gameObject.tag == "Player") {
			AttackPlayer();
		}
	}
}
