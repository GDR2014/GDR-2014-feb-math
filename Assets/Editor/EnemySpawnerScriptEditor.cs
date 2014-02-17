using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( EnemySpawnerScript ) )]
public class EnemySpawnerScriptEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        // TOOD: Make pretty editor for dictionaries
    }

}