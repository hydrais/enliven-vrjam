using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Layer 
{
	First,
	Second,
	Third
}

public enum LayerDiceRoll
{
	Advance,
	Retreat,
	Hold
}

public class EnemyController : MonoBehaviour 
{
	public GameObject Enemy;

	public List<Enemy> Enemies;

	public bool doUpdate;

	private TerrainData CanaryMountainTerrainData;

	void Start () 
	{
		doUpdate = true;
		CanaryMountainTerrainData = GameObject.FindGameObjectWithTag ("Terrain").GetComponent<TerrainCollider>().terrainData;
		for (int i = 0; i < 5; i++) 
		{
			CreateEnemy ();
		}

		InvokeRepeating (
			"ControllerLoop",
			0,
			.2F
		);
	}

	public void GameOver()
	{
		doUpdate = false;
        foreach (var enemy in Enemies)
        {
            enemy.GameOver();
        }
	}

	void ControllerLoop ()
	{
        if (doUpdate)
        {
            var attackThisTurn = false;
            foreach (var enemy in Enemies)
            {
                var layerRoll = RollLayerDice();

                if (layerRoll == LayerDiceRoll.Advance)
                {
                    attackThisTurn = AdvanceLayer(enemy);
                }
                else if (layerRoll == LayerDiceRoll.Retreat)
                {
                    RetreatLayer(enemy);
                }

                if (enemy.PathComplete())
                {
                    enemy.MoveTo(
                        GetRandomEnemyPosition(enemy.CurrentLayer)
                    );
                }
            }
        }
	}

	public void CreateEnemy ()
	{
		var instantiatedEnemy = (GameObject) GameObject.Instantiate (
			Enemy,
			GetRandomEnemyPosition (
				Layer.Third
			),
			Enemy.transform.rotation
		);

        var enemy = instantiatedEnemy.GetComponent<Enemy>();
		this.Enemies.Add(enemy);
        enemy.Vanquished += enemy_Vanquished;
	}

    void enemy_Vanquished(object sender, EventArgs e)
    {
        var enemy = (Enemy)sender;
        Enemies.Remove(enemy);
        CreateEnemy();
    }

	public bool AdvanceLayer (Enemy enemy)
	{
		if (enemy.CurrentLayer == Layer.Third)
			enemy.CurrentLayer = Layer.Second;
		else if (enemy.CurrentLayer == Layer.Second)
			enemy.CurrentLayer = Layer.First;
        else if (enemy.CurrentLayer == Layer.First)
        {
            enemy.Kill();
            return true;
        }
        return false;
	}

	public void RetreatLayer (Enemy enemy)
	{
		if (enemy.CurrentLayer == Layer.Second)
			enemy.CurrentLayer = Layer.Third;
		
		if (enemy.CurrentLayer == Layer.First)
			enemy.CurrentLayer = Layer.Second;
	}

	private LayerDiceRoll RollLayerDice ()
	{
		var random = UnityEngine.Random.Range (0, 10000);

		if (random <= 10) 
		{
			return LayerDiceRoll.Retreat;
		} 
		else if (random > 10 && random <= 80) 
		{
			return LayerDiceRoll.Advance;
		} 
		else if (random > 80 && random <= 100) 
		{
			return LayerDiceRoll.Hold;
		}

		return LayerDiceRoll.Hold;
	}

	private Vector3 GetRandomEnemyPosition (Layer layer = Layer.Third)
	{
		var playerPosition = transform.position;
		var range = GetRange (layer);
		var direction = GetDirection ();
		
		var position = GetEnemyPosition (
			playerPosition, 
            range, 
            direction
        );

		NavMeshHit hit;
		if (NavMesh.SamplePosition(position, out hit, 3f, 1)) {
			position = hit.position;
		}

		return position;
    }
    
    private int GetRange (Layer layer)
	{
		int max = 0;
		int min = 0;

		if (layer == Layer.First) 
		{
			min = 15;
			max = 35;
		} 
		else if (layer == Layer.Second) 
		{
			min = 35;
			max = 55;
		} 
		else if (layer == Layer.Third) 
		{
			min = 55;
			max = 75;
		}

		return UnityEngine.Random.Range (min, max);
	}

	private Vector3 GetEnemyPosition (Vector3 playerPosition, int range, int angle)
	{
		var position = playerPosition;

		var x = range * Mathf.Cos(angle * Mathf.Deg2Rad);
		var z = range * Mathf.Sin(angle * Mathf.Deg2Rad);

		position.x += x;
		position.z += z;

		return position;
	}

	private int GetDirection ()
	{
		return UnityEngine.Random.Range (0, 360);
	}
}