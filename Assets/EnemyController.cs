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

	void Start () 
	{
		for (int i = 0; i < 5; i++) 
		{
			CreateEnemy ();
		}

		InvokeRepeating (
			"ControllerLoop",
			0,
			5
		);
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
		var instantiatedEnemy = GameObject.Instantiate (
			Enemy
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

		if (enemy.CurrentLayer == Layer.Second)
			enemy.CurrentLayer = Layer.First;
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
		var random = new System.Random ().Next (0, 100);

		if (random <= 10) 
		{
			return LayerDiceRoll.Retreat;
		} 
		else if (random > 10 && random <= 80) 
		{
			return LayerDiceRoll.Advance;
		} 
		else if (random > 80) 
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

		return new System.Random ().Next (min, max);
	}

	private Vector3 GetEnemyPosition (Vector3 playerPosition, int range, int angle)
	{
		var x = range * Mathf.Cos(angle * Mathf.Deg2Rad);
		var z = range * Mathf.Sin(angle * Mathf.Deg2Rad);

		var position = playerPosition;

		position.x += x;
		position.z += z;

		return position;
	}

	private int GetDirection ()
	{
		return new System.Random ().Next (0, 360);
	}
}