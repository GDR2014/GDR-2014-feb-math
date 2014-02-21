using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {

    public int maxEnemies = 10;
    public EnemySpawnerScript leftSpawner, rightSpawner;

    private List<EnemyScript> enemies;
    public PlayerScript player;

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
        if( spawner == leftSpawner ) {
            player.leftEnemies.Enqueue( enemy );
        }
        enemies.Add(enemy);
    }

    IEnumerator SpawnRoutine( EnemySpawnerScript spawner ) {
        if( spawner == null ) yield break;
        float min = spawner.SpawnInterval.Min;
        float max = spawner.SpawnInterval.Max;
        float delay = Random.Range( min, max ) / 1000;
        yield return new WaitForSeconds( delay );
        if( enemies.Count < maxEnemies ) AddEnemy(spawner);
        StartCoroutine( SpawnRoutine( spawner ) );
    }

}
