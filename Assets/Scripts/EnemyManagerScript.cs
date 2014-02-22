using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {

    public int maxEnemies = 10;
    public EnemySpawnerScript leftSpawner, rightSpawner;

    public PlayerScript player;

    public int EnemyCount {
        get { return player.leftEnemies.Count + player.rightEnemies.Count; }
    }

    void Start() {
        StartCoroutine( SpawnRoutine( leftSpawner ) );
        StartCoroutine( SpawnRoutine( rightSpawner ) );
    }

    public void StopSpawning() {
        StopAllCoroutines();
    }

    void AddEnemy( EnemySpawnerScript spawner ) {
        EnemyScript enemy = spawner.SpawnEnemy();
        Queue<EnemyScript> queue = spawner == leftSpawner ? player.leftEnemies : player.rightEnemies;
        queue.Enqueue( enemy );
    }

    IEnumerator SpawnRoutine( EnemySpawnerScript spawner ) {
        if( spawner == null ) yield break;
        float min = spawner.SpawnInterval.Min;
        float max = spawner.SpawnInterval.Max;
        float delay = Random.Range( min, max ) / 1000;
        yield return new WaitForSeconds( delay );
        if( EnemyCount < maxEnemies ) AddEnemy(spawner);
        StartCoroutine( SpawnRoutine( spawner ) );
    }

}
