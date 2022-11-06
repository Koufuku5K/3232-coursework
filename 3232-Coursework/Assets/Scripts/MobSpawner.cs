using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject slimePrefab;

    // Spawns slime in random interval between 1 to 3 seconds
    private float spawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        spawnInterval = Random.Range(1f, 3f);
        StartCoroutine(spawnEnemy(spawnInterval, slimePrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject slime)
    {
        yield return new WaitForSeconds(interval);
        // Spawns slime in random position
        GameObject newSlime = Instantiate(slime, new Vector3(Random.Range(-10f, 10f), Random.Range(-8f, 8f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, slime));
    }
}
