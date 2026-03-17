using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLine
{
    public int Index { get; set; }

    public TileFloor TileFloor { get; set; }

    public IList<Tile> Tiles { get; set; }

    public TileLine(int index)
    {
        this.Index = index;
        this.Tiles = new List<Tile>();
    }
}