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

		/*InvokeRepeating (
			"ControllerLoop",
			0,
			5
		);*/
	}

	void Update ()
	{
		if (doUpdate) {
			foreach (var enemy in Enemies) {
				var layerRoll = RollLayerDice ();
				
				if (layerRoll == LayerDiceRoll.Advance) {
					AdvanceLayer (enemy);
				} else if (layerRoll == LayerDiceRoll.Retreat) {
					RetreatLayer (enemy);
				}
				
				enemy.MoveTo (
					GetRandomEnemyPosition (enemy.CurrentLayer)
				);
			}
		}
	}

	void OnDisable()
	{
		doUpdate = false;
	}

	void ControllerLoop ()
	{
		foreach (var enemy in Enemies)
		{
			var layerRoll = RollLayerDice ();

			if (layerRoll == LayerDiceRoll.Advance)
			{
				AdvanceLayer(enemy);
			}
			else if (layerRoll == LayerDiceRoll.Retreat)
			{
				RetreatLayer(enemy);
			}

			enemy.MoveTo (
				GetRandomEnemyPosition (enemy.CurrentLayer)
			);
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

		instantiatedEnemy.transform.position = GetRandomEnemyPosition (
			Layer.Third
		);

		this.Enemies.Add (
			instantiatedEnemy.GetComponent<Enemy> ()
		);
	}

	public void AdvanceLayer (Enemy enemy)
	{
		if (enemy.CurrentLayer == Layer.Third)
			enemy.CurrentLayer = Layer.Second;
		else if (enemy.CurrentLayer == Layer.Second)
			enemy.CurrentLayer = Layer.First;
		else if (enemy.CurrentLayer == Layer.First)
			enemy.Kill ();
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

		return position;
    }
    
    private int GetRange (Layer layer)
	{
		int max = 0;
		int min = 0;

		if (layer == Layer.First) 
		{
			min = 10;
			max = 15;
		} 
		else if (layer == Layer.Second) 
		{
			min = 15;
			max = 35;
		} 
		else if (layer == Layer.Third) 
		{
			min = 35;
			max = 55;
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
		position.y = CanaryMountainTerrainData.GetHeight ((int)position.x, (int)position.z)+2;

		return position;
	}

	private int GetDirection ()
	{
		return UnityEngine.Random.Range (0, 360);
	}
}