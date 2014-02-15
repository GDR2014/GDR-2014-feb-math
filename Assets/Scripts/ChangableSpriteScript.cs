using UnityEngine;

[ExecuteInEditMode]
[RequireComponent( typeof( SpriteRenderer ) )]
public class ChangableSpriteScript : MonoBehaviour {

    public int spriteIndex = 0;
    public Sprite[] sprites;

    protected new SpriteRenderer renderer;

    // Use this for initialization
    private void Start() {
        renderer = GetComponent<SpriteRenderer>();
        if( spriteIndex >= 0 && spriteIndex < sprites.Length )
            UpdateSprite();
    }

    // Update is called once per frame
    public void UpdateSprite() {
        renderer.sprite = sprites[spriteIndex];
    }
}