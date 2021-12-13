using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Config", menuName = "Wave/New Config", order = 0)]
public class WaveConfig : ScriptableObject
{
	//private List<GameObject> enemyPrefabs;
	[SerializeField]
	public List<Wave> waves;

	//private int index = 0;

	//public void Initialize()
	//{
	//	index = 0;
	//}

	public Wave GetWaveData(int index)
	{
		//Debug.Log($"Wave {currentWave}/{waves.Count} enemy = {waves[currentWave].enemyPrefab.name}, amount {waves[currentWave].amount}");
		//Debug.Log($"Wave {index}/{waves.Count}");

		if (waves.Count > index)
		{
			var wave = waves[index];
			index++;
			return wave;
		}
		else
		{
			return null;
		}

		//Debug.Log($"Wave {currentWave}/{waves.Count} enemy = {waves[currentWave].enemyPrefab.name}, amount {waves[currentWave].amount}");

		//if (currentWave >= waves.Count) return null;

		//var wave = waves[currentWave];
		//currentWave++;

		//return wave;
	}
}

[System.Serializable]
public class Wave
{
	public GameObject enemyPrefab;
	public int amount;
}

//[System.Serializable]
//public class EnemyData
//{
//	public GameObject enemyPrefab;
//	public int amount;
//}