using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table
{
    public int NumberOfColumns { get; set; }
    public int NumberOfLines { get; set; }

    public IList<string> Lines { get; set; }
    
    private IList<string> TempLines { get; set; }
    private IList<TilePosition> FreeTiles { get; set; }
    
    // public IList<Pair> Pairs { get; set; }

    public Table(int numberOfColumns, int numberOfLines)
    {
        this.NumberOfColumns = numberOfColumns;
        this.NumberOfLines = numberOfLines;
        this.Lines = new List<string>();
        this.TempLines = new List<string>();
        this.FreeTiles = new List<TilePosition>();
    }

    public void SortTable()
    {
        // sort tiles for this table

        // step 1: solve puzzle, storing pairs of free unmarked tiles (number of floor, line, and tile position)

        // step 2: loop through pairs, creating the Game object classes with TileFloors, TileLines and Tiles

        this.TempLines.Clear();
        this.TempLines = this.Lines.ToList();

        this.GetFreeTiles();
        this.GetFreeTiles();
    }

    private void GetFreeTiles()
    {
        Debug.Log("-----------");
        
        for(int i=0; i<this.TempLines.Count; i++)
        {
            var line = this.TempLines[i];

            // // Counts characters from the start of the string as long as they are '\0'
            // int n = line.TakeWhile(c => c == '0').Count();            
            // tileLine.TileOffsetLeft = n;

            for(int j=0; j<line.Length; j++)
            {
                if(line[j] == '1')
                {
                    bool hasActiveTileAtLeft = false;
                    if(j > 0 && line[j-1] == '1')
                    {
                        hasActiveTileAtLeft = true;
                    }
                    
                    bool hasActiveTileAtRight = false;
                    if((j < line.Length - 1) && line[j+1] == '1')
                    {
                        hasActiveTileAtRight = true;
                    }

                    if(!hasActiveTileAtLeft || !hasActiveTileAtRight)
                    {
                        Debug.Log($"Free tile found at line {i}, cell {j}");
                        
                        // add to free tiles list
                        this.FreeTiles.Add(
                            new TilePosition
                            {
                                Floor = 0, // TODO
                                Line = j,
                                Tile = i
                            }
                        );

                        // remove tile from list
                        char[] chars = line.ToCharArray();
                        chars[j] = '0';
                        this.TempLines[i] = new string(chars);

                        Debug.Log(this.TempLines[i]);
                    }
                }

            }
        }

    }
}

public struct TilePosition
{
    public int Floor { get; set; }
    public int Line { get; set; }
    public int Tile { get; set; }
}