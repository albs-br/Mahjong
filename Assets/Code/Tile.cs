using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsBlocked { get; set; }
    public bool IsSelected { get; set; }
    
    public TileLine TileLine { get; set; }

    public Tile()
    {
        IsBlocked = false;
        IsSelected = false;
    }
}