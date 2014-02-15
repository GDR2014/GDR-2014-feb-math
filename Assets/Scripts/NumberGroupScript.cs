using System;
using System.Collections;
using UnityEngine;

public class NumberGroupScript : MonoBehaviour {

    public int value;
    public float numberSpacing = 1.0f;
    public ChangableSpriteScript numberPrefab;

    void Start() {
        numberPrefab.CreatePool();
        StartCoroutine( Tester() );
    }

    protected IEnumerator Tester() {
        value = 0;
        while( true ) {
            yield return new WaitForSeconds( 1 );
            value++;
            UpdateValue();
            if( value > 11 ) value = 7;
        }
    }

    public void UpdateValue() {
        foreach( ChangableSpriteScript child in GetComponentsInChildren<ChangableSpriteScript>() ) {
            child.Recycle();
        }
        
        String valueString = value.ToString();
        int valueLength = valueString.Length;

        for( int i = 0; i < valueLength; i++ ) {
            // Get the digit
            String substring = valueString.Substring( i, 1 );
            int digit = Int32.Parse( substring );
            // Create the graphic
            ChangableSpriteScript child = numberPrefab.Spawn();
            child.transform.parent = transform;
            child.transform.position = new Vector3(i * numberSpacing, 0, 0);
            child.spriteIndex = digit;
            child.UpdateSprite(); // TODO: Find a workaround for calling this twice (it's also called in child.OnEnable() )
        }
    }
}