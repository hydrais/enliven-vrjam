using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {

    public GameObject[] enemies;
    private int CurrentEnemy;
    private bool holdEnemyGeneration = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (enemies.Length > CurrentEnemy || holdEnemyGeneration)
        {
            var currentEnemy = enemies[CurrentEnemy].GetComponent<EnemyBehavior>();
            if (!currentEnemy.alive)
            {
                enemies[CurrentEnemy].SetActive(false);
                CurrentEnemy++;
                if (enemies.Length > CurrentEnemy)
                {
                    holdEnemyGeneration = true;
                    var random = Random.Range(.5f, 5f);
                    Invoke("ActivateNextEnemy", random);
                }
            }
        }
	}

    void ActivateNextEnemy()
    {
        holdEnemyGeneration = false;
        enemies[CurrentEnemy].SetActive(true);
    }
}
