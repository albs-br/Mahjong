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

        var game = tile.TileLine.TileFloor.Game;

        if(!tile.IsSelected)
        {
            if(game.TileSelected == null)
            {
                // Set sprite gray (selected)
                renderer.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                tile.IsSelected = true;
                game.TileSelected = tile;
            }
            else
            {
                if(game.TileSelected.TileType == tile.TileType)
                {
                    // Remove both tiles
                    Debug.Log("Remove both tiles");
                    //game.TileSelected
                }
            }
        }
        else
        {
            // Remove gray (unselected)
            renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            tile.IsSelected = false;
            game.TileSelected = null;
        }
    }
}
