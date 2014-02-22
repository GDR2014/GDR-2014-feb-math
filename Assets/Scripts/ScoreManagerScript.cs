using UnityEngine;

public class ScoreManagerScript : MonoBehaviour {

    public int Score = 0;

    public Rect ScorePosition;

    void OnGUI() {
        // TODO: Temporary solution. Use some sort of renderer instead
        float relativeWidth = Screen.width / InputScript.nativeWidth;
        float relativeHeight = Screen.height / InputScript.nativeHeight;
        GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( relativeWidth, relativeHeight, 1.0f ) );

        GUI.Label(ScorePosition, "Score: " + Score);
    }
}
