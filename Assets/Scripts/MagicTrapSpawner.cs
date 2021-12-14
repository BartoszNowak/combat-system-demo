using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTrapSpawner : MonoBehaviour
{
    private const float ArenaSize = 17.5f;

    [SerializeField]
    private int instances = 1;
    [SerializeField]
    private float randomFactor = 4f;
    [SerializeField]
    private GameObject indicator;
    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private GameObject impact;
    [SerializeField]
    private AudioClip sound;

    private Transform playerTransform;
    private bool attacking = true;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Loop());
    }

    public void Disable()
	{
        attacking = false;
        StopAllCoroutines();
	}

    private IEnumerator Loop()
	{
        while(attacking)
		{
            yield return new WaitForSeconds(3);
            yield return StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
	{
        var spawnDelay = 2f;
        var positions = new List<Vector3>();

        var initial = Instantiate(indicator, SpawnPosition(), Quaternion.identity);
        positions.Add(initial.transform.position);
        Destroy(initial, spawnDelay);

        for (int i = 1; i < instances; i++)
		{
            var target = Instantiate(indicator, RandomPosition(), Quaternion.identity);
            positions.Add(target.transform.position);
            Destroy(target, spawnDelay);
        }
        

        yield return new WaitForSeconds(spawnDelay);

		foreach (var p in positions)
		{
            var magic = Instantiate(effect, p, Quaternion.identity);
			var hitbox = Instantiate(impact, p, Quaternion.identity);
			AudioSource.PlayClipAtPoint(sound, magic.transform.position + Vector3.up * 5);

			Destroy(magic, 4f);
			Destroy(hitbox, 3f);
		}
    }

    private Vector3 SpawnPosition()
	{
        var xPos = Mathf.Clamp(playerTransform.position.x, -ArenaSize, ArenaSize);
        var zPos = Mathf.Clamp(playerTransform.position.z, -ArenaSize, ArenaSize);
        return new Vector3(xPos, 0, zPos);
	}

    private Vector3 RandomPosition()
	{
        var pos = SpawnPosition();

        var offset = new Vector3(RandomOffset(pos.x), 0, RandomOffset(pos.z));
        return pos + offset;
	}

    private float RandomOffset(float pos)
	{
        var limit = 1;
        var s = Mathf.Max(-randomFactor, -ArenaSize - pos);
        var e = Mathf.Min(randomFactor, ArenaSize - pos);
        var result = Random.Range(s, e);
        if (result < limit && result > 0) result = limit;
        if (result > -limit && result < 0) result = -limit;

        return result;
    }
}
