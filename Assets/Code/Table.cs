using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table
{
    public int NumberOfColumns { get; set; }
    public int NumberOfLines { get; set; }

    // TODO: make properties public and fields private

    public IList<string> Lines { get; set; }
    
    private IList<string> TempLines { get; set; }
    
    private IList<Pair> Pairs { get; set; }



    string[] tileTypes_Regular = {
        "bamboo1",
        "bamboo2",
        "bamboo3",
        "bamboo4",
        "bamboo5",
        "bamboo6",
        "bamboo7",
        "bamboo8",
        "bamboo9",
        "circle1",
        "circle2",
        "circle3",
        "circle4",
        "circle5",
        "circle6",
        "circle7",
        "circle8",
        "circle9",
        "pinyin1",
        "pinyin10",
        "pinyin11",
        "pinyin12",
        "pinyin13",
        "pinyin14",
        "pinyin15",
        "pinyin2",
        "pinyin3",
        "pinyin4",
        "pinyin5",
        "pinyin6",
        "pinyin7",
        "pinyin8",
        "pinyin9",
    };

    string[] tileTypes_Flowers = {
        "lotus",
        "orchid",
        "peony",
        "chrysanthemum",
    };

    string[] tileTypes_Seasons = {
        "spring",
        "summer",
        "winter",
        "fall",
    };


    private IList<string> tileTypes_Regular_Temp;

    public Table(int numberOfColumns, int numberOfLines)
    {
        this.NumberOfColumns = numberOfColumns;
        this.NumberOfLines = numberOfLines;
        this.Lines = new List<string>();
        this.TempLines = new List<string>();
        this.Pairs = new List<Pair>();
    }

    /// Sort tiles for this table
    public IList<Pair> SortTable()
    {
        // step 1: solve puzzle, storing pairs of free unmarked tiles (number of floor, line, and tile position)

        // step 2: loop through pairs, creating the Game object classes with TileFloors, TileLines and Tiles

        this.tileTypes_Regular_Temp = this.tileTypes_Regular.ToList();

        this.TempLines.Clear();
        this.TempLines = this.Lines.ToList();

        do
        {
            this.GetFreeTiles();
        } while (!this.IsEmpty());

        Debug.Log($"this.Pairs.Count: {this.Pairs.Count}");

        return this.Pairs;
    }

    /// Return true if the table is all sorted
    private bool IsEmpty()
    {
        bool isEmpty = true;
        for(int i=0; i < this.TempLines.Count; i++)
        {
            if(this.TempLines[i].Contains("1"))
            {
                isEmpty = false;
                break;
            }
        }
        return isEmpty;
    }

    // get free tiles for one iteration
    private void GetFreeTiles()
    {
        IList<TilePosition> freeTiles = new List<TilePosition>();
        
        System.Random random = new System.Random();


        Debug.Log("-----------");

        IList<string> outputLines = new List<string>();

        for(int i=0; i < this.TempLines.Count; i++)
        {
            var newLine = "";

            Debug.Log("Line before: " + this.TempLines[i]);

            for(int j=0; j<this.TempLines[i].Length; j++)
            {
                var newChar = this.TempLines[i][j];

                if(this.TempLines[i][j] == '1')
                {
                    bool hasActiveTileAtLeft = false;
                    if(j > 0 && this.TempLines[i][j-1] == '1')
                    {
                        hasActiveTileAtLeft = true;
                    }
                    
                    bool hasActiveTileAtRight = false;
                    if((j < this.TempLines[i].Length - 1) && this.TempLines[i][j+1] == '1')
                    {
                        hasActiveTileAtRight = true;
                    }

                    if(!hasActiveTileAtLeft || !hasActiveTileAtRight)
                    {
                        Debug.Log($"Free tile found at line {i}, cell {j}");
                        

                        // add to free tiles list
                        freeTiles.Add(
                            new TilePosition
                            {
                                Floor = 0, // TODO
                                Line = i,
                                Tile = j
                            }
                        );

                        // remove tile from list
                        // char[] chars = this.TempLines[i].ToCharArray();
                        // chars[j] = '0';
                        // this.TempLines[i] = new string(chars);

                        newChar = '0';

                        //Debug.Log("Line after: " + this.TempLines[i]);
                    }
                }

                newLine += newChar;
            }

            outputLines.Add(newLine);

            Debug.Log("Line after: " + newLine);
        }

        // copy outputLines to tempLines
        for(int i=0; i < this.TempLines.Count; i++)
        {
            this.TempLines[i] = outputLines[i];
        }

        // freeTiles.Count must be even (?)
        if(freeTiles.Count % 2 != 0)
        {
            throw new Exception($"Free tiles not even.");
        }
        




        // do while here until Free tiles list is empty
        do
        {
            // sort 2 free tiles
            int freeTile_1, freeTile_2;
            do
            {
                freeTile_1 = random.Next(freeTiles.Count);
                freeTile_2 = random.Next(freeTiles.Count);
            } while (freeTile_1 == freeTile_2);
            
            // sort tile type
            int tileTypeIndex = random.Next(this.tileTypes_Regular_Temp.Count);

            
            var newPair = new Pair
            {
                Tile_1 = freeTiles[freeTile_1],
                Tile_2 = freeTiles[freeTile_2],
                TileType = this.tileTypes_Regular_Temp[tileTypeIndex]
            };
            this.Pairs.Add(newPair);


            // remove tile type from list
            this.tileTypes_Regular_Temp.RemoveAt(tileTypeIndex);


            Debug.Log($"{newPair}");
            
            // --- remove these 2 free tiles from list
            
            // first, set them to null
            freeTiles[freeTile_1] = null;
            freeTiles[freeTile_2] = null;

            // loop all list removing nulls
            int k=0;
            do
            {
                if(freeTiles[k] == null)
                {
                    freeTiles.RemoveAt(k);
                }
                else
                {
                    k++;
                }
            } while (k <= freeTiles.Count - 1);

            // Debug.Log($"freeTiles.Count: {freeTiles.Count}");

        } while (freeTiles.Count > 0);
         
        

    }
}

public class TilePosition
{
    public int Floor { get; set; }
    public int Line { get; set; }
    public int Tile { get; set; }

    public override string ToString()
    {
        return $"TilePosition, Floor: {this.Floor}, Line: {this.Line}, Tile: {this.Tile}";
    }
}

public class Pair
{
    public TilePosition Tile_1 { get; set; }
    public TilePosition Tile_2 { get; set; }
    public string TileType { get; set; }

    public override string ToString()
    {
        return $"Pair, Tile_1: {this.Tile_1}, Tile_2: {this.Tile_2}, TileType: {this.TileType}";
    }
}