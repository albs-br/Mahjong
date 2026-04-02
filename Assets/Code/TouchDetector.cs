using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    void OnMouseDown()
    {
        // This code runs when the sprite is touched or clicked
        
        // Debug.Log("Sprite Touched: " + gameObject.name);
        // Debug.Log("sorting order: " + this.gameObject.GetComponent<Renderer>().sortingOrder);
        // Debug.Log("IsBlocked: " + this.gameObject.GetComponent<Tile>().IsBlocked);

        var tile = gameObject.GetComponent<Tile>();

        if(tile.IsBlocked)
        {
            Debug.Log("IsBlocked");
            return;
        }


        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // Debug.Log($"tile.Index: {tile.Index}");
        var game = tile.Game; //tile.TileLine.TileFloor.Game;

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
                    
                    // Debug.Log("Remove both tiles");
                    
                    // // Call this method to hide/deactivate the GameObject
                    // this.gameObject.SetActive(false);

                    //Debug.Log("Removing tile " + this.gameObject.GetComponent<Tile>().Index);
                    this.gameObject.GetComponent<Tile>().Remove();

                    game.TileSelected.Remove();
                    game.TileSelected = null;
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
