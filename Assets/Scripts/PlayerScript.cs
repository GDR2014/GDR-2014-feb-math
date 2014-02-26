using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NumberGroupScript))]
public class PlayerScript : MonoBehaviour {

    private NumberGroupScript numberRenderer;
    public AudioClip AttackClip;
    public AudioClip FumbleClip;
    public AudioClip ScoreClip;
    public AudioClip HurtClip;
    public AudioClip DeathClip;

    public ScoreManagerScript scoreManager;
    public EnemyManagerScript enemyManager;

    # region Properties
    // Player's own number as string
    private string _attackString = "";
    public string attackString {
        get { return _attackString; }
        set {
            _attackString = value;
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
            UpdateNumberRenderer();
        }
    }

    # endregion

    void Start() {
        numberRenderer = GetComponent<NumberGroupScript>();
        attackNumber = (int) ( Random.value * 8 ) + 2;
    }

    public bool AttemptAttack(int newNum) {
        EnemyScript leftEnemy = enemyManager.nearestLeftEnemy;
        EnemyScript rightEnemy = enemyManager.nearestRightEnemy;
        if ( leftEnemy  != null ) leftEnemy.UpdateAttackTarget(  attackNumber );
        if ( rightEnemy != null ) rightEnemy.UpdateAttackTarget( attackNumber );

        EnemyScript enemy = null;
        Debug.Log(rightEnemy);
        if( leftEnemy != null && leftEnemy.attackTarget == newNum ) {
            enemy = leftEnemy;
            enemyManager.leftEnemies.Remove( enemy );
        }
        else if( rightEnemy != null && rightEnemy.attackTarget == newNum ) {
            enemy = rightEnemy;
            enemyManager.rightEnemies.Remove( enemy );   
        }
        #region debug
        // TODO: ¤¤¤¤¤¤¤¤
        string enemyString = enemy == null ? "null" : enemy == rightEnemy ? "right enemy" : enemy == leftEnemy ? "left enemy" : "weird";
        Debug.Log("Attempting attack. Number is " + newNum + ". Target is " + enemyString);
        // TODO: ########
        #endregion
        if( enemy == null ) {
            Fumble();
            return false;
        }
        Attack(enemy);
        return true;
    }

    void Fumble() {
        PlayClip( FumbleClip );
        Debug.Log("Fumble!");
    }

    void Attack( EnemyScript enemy ) {
        StartCoroutine(ChargeTowardsAndDestroy( enemy ));
    }

    public float ATTACK_THRESHOLD = .02f;
    public float ATTACK_SPEED = .2f;
    IEnumerator ChargeTowardsAndDestroy( EnemyScript enemy ) {
        PlayClip(AttackClip);
        Vector2 pos = transform.position;
        while ( Mathf.Abs( pos.x - enemy.transform.position.x ) > ATTACK_THRESHOLD ) {
            // TODO: Use tweening to give the animation some personality
            pos.x = Mathf.MoveTowards( pos.x, enemy.transform.position.x, ATTACK_SPEED );
            transform.position = pos;
            yield return null;
        }
        attackNumber = enemy.attackTarget;
        enemyManager.RemoveEnemy(enemy);
        PlayClip(ScoreClip);
        scoreManager.Score++;
        enemy.Recycle();
    } 

    void UpdateNumberRenderer() {
        numberRenderer.value = attackNumber;
        numberRenderer.UpdateValue();
    }

    void PlayClip( AudioClip clip ) {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
