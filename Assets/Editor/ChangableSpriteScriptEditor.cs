using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( ChangableSpriteScript ) )]
public class ChangableSpriteScriptEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        ChangableSpriteScript script = (ChangableSpriteScript) target;
        if( GUI.changed ) {
            script.UpdateSprite();
            GUI.changed = false;
        }
    }
}