using UnityEngine;
using System.Collections;

/* This class spawns a new enemy once a timer reaches 0. */

public class SpawnEnemy : MonoBehaviour {

    static int spawnCount;
    const int MAX_SPAWNS = 30;
    int timer;      //controls the spawn rate

    //prefabs
    public GameObject EnemyPrefab;

	// Use this for initialization
	void Start () 
    {
        spawnCount = 0;
        timer = Random.Range(60, 300);
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer--;
        if (timer <= 0) //spawn an enemy if true
        {
            
            if (spawnCount < MAX_SPAWNS)
            {
                //creates an enemy and places it in a random location on the field.
                float xSpawnPoint = Random.Range(-367, -297);
                float zSpawnPoint = Random.Range(889, 915);

                //make sure an enemy doesn't spawn inside the house
                while ((xSpawnPoint >= -336 && xSpawnPoint <= -326) && !(zSpawnPoint >= 904 && zSpawnPoint <= 915))
                    //generate another number
                    xSpawnPoint = Random.Range(-367, -297);

                while (zSpawnPoint >= 904 && zSpawnPoint <= 915)
                    zSpawnPoint = Random.Range(889, 915);

                Instantiate(EnemyPrefab, new Vector3(xSpawnPoint, 2, zSpawnPoint), Quaternion.identity);
                spawnCount++;
            }

            //reset timer
            timer = Random.Range(60, 300);
        }
	
	}

    void ReduceSpawnCount()
    {
        spawnCount--;
        if (spawnCount < 0)
            spawnCount = 0;
    }
}
