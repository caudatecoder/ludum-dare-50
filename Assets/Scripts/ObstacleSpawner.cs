using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public Vector2 spawnRate = new Vector2(1f, 3f);

    void Start()
    {
        Invoke("Spawn", Random.Range(spawnRate.x, spawnRate.y));
    }


    private void Spawn()
    {
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        Invoke("Spawn", Random.Range(spawnRate.x, spawnRate.y));
    }
}
