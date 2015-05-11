using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour {
	public event EventHandler Vanquished;

	public Layer CurrentLayer;

	public AudioClip Howl;
	public AudioClip Yelp;
	public AudioClip Eat;
	public AudioClip Attack;
	public float attackSpeed;
    public SkinnedMeshRenderer WolfMesh;
    public GameObject Eyes;
    public bool dead = false;
    public EnemyController enemyController;

	private Animation animation;
	private NavMeshAgent navMeshAgent;
	private AudioSource audioSource;
	private OSPAudioSource ospAudioSource;
	private AudioClip audioClip;
	private bool attackingPlayer = false;
	private Transform target;
	private bool attackNoisePlayed = false;
    private bool eatingPlayer = false;
	
	void Start ()
	{
		this.animation = GetComponent<Animation> ();
		this.navMeshAgent = GetComponent<NavMeshAgent> ();
		this.audioSource = GetComponent<AudioSource> ();
		this.ospAudioSource = GetComponent<OSPAudioSource> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update ()
	{
		if (attackingPlayer && !dead) 
		{
			this.navMeshAgent.destination = target.position;
            if (!audioSource.isPlaying)
            {
                audioSource.clip = Attack;
                ospAudioSource.Play();
                ospAudioSource.Priority = 10;
            }
		}
		else if (!audioSource.isPlaying && RollAudioDice (out audioClip) && !dead) 
		{
			audioSource.clip = audioClip;
			ospAudioSource.Play();
		}
        else if (dead && eatingPlayer && !audioSource.isPlaying)
        {
            audioSource.clip = Eat;
            ospAudioSource.Play();
        }
	}

	public void Vanquish ()
	{
        if (!dead)
        {
            attackingPlayer = false;
            this.animation.Play("death");
            this.ospAudioSource.Stop();
            this.audioSource.clip = Yelp;
            this.ospAudioSource.Play();
            dead = true;
            this.navMeshAgent.Stop();
            if (Vanquished != null)
            {
                Vanquished(this, null);
            }
        }
	}

    public void GameOver()
    {
        dead = true;
        this.navMeshAgent.Stop();
        WolfMesh.enabled = false;
        Eyes.SetActive(false);
    }

    public void EatPlayer()
    {
        eatingPlayer = true;
        audioSource.volume = .2f;
    }
	
	public void MoveTo (Vector3 destination)
	{
        if (!dead && !attackingPlayer)
        {
            this.navMeshAgent.destination = destination;
        }
	}

    public bool PathComplete()
    {
        if (this.navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid || this.navMeshAgent.remainingDistance < 5f)
        {
            return true;
        }
        return false;
    }

	public void Kill()
	{
        Invoke("StartAttacking", 1f);
		audioSource.clip = Attack;
        ospAudioSource.Play();
        ospAudioSource.Priority = 10;
	}

    private void StartAttacking()
    {
        this.navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        attackingPlayer = true;
        this.navMeshAgent.speed = 10;
        this.animation.Play("run");
        this.navMeshAgent.destination = target.position;
        audioSource.clip = Attack;
        ospAudioSource.Play();
    }
	
	private bool RollAudioDice (out AudioClip audioClip)
	{
		var random = UnityEngine.Random.Range(0, 4000);
		audioClip = null;
		if (random == 0) 
		{
			audioClip = Howl;
			return true;
		}
		
		return false;
	}
}
