using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NumberGroupScript))]
public class PlayerScript : MonoBehaviour {

    private NumberGroupScript numberRenderer;

    # region Properties
    // Player's own number as string
    private string _attackString = "";
    public string attackString {
        get { return _attackString; }
        set {
            _attackString = value;
            UpdateImmediateEnemies();
            UpdateNumberRenderer();
        }
    }

    // Player's own number as integer
    public int attackNumber {
        get {
            int val;
            bool canParse = Int32.TryParse( attackString, out val );
            if( !canParse ) throw new Exception( "Attack Number can not be parsed!" );
            return val;
        }
        set {
            attackString = value.ToString();
            UpdateImmediateEnemies();
            UpdateNumberRenderer();
        }
    }

    // Enemies to the left
    public Queue<EnemyScript> leftEnemies;

    // Enemies to the right
    public Queue<EnemyScript> rightEnemies;

    # endregion

    void Start() {
        numberRenderer = GetComponent<NumberGroupScript>();
        attackNumber = (int) ( Random.value * 8 ) + 2;
        rightEnemies = new Queue<EnemyScript>();
        leftEnemies = new Queue<EnemyScript>();
    }

    public bool AttemptAttack(int newNum) {
        EnemyScript leftEnemy = leftEnemies.Peek();
        EnemyScript rightEnemy = rightEnemies.Peek();
        // TODO: ¤¤¤¤¤¤¤¤
        // Only set attackNumber if attack was successful
        attackNumber = newNum;
        // TODO: ########
        EnemyScript enemy = null;
        if( leftEnemy != null && leftEnemy.attackTarget == newNum ) enemy = leftEnemy;
        else if( rightEnemy != null && rightEnemy.attackTarget == newNum ) enemy = rightEnemy;
        // TODO: ¤¤¤¤¤¤¤¤
        string enemyString = enemy == null ? "null" : enemy == rightEnemy ? "right enemy" : enemy == leftEnemy ? "left enemy" : "weird";
        Debug.Log("Attempting attack. Number is " + newNum + ". Target is " + enemyString);
        // TODO: ########
        if( enemy == null ) {
            Fumble();
            return false;
        }
        Attack(enemy);
        return true;
    }

    void Fumble() {
        Debug.Log("Fumble!");
    }

    void Attack( EnemyScript enemy ) {
        Vector2 pos = transform.position;
        pos.x = enemy.transform.position.x;
        transform.position = pos;

        attackNumber = enemy.attackTarget;
        enemy.Recycle();
    }

    private void UpdateImmediateEnemies() {
        if( _left != null ) _left.UpdateAttackTarget( attackNumber );
        if ( _right != null ) _right.UpdateAttackTarget( attackNumber );
    }

    void UpdateNumberRenderer() {
        numberRenderer.value = attackNumber;
        numberRenderer.UpdateValue();
    }

}
