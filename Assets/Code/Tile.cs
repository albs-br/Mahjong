using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
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

    public Tile()
    {
        IsSelected = false;
    }
}