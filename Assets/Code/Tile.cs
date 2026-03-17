using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string TileType { get; set; }

    public bool IsBlocked 
    { 
        get
        {
            return (TileAtLeft != null && TileAtRight != null);
        }
    }
    
    public bool IsSelected { get; set; }
    
    public TileLine TileLine { get; set; }
    public Tile TileAtLeft { get; set; }
    public Tile TileAtRight { get; set; }



    /*
    618 - 70 = 
    859 - 73 = 
    */
    public static float TotalWidth = 618; // in pixels
    public static float TotalHeight = 859; // in pixels
    
    // Size of the tile top, discarding the 3D effect
    public static float Width_2D = TotalWidth - 70; // in pixels
    public static float Height_2D = TotalHeight - 73; // in pixels



    public Tile()
    {
        IsSelected = false;
    }
}