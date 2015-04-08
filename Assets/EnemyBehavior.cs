using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

    public bool alive = true;
    bool visible;
    bool trackTarget;
    Material cubeMaterial;
    Color color;
    int moveSpeed = 8;
    int rotationSpeed = 3;
    float startTime;
    float randomValue;
    Transform target;
    Transform myTransform;
    float deathTimer;
    OSPAudioSource audioSource;
    bool audioSourcePlayed = false;

    void Awake()
    {
        myTransform = transform;
        audioSource = GetComponent<OSPAudioSource>();
    }

    void Start()
    {
        visible = false;
        trackTarget = false;
        //audio = GetComponent<TBE_3DCore.TBE_Source>();
        startTime = Time.time;
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(visible){
            deathTimer += Time.deltaTime;
            if (deathTimer >= 3)
            {
                alive = false;
            }
        }
        
        randomValue = Random.value;
        if (randomValue > .5 && randomValue < .51 && !trackTarget && !visible)
        {
            trackTarget = true;
            startTime = Time.time;
        }
        if (trackTarget && FiveSecondsHavePassed())
        {
            TrackTarget();
        }
        if (visible)
        {
        }
        else
        {
        }
    }

    void OnBecameVisible()
    {
        visible = true;
        trackTarget = false;
        deathTimer = 0;
    }

    void OnBecameInvisible()
    {
        visible = false;
        deathTimer = 0;
    }

    void TrackTarget()
    {
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
        Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(target.position,myTransform.position) < 5 && !audioSourcePlayed){
            audioSource.Play();
            audioSourcePlayed = true;
        }
        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
    }

    bool FiveSecondsHavePassed()
    {
        return (Time.time - startTime) > 0;
    }
}
