using System;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerScript : MonoBehaviour {
    #region Fields
    // The float is a relative probablitity for an operator to be chosen
    public Dictionary<float, Operator> PossibleOperators = new Dictionary<float, Operator>();
    public Operator[] __TemporaryPossibleOperators;

    public EnemyScript enemyPrefab;
    public float EnemySpeed;
    public IntervalBoundsInMillis SpawnInterval;
    public OperatorNumberSetScript.OperatorSide operatorSide = OperatorNumberSetScript.OperatorSide.LEFT;

    #endregion
    #region Unity methods
    private void Start() {}
    private void Update() {}
    #endregion
    public EnemyScript SpawnEnemy() {
        EnemyScript e = enemyPrefab.Spawn( transform.position );
        e.Speed = EnemySpeed;
        e.attackOperator = SelectRandomOperator();
        e.attackModifier = SelectRandomNumber( e.attackOperator );

        e.operatorSide = operatorSide;
        return e;
    }

    int SelectRandomNumber( Operator op ) {
        // TODO: Write fancy logic for determining numbers based on the player's number and the operator.
        // E.g. Don't divide 10 with 3, or odd numbers with 2.
        switch( op ) {
            case Operator.PLUS:
            case Operator.MINUS:
            case Operator.MULTIPLY:
            case Operator.DIVIDE:
                return 66;
        }
        return 33;
    }

    Operator SelectRandomOperator() {
        return __TemporaryPossibleOperators[Random.Range( 0, __TemporaryPossibleOperators.Length )];
    }

    #region Editor helpers
    [Serializable]
    public class IntervalBoundsInMillis {
        public float Min = 500;
        public float Max = 2000;
    }
    #endregion
}