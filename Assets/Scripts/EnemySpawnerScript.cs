using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEditor;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour {

    #region Fields
    // The float is a relative probablitity for an operator to be chosen
    public Dictionary<float, Operator> PossibleOperators = new Dictionary<float, Operator>();
    public Operator[] __TemporaryPossibleOperators;

    public EnemyDirection Direction;
    public IntervalBoundsInMillis SpawnInterval;
    #endregion

    #region Unity methods
    void Start() {
        StartCoroutine( "SpawnCycle" );
    }
    
    void Update () {}
    #endregion

    void StopSpawning() {
        //StopAllCoroutines(); // <-- Should work as well, as long as SpawnCycle is the only coroutine running
        StopCoroutine("SpawnCycle");
    }

    IEnumerator SpawnCycle() {
        float delay = Random.Range( SpawnInterval.Min, SpawnInterval.Max );
        yield return new WaitForSeconds( delay / 1000 );
        StartCoroutine( "SpawnCycle" );
    }


    #region Editor helpers
    public enum EnemyDirection {
        LEFT, RIGHT
    }

    [System.Serializable]
    public class IntervalBoundsInMillis {
        public float Min = 500;
        public float Max = 2000;
    }
    #endregion

}
