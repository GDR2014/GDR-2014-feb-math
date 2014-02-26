using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManagerScript : MonoBehaviour {

    public int maxEnemies = 10;
    public EnemySpawnerScript leftSpawner, rightSpawner;

    public PlayerScript player;
    public List<EnemyScript> leftEnemies;
    public List<EnemyScript> rightEnemies;

    // Sorting layer IDs
    private const int NORMAL = 2;
    private const int FRONT = 4;

    // Highlights
    private static readonly Color NORMAL_COLOR = new Color( 1, 1, 1 );
    private static readonly Color FRONT_COLOR = new Color( 1f, .5f, .5f );

    protected EnemyScript _nearestLeft = null;
    public EnemyScript nearestLeftEnemy {
        get { return _nearestLeft; }
        set {
            removeFrontEffects(_nearestLeft);
            _nearestLeft = value;
            StartCoroutine( enableFrontEffects( _nearestLeft ) );
        }
    }
    protected EnemyScript _nearestRight = null;
    public EnemyScript nearestRightEnemy {
        get { return _nearestRight; }
        set {
            removeFrontEffects( _nearestRight );
            _nearestRight = value;
            StartCoroutine( enableFrontEffects( _nearestRight ) );
        }
    }

    private void Start() {
        leftEnemies = new List<EnemyScript>();
        rightEnemies = new List<EnemyScript>();
        StartCoroutine( SpawnRoutine( leftSpawner ) );
        StartCoroutine( SpawnRoutine( rightSpawner ) );
    }

    public int EnemyCount {
        get { return leftEnemies.Count + rightEnemies.Count; }
    }
    public void StopSpawning() {
        StopAllCoroutines();
    }

    private void AddEnemy( EnemySpawnerScript spawner ) {
        bool isLeft = spawner == leftSpawner;
        EnemyScript enemy = spawner.SpawnEnemy();
        List<EnemyScript> list = isLeft ? leftEnemies : rightEnemies;
        list.Add( enemy );
        EnemyScript currentClosest = isLeft ? nearestLeftEnemy : nearestRightEnemy;
        float currentDistance = currentClosest == null ? float.MaxValue : getDistanceToPlayer( currentClosest );
        float newDistance = getDistanceToPlayer( enemy );
        if( newDistance >= currentDistance ) return;
        if( isLeft ) nearestLeftEnemy = enemy;
        else nearestRightEnemy = enemy;
    }

    public void RemoveEnemy( EnemyScript enemy ) {
        bool isLeft = enemy == nearestLeftEnemy;
        List<EnemyScript> list = isLeft ? leftEnemies : rightEnemies;
        list.Remove( enemy );
        if ( isLeft ) nearestLeftEnemy = findNearestEnemy( list );
        else nearestRightEnemy = findNearestEnemy( list );
    }

    private IEnumerator SpawnRoutine( EnemySpawnerScript spawner ) {
        if( spawner == null ) yield break;
        float min = spawner.SpawnInterval.Min;
        float max = spawner.SpawnInterval.Max;
        float delay = Random.Range( min, max ) / 1000;
        yield return new WaitForSeconds( delay );
        if( EnemyCount < maxEnemies ) AddEnemy( spawner );
        StartCoroutine( SpawnRoutine( spawner ) );
    }

    protected IEnumerator enableFrontEffects( EnemyScript enemy ) {
        yield return new WaitForEndOfFrame();
        Debug.Log("Attempting to enable FrontFX. Enemy is " + enemy);
        if( enemy == null ) yield break;
        SpriteRenderer[] renderers = enemy.GetComponentsInChildren<SpriteRenderer>();
        Debug.Log("Number of children found in " + enemy + ": " + enemy.GetComponentsInChildren<Transform>().Length);
        foreach( SpriteRenderer renderer in renderers ) {
            Debug.Log("Coloring renderer for " + renderer.gameObject);
            renderer.sortingLayerID = FRONT;
            renderer.color = FRONT_COLOR;
        }
    }

    protected void removeFrontEffects( EnemyScript enemy ) {
        if ( enemy == null ) return;
        SpriteRenderer[] renderers = enemy.GetComponentsInChildren<SpriteRenderer>();
        foreach ( SpriteRenderer renderer in renderers ) {
            renderer.sortingLayerID = NORMAL;
            renderer.color = NORMAL_COLOR;
        }
    }

    protected EnemyScript findNearestEnemy( List<EnemyScript> list ) {
        float closestDistance = float.MaxValue;
        EnemyScript nearestEnemy = null;
        foreach( EnemyScript enemy in list ) {
            float distance = getDistanceToPlayer( enemy );
            if( distance >= closestDistance ) continue;
            closestDistance = distance;
            nearestEnemy = enemy;
        }
        return nearestEnemy;
    }

    protected float getDistanceToPlayer( EnemyScript enemy ) {
        return Mathf.Abs( player.transform.position.x - enemy.transform.position.x );
    }
}