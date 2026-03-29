using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table
{
    public int NumberOfFloors { get; set; }
    public int NumberOfLines { get; set; }
    public int NumberOfColumns { get; set; }

    public IList<IList<string>> Floors { get; set; }
    
    private IList<IList<string>> tempFloors;

    private IList<Pair> pairs;
    private IList<string> tileTypes_Regular_Temp;



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



    public Table(int numberOfFloors, int numberOfLines, int numberOfColumns)
    {
        this.NumberOfFloors = numberOfFloors;
        this.NumberOfLines = numberOfLines;
        this.NumberOfColumns = numberOfColumns;
        
        this.Floors = new List<IList<string>>();

        this.tempFloors = new List<IList<string>>();

        this.pairs = new List<Pair>();
    }

    /// Sort tiles for this table
    public IList<Pair> SortTable()
    {
        // step 1: solve puzzle, storing pairs of free unmarked tiles (number of floor, line, and tile position)

        // step 2: loop through pairs, creating the Game object classes with TileFloors, TileLines and Tiles

        this.tileTypes_Regular_Temp = this.tileTypes_Regular.ToList();

        this.tempFloors.Clear();
        for (int i=0; i < this.Floors.Count; i++)
        {
            var newTempFloor = this.Floors[i].ToList();
            this.tempFloors.Add(newTempFloor);
            // this.tempFloors[i] = new List<string>();
            // this.tempFloors[i] = this.Floors[i].ToList();
        }

        do
        {
            this.GetFreeTiles();
        } while (!this.IsEmpty());

        //Debug.Log($"this.pairs.Count: {this.pairs.Count}");

        return this.pairs;
    }

    /// Return true if the table is all sorted
    private bool IsEmpty()
    {
        bool isEmpty = true;
        for(int j=0; j < this.tempFloors.Count; j++) // loop floors
        {
            for(int i=0; i < this.tempFloors[j].Count; i++) // loop lines
            {
                if(this.tempFloors[j][i].Contains("1"))
                {
                    isEmpty = false;
                    break;
                }
            }
        }
        return isEmpty;
    }

    // get free tiles for one iteration
    private void GetFreeTiles()
    {
        IList<TilePosition> freeTiles = new List<TilePosition>();
        
        System.Random random = new System.Random();


        //Debug.Log("-----------");

        IList<IList<string>> outputFloors = new List<IList<string>>();

        for(int k=0; k < this.tempFloors.Count; k++)
        {
            outputFloors.Add(new List<string>());

            for(int i=0; i < this.tempFloors[k].Count; i++)
            {
                var newLine = "";

                //Debug.Log("Line before: " + this.tempFloors[k][i]);

                for(int j=0; j<this.tempFloors[k][i].Length; j++)
                {
                    var newChar = this.tempFloors[k][i][j];

                    if(this.tempFloors[k][i][j] == '1')
                    {
                        bool hasActiveTileAtLeft = false;
                        if(j > 0 && this.tempFloors[k][i][j-1] == '1')
                        {
                            hasActiveTileAtLeft = true;
                        }
                        
                        bool hasActiveTileAtRight = false;
                        if((j < this.tempFloors[k][i].Length - 1) && this.tempFloors[k][i][j+1] == '1')
                        {
                            hasActiveTileAtRight = true;
                        }

                        if(!hasActiveTileAtLeft || !hasActiveTileAtRight)
                        {
                            //Debug.Log($"Free tile found at line {i}, cell {j}");
                            

                            // add to free tiles list
                            freeTiles.Add(
                                new TilePosition
                                {
                                    Floor = k,
                                    Line = i,
                                    Tile = j
                                }
                            );

                            newChar = '0';

                            //Debug.Log("Line after: " + this.tempFloors[k][i]);
                        }
                    }

                    newLine += newChar;
                }

                outputFloors[k].Add(newLine);

                //Debug.Log("Line after: " + newLine);
            }
        }

        // copy outputFloors to tempFloors
        for(int k=0; k < this.tempFloors.Count; k++)
        {
            for(int i=0; i < this.tempFloors[k].Count; i++)
            {
                this.tempFloors[k][i] = outputFloors[k][i];
            }
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
            this.pairs.Add(newPair);


            // remove tile type from list
            this.tileTypes_Regular_Temp.RemoveAt(tileTypeIndex);
            Debug.Log($"this.tileTypes_Regular_Temp: {this.tileTypes_Regular_Temp.Count}");

            if(this.tileTypes_Regular_Temp.Count == 0)
            {
                Debug.Log("this.tileTypes_Regular_Temp = 0");

                // Reload list of tiles
                this.tileTypes_Regular_Temp = this.tileTypes_Regular.ToList();
            }

            // Debug.Log($"{newPair}");
            
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

    public static Table LoadTable_SingleFloorTest()
    {
        var table = new Table(
            numberOfFloors: 1,
            numberOfLines: 5,
            numberOfColumns: 6 // tile width = 1/6 of screen
        );
        // 0 = empty, 1 = tile
        // Must have an even number of tiles
        
        IList<string> floor_0 = new List<string>();
        floor_0.Add("111110");
        floor_0.Add("001111");
        floor_0.Add("011110");
        floor_0.Add("110011");
        floor_0.Add("011111");
        table.Floors.Add(floor_0);

        return table;
    }

    public static Table LoadTable_DoubleFloorTest()
    {
        var table = new Table(
            numberOfFloors: 2,
            numberOfLines: 5,
            numberOfColumns: 6 // tile width = 1/6 of screen
        );
        // 0 = empty, 1 = tile
        // Must have an even number of tiles
        IList<string> floor_0 = new List<string>();
        floor_0.Add("011110");
        floor_0.Add("111111");
        floor_0.Add("011110");
        floor_0.Add("111111");
        floor_0.Add("110011");
        table.Floors.Add(floor_0);

        IList<string> floor_1 = new List<string>();
        floor_1.Add("000000");
        floor_1.Add("001100");
        floor_1.Add("011110");
        floor_1.Add("001100");
        floor_1.Add("000000");
        table.Floors.Add(floor_1);

        return table;
    }

    public static Table LoadTable_TripleFloorTest()
    {
        var table = new Table(
            numberOfFloors: 3,
            numberOfLines: 6,
            numberOfColumns: 8 // tile width = 1/8 of screen
        );
        // 0 = empty, 1 = tile
        // Must have an even number of tiles
        IList<string> floor_0 = new List<string>();
        floor_0.Add("00011000");
        floor_0.Add("00111100");
        floor_0.Add("01111110");
        floor_0.Add("11111111");
        floor_0.Add("11111111");
        floor_0.Add("01100110");
        table.Floors.Add(floor_0);

        IList<string> floor_1 = new List<string>();
        floor_1.Add("00000000");
        floor_1.Add("00011000");
        floor_1.Add("00111100");
        floor_1.Add("00111100");
        floor_1.Add("00011000");
        floor_1.Add("00000000");
        table.Floors.Add(floor_1);


        IList<string> floor_2 = new List<string>();
        floor_2.Add("00000000");
        floor_2.Add("00000000");
        floor_2.Add("00011000");
        floor_2.Add("00011000");
        floor_2.Add("00000000");
        floor_2.Add("00000000");
        table.Floors.Add(floor_2);

        return table;
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