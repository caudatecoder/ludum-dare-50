using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public string spawnLane = "center";
    public GameObject[] prefabs;
    public GameObject[] warPrefabs;
    public Vector2 spawnRate = new Vector2(1f, 3f);

    private InfoSystem system;

    void Start()
    {
        system = GameObject.FindWithTag("InfoSystem").GetComponent<InfoSystem>();
        Invoke("Spawn", Random.Range(spawnRate.x, spawnRate.y));
    }


    private void Spawn()
    {
        GameObject prefab;
        if (system.warStarted)
        {
            prefab = warPrefabs[Random.Range(0, warPrefabs.Length)];
        }
        else
        {
            prefab = prefabs[Random.Range(0, prefabs.Length)];
        }
        GameObject newObstacle = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        newObstacle.GetComponent<Obstacle>().path = spawnLane;
        Invoke("Spawn", Random.Range(spawnRate.x, spawnRate.y));
    }
}
