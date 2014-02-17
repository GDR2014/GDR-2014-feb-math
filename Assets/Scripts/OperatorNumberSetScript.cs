using Assets.Scripts.Data;
using UnityEngine;

[RequireComponent( typeof( NumberGroupScript ) )]
public class OperatorNumberSetScript : MonoBehaviour {

    public float OperatorOffset = -1f;

    public int value = 0;
    public Operator op = Operator.PLUS;

    private NumberGroupScript number;

    private void Start() {
        number = GetComponent<NumberGroupScript>();

    }

    private void Update() {}
}