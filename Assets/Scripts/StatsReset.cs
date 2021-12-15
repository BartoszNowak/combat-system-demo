using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsReset : MonoBehaviour
{
	private void Awake()
	{
		var stats = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<GameStats>();
		stats.ResetStats();
	}
}
