using System;
using UnityEngine;

public class InputScript : MonoBehaviour {
    private string input = "";
    public PlayerScript playerScript;
    public bool clearOnFailedAttack = false;
    #region Unity methods
    private void Start() {}

    private void Update() {


    }
    #endregion
    private bool checkInput( out int parsed ) {
        bool canParse = Int32.TryParse( input, out parsed );
        if( !canParse ) {
            ReactOnInvalidInput();
            return false;
        }
        ReactOnValidInput();
        return true;
    }

    private void ReactOnInvalidInput() {}

    private void ReactOnValidInput() {}
    #region GUI
    // TODO: Fix this mess. Input should not keep firing if enter/return is held down. :/
    public Rect inputPosition;
    private bool inputHasFired = false;
    private void OnGUI() {
        GUIStyle centerTextStyle = new GUIStyle( GUI.skin.textField ) {alignment = TextAnchor.MiddleCenter};
        input = GUI.TextField( inputPosition, input, centerTextStyle );

        KeyCode c = Event.current.keyCode;
        //Debug.Log(c);
        if( c == KeyCode.Return || c == KeyCode.KeypadEnter ) {
            //Debug.Log("Enter/Return pressed. Input has fired = " + inputHasFired);
            if( inputHasFired ) return;
            int inputAsInt;
            bool inputIsValid = checkInput( out inputAsInt );
            if( inputIsValid ) playerScript.AttemptAttack( inputAsInt );
            inputHasFired = true;
        } else if( !Event.current.isKey) inputHasFired = false;
    }
    #endregion
}