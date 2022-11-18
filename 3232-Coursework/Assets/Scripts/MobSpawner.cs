using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject slimePrefab;
    public GameObject markPrefab;
    public Transform target;

    // Spawns slime in random interval between 1 to 3 seconds
    private float spawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        spawnInterval = Random.Range(1f, 2f);
        StartCoroutine(spawnEnemy(spawnInterval, slimePrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject slime)
    {
        Vector3 location = new Vector3(Random.Range(-10f, 10f), Random.Range(-8f, 8f));
        StartCoroutine(wait(markPrefab, location));
        yield return new WaitForSeconds(interval);
        GameObject newSlime = Instantiate(slime, location, Quaternion.identity) as GameObject;
        newSlime.transform.parent = GameObject.Find("MobSpawner").transform;
        StartCoroutine(spawnEnemy(interval, slime));
    }

    private IEnumerator wait(GameObject mark, Vector3 location)
    {
        GameObject newMark = Instantiate(mark, location, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(newMark);
    }
}
