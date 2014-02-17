using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {

    public int maxEnemies = 10;
    public EnemySpawnerScript leftSpawner, rightSpawner;

    private List<EnemyScript> enemies;

    void Start() {
        enemies = new List<EnemyScript>(maxEnemies);
        StartCoroutine( SpawnRoutine( leftSpawner ) );
        StartCoroutine( SpawnRoutine( rightSpawner ) );
    }

    public void StopSpawning() {
        StopAllCoroutines();
    }

    void AddEnemy( EnemySpawnerScript spawner ) {
        EnemyScript enemy = spawner.SpawnEnemy();
        enemies.Add(enemy);
    }

    IEnumerator SpawnRoutine( EnemySpawnerScript spawner ) {
        float min = spawner.SpawnInterval.Min;
        float max = spawner.SpawnInterval.Max;
        float delay = Random.Range( min, max ) / 1000;
        yield return new WaitForSeconds( delay );
        AddEnemy(spawner);
        StartCoroutine( SpawnRoutine( spawner ) );
    }

}
