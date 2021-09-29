using UnityEngine;

public class MapVisualization : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void ToSprite(Texture2D texture)
    {
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
