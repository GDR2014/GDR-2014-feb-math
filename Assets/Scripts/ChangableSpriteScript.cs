using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class ChangableSpriteScript : MonoBehaviour {

    public int spriteIndex = 0;
    public Sprite[] sprites;

    protected new SpriteRenderer renderer;

    private void OnEnable() {
        renderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void UpdateSprite() {
        renderer.sprite = sprites[spriteIndex];
    }
}