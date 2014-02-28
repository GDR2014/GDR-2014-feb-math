using System;
using System.Collections;
using UnityEngine;

public class InputScript : MonoBehaviour {
    private string input = "";
    public PlayerScript playerScript;
    public ScoreManagerScript scoreScript;
    public bool clearOnFailedAttack = false;

    public const float nativeWidth = 800, nativeHeight = 450;

    private void Awake() {
        StartCoroutine( waitToClear() );
    }

    private IEnumerator waitToClear() {
        yield return new WaitForEndOfFrame();
        ClearInput();
    }

    private bool checkInput( out int parsed ) {
        bool canParse = Int32.TryParse( input, out parsed );
        if( !canParse ) {
            ReactOnInvalidInput();
            return false;
        }
        ReactOnValidInput();
        return true;
    }

    private void ReactOnInvalidInput() {
        if( clearOnFailedAttack ) ClearInput();
    }

    private void ReactOnValidInput() {
        ClearInput();
    }

    public void ClearInput() {
        input = "";
    }
    #region GUI
    // TODO: Fix this mess. Input should not keep firing if enter/return is held down. :/
    public Rect inputPosition;
    private bool inputHasFired = false;

    private void OnGUI() {
        bool gameOver = playerScript.Health <= 0;
        float relativeWidth = Screen.width / nativeWidth;
        float relativeHeight = Screen.height / nativeHeight;
        GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( relativeWidth, relativeHeight, 1.0f ) );

        // This is such a bad way to do this, but I just want to be done with it...
        if( !gameOver ) {
            // Health label
            GUI.Label( new Rect(40, 80, 200, 40), "Health: " + playerScript.Health  );
            // Input field
            GUIStyle centerTextStyle = new GUIStyle( GUI.skin.textField ) {alignment = TextAnchor.MiddleCenter};
            GUI.SetNextControlName( "textField" );
            input = GUI.TextField( inputPosition, input, centerTextStyle );
            GUI.FocusControl( "textField" );
            KeyCode c = Event.current.keyCode;
            //Debug.Log(c);
            if( c == KeyCode.Return || c == KeyCode.KeypadEnter ) {
                //Debug.Log("Enter/Return pressed. Input has fired = " + inputHasFired);
                if( inputHasFired ) return;
                int inputAsInt;
                bool inputIsValid = checkInput( out inputAsInt );
                if( inputIsValid ) playerScript.AttemptAttack( inputAsInt );
                inputHasFired = true;
            } else if( !Event.current.isKey ) inputHasFired = false;
        } else {
            GUI.Label( new Rect( 360, 100, 100, 35 ), "Game Over" );
            GUI.Label( new Rect( 355, 140, 100, 35 ), "Your score: " + scoreScript.Score );
            if( GUI.Button(new Rect(350, 180, 100, 35), "Try again" )) Application.LoadLevel( 0 );
        }
    }
    #endregion
}