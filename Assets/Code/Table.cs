using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table
{
    public int NumberOfColumns { get; set; }
    public int NumberOfLines { get; set; }

    public IList<string> Lines { get; set; }
    private IList<string> TempLines { get; set; }
    
    // public IList<Pair> Pairs { get; set; }

    public Table(int numberOfColumns, int numberOfLines)
    {
        this.NumberOfColumns = numberOfColumns;
        this.NumberOfLines = numberOfLines;
        this.Lines = new List<string>();
    }

    public void SortTable()
    {
        // sort tiles for this table

        // step 1: solve puzzle, storing pairs of free tiles (number of floor, line, and tile)

        // step 2: loop through pairs, creating the Game object classes with TileFloors, TileLines and Tiles

        this.GetFreeTiles();
    }

    private void GetFreeTiles()
    {
        // TODO: copy Lines to tempLines and use it
        
        for(int i=0; i<this.Lines.Count; i++)
        {
            var line = this.Lines[i];

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
                        // add to free tiles list
                        Debug.Log($"Free tile found at line {i}, cell {j}");
                    }
                }

            }
        }

    }
}