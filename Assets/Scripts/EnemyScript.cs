using System.Collections;
using Assets.Scripts.Data;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public PlayerScript player;
    public int attackTarget;
    public int attackModifier;
    public Operator attackOperator;
    public float Speed;
    public float AttackRange = 1f;

    IEnumerator Move() {
        Vector2 pos = transform.position;
        float playerX = player.transform.position.x;
        float distance = Mathf.Abs( pos.x - playerX );
        if( distance <= AttackRange ) yield break; // TODO: Maybe play some sort of attack animation here
        pos.x += Speed * Time.deltaTime;
        transform.position = pos;
    }

    public void UpdateAttackTarget( int playerVal ) {
        switch( attackOperator ) {
            case Operator.PLUS:
                attackTarget = playerVal + attackModifier;
                break;
            case Operator.MINUS:
                attackTarget = playerVal - attackModifier;
                break;
            case Operator.MULTIPLY:
                attackTarget = playerVal * attackModifier;
                break;
            case Operator.DIVIDE:
                attackTarget = playerVal / attackModifier;
                break;
        }
    }
}