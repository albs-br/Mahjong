using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    void OnMouseDown()
    {
        // This code runs when the sprite is touched or clicked
        Debug.Log("Sprite Touched: " + gameObject.name);

        var tile = gameObject.GetComponent<Tile>();

        if(tile.IsBlocked)
        {
            Debug.Log("IsBlocked");
            return;
        }

        // Add your specific action here, e.g., load a level or change a state

        // // Call this method to hide/deactivate the GameObject
        // this.gameObject.SetActive(false);             

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // Set sprite gray (selected)
        if(!tile.IsSelected)
        {
            renderer.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            tile.IsSelected = true;
        }
        else
        {
            renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            tile.IsSelected = false;
        }
    }
}
