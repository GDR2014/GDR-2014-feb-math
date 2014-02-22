using System.Collections;
using UnityEngine;

public class DebugScript : MonoBehaviour {

    public float ResetCooldown = 2000; // ms
    protected bool canReset = false;

    void Awake() {
        canReset = false;
        StartCoroutine( Countdown() );
    }

    IEnumerator Countdown() {
        yield return new WaitForSeconds( ResetCooldown / 1000 );
        canReset = true;
    }

    void OnGUI() {
        if( Event.current.keyCode == KeyCode.R && canReset ) {
            Application.LoadLevel( 0 );
        }
    }
}
