using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLine
{
    public IList<Tile> Tiles { get; set; }

    public TileLine()
    {
        Tiles = new List<Tile>();
    }
}