using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesAnimation
{
    public int Counter { get; set; }
    public Tile Tile_1 { get; set; }
    public Tile Tile_2 { get; set; }
    

    public TilesAnimation(Tile tile_1, Tile tile_2)
    {
        this.Counter = 0;
        this.Tile_1 = tile_1;
        this.Tile_2 = tile_2;
    }
}