using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour {
	public event EventHandler Vanquished;

	public Layer CurrentLayer;

	public AudioClip Howl;
	public AudioClip Snarl;
	public AudioClip Growl;
	public AudioClip Attack;
	public float attackSpeed;

	private Animation animation;
	private NavMeshAgent navMeshAgent;
	private AudioSource audioSource;
	private OSPAudioSource ospAudioSource;
	private AudioClip audioClip;
	private bool attackingPlayer = false;
	private Transform target;
	private bool dead = false;
	private bool attackNoisePlayed = false;
	
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
		if (attackingPlayer) 
		{
			var distance = Vector3.Distance(target.position, transform.position);
			if (distance < 10f && !audioSource.isPlaying && !attackNoisePlayed)
			{
				audioSource.clip = Attack;
				ospAudioSource.Play();
				attackNoisePlayed = true;
				//GetComponent<Animation>().Play("runBite");
			}
			//this.navMeshAgent.destination = target.position;
		}
		else if (!audioSource.isPlaying && RollAudioDice (out audioClip)) 
		{
			audioSource.clip = audioClip;
			ospAudioSource.Play();
		}
	}

	public void Vanquish ()
	{

		this.animation.Play ("death");
		dead = true;
		this.navMeshAgent.Stop ();
	}
	
	public void MoveTo (Vector3 destination)
	{
		var random = UnityEngine.Random.Range(0, 10);
		if (attackingPlayer || random == 5) 
		{
			attackingPlayer = true;
			this.navMeshAgent.speed = 30;
			this.animation.Play("run");
			this.navMeshAgent.destination = target.position;

			return;
		}
		this.navMeshAgent.destination = destination;
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
		else if (random == 50) 
		{
			audioClip = Snarl;
			return true;
		} 
		else if (random == 100) 
		{
			audioClip = Growl;
			return true;
		}
		
		return false;
	}
}
