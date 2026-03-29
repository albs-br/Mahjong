using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int Index { get; set; }
    public string TileType { get; set; }

    public bool IsActive  { get; set; }
    
    public bool IsBlocked  { get; set; }
    
    public bool IsSelected { get; set; }
    
    public TileLine TileLine { get; set; }

    public Game Game
    {
        get
        {
            return this.TileLine.TileFloor.Game;
        }
    }

    /*
    618 - 70 = 
    859 - 73 = 
    */
    public static float TotalWidth_px = 618; // in pixels
    public static float TotalHeight_px = 859; // in pixels
    
    // Size of the tile top, discarding the 3D effect
    public static float Width_2D_px = TotalWidth_px - 70; // in pixels
    public static float Height_2D_px = TotalHeight_px - 73; // in pixels

    public static float TotalWidth; // in screen units
    public static float TotalHeight; // in screen units

    public static float Width_2D; // in screen units
    public static float Height_2D; // in screen units


    public Tile()
    {
        IsActive = true;
        IsSelected = false;
    }

    public void Remove()
    {
        // Call this method to hide/deactivate the GameObject
        this.gameObject.SetActive(false);
        
        this.IsActive = false;
        
        this.Game.UpdateGame();
    }
}