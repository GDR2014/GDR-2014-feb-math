using System;
using System.Collections;
using UnityEngine;

public class NumberGroupScript : MonoBehaviour {

    public int value;
    public float numberSpacing = 1.2f;
    public float yOffset = 0.4f;
    public ChangableSpriteScript numberPrefab;

    void Start() {
        numberPrefab.CreatePool();
        UpdateValue();
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
            int digit = substring != "-" ? Int32.Parse( substring ) : 10;
            // Create the graphic
            ChangableSpriteScript child = numberPrefab.Spawn();
            child.transform.parent = transform;
            child.transform.localPosition = new Vector3(i * numberSpacing, yOffset, 0);
            child.spriteIndex = digit;
            child.UpdateSprite(); // TODO: Find a workaround for calling this twice (it's also called in child.OnEnable() )
        }
    }
}