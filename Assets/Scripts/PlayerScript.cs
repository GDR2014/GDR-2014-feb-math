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
        EnemyScript leftEnemy = null;
        EnemyScript rightEnemy = null;
        if ( leftEnemies.Count  > 0 ) leftEnemy  = leftEnemies.Peek();
        if ( rightEnemies.Count > 0 ) rightEnemy = rightEnemies.Peek();
        if ( leftEnemy  != null ) leftEnemy.UpdateAttackTarget(  attackNumber );
        if ( rightEnemy != null ) rightEnemy.UpdateAttackTarget( attackNumber );

        EnemyScript enemy = null;
        Debug.Log(rightEnemy);
        if( leftEnemy != null && leftEnemy.attackTarget == newNum ) enemy = leftEnemies.Dequeue();
        else if( rightEnemy != null && rightEnemy.attackTarget == newNum ) enemy = rightEnemies.Dequeue();
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
