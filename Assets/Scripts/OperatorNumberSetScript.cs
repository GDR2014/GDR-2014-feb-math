using System.Collections;
using Assets.Scripts.Data;
using UnityEngine;

public class OperatorNumberSetScript : MonoBehaviour {

    public float OperatorOffset = -1.3f;

    public int value = 0;
    public Operator op = Operator.PLUS;
    public OperatorSide operatorSide = OperatorSide.LEFT;

    public NumberGroupScript numberGroupPrefab;
    private NumberGroupScript numberScript;

    public ChangableSpriteScript operatorPrefab;
    private ChangableSpriteScript operatorScript;

    private void Start() {
        numberScript = numberGroupPrefab.Spawn();
        numberScript.transform.parent = transform;
        numberScript.yOffset = 0;
        numberScript.transform.localPosition = new Vector3();

        operatorScript = operatorPrefab.Spawn();
        operatorScript.transform.parent = transform;
        StartCoroutine( SetOperatorPosition() );
        UpdateSetRenderer();
    }

    protected IEnumerator SetOperatorPosition() {
        yield return new WaitForEndOfFrame();
        float posX = 0;
        switch( operatorSide ) {
            case OperatorSide.LEFT:
                posX = OperatorOffset;
                break;
            case OperatorSide.RIGHT:
                posX = CalculateOperatorOffsetRight();
                break;
        }
        operatorScript.transform.localPosition = new Vector2( posX, 0 );
    }

    float CalculateOperatorOffsetRight() {
        int numberLength = numberScript.value.ToString().Length;
        return numberLength * numberScript.numberSpacing;
    }

    public void UpdateSetRenderer() {
        operatorScript.spriteIndex = (int) op;
        operatorScript.UpdateSprite();

        numberScript.value = value;
        numberScript.UpdateValue();
    }

    public enum OperatorSide {
        LEFT, RIGHT
    }
}