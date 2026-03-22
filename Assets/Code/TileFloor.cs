using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFloor
{
    public int Index { get; set; }

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
        
        for(int i=0; i < this.Game.Table.NumberOfColumns; i++)
        {
            tileLine.Tiles.Add(null);
        }


        this.TileLines.Add(tileLine);

        return tileLine;
    }
}