using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFloor
{
    public IList<TileLine> TileLines { get; set; }

    public Game Game { get; set; }

    public TileFloor()
    {
        this.TileLines = new List<TileLine>();
    }

    public TileLine AddTileLine()
    {
        var tileLine = new TileLine();
        tileLine.Index = this.TileLines.Count;
        tileLine.TileFloor = this;
        
        this.TileLines.Add(tileLine);

        return tileLine;
    }
}