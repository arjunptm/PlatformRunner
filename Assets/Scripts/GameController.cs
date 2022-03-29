using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float monsterSpawnRate;
    private float spawn_elapsed = 0;
    public float spawnCloser;
    public float spawnFarther;

    public float monsterMoveSpeed;

    private GameObject latestPlatform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawn_elapsed += Time.deltaTime;
        if (spawn_elapsed >= monsterSpawnRate)
        {
            spawn_elapsed = 0;
            Spawn_Monster();
        }
    }

    void Spawn_Monster()
    {
        GameObject monster = Instantiate(monsterPrefab);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 spawnPoint = player.transform.position + new Vector3(Random.Range(spawnCloser, spawnFarther) * (Random.Range(0,2)*2-1), 1f, Random.Range(spawnCloser, spawnFarther) * (Random.Range(0, 2) * 2 - 1));
        monster.transform.position = spawnPoint;
        monster.transform.rotation = transform.rotation * Quaternion.Euler(0, Random.Range(0,359), 0);
    }
}
