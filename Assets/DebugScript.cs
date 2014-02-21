using UnityEngine;

public class DebugScript : MonoBehaviour {
    void OnGUI() {
        if( Event.current.keyCode == KeyCode.R )
            Application.LoadLevel( 0 );
    }
}
