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

    //TODO:
    public void ValidateTable()
    {
    }

    /// Sort tiles for this table
    public IList<Pair> SortTable()
    {
        // step 1: solve puzzle, storing pairs of free unmarked tiles (number of floor, line, and tile position)

        // step 2: loop through pairs, creating the Game object classes with TileFloors, TileLines and Tiles

        this.tileTypes_Regular_Temp = this.tileTypes_Regular.ToList();

        this.tempFloors.Clear();
        for (int floorIndex=0; floorIndex < this.Floors.Count; floorIndex++)
        {
            var newTempFloor = this.Floors[floorIndex].ToList();
            this.tempFloors.Add(newTempFloor);
        }

        this.freeTiles = new List<TilePosition>();
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
        for(int floorIndex=0; floorIndex < this.tempFloors.Count; floorIndex++) // loop floors
        {
            for(int lineIndex=0; lineIndex < this.tempFloors[floorIndex].Count; lineIndex++) // loop lines
            {
                var line = this.tempFloors[floorIndex][lineIndex];
                if(line.Contains("1") || line.Contains("2"))
                {
                    isEmpty = false;
                    break;
                }
            }
        }
        return isEmpty;
    }

    private IList<TilePosition> freeTiles; // debug


    // get free tiles for one iteration
    private void GetFreeTiles()
    {
        
        System.Random random = new System.Random();


        //Debug.Log("-----------");

        IList<IList<string>> outputFloors = new List<IList<string>>();

        for(int floorIndex=0; floorIndex < this.tempFloors.Count; floorIndex++) // Loop floors
        {
            outputFloors.Add(new List<string>());

            bool isFirstFloor = (floorIndex == 0);
            bool isLastFloor = (floorIndex == this.tempFloors.Count - 1);

            for(int lineIndex=0; lineIndex < this.tempFloors[floorIndex].Count; lineIndex++) // Loop lines
            {
                var newLine = "";

                bool isFirstLineOfFloor = (lineIndex == 0);
                bool isLastLineOfFloor = (lineIndex == this.tempFloors[floorIndex].Count - 1);

                //Debug.Log("Line before: " + this.tempFloors[floorIndex][i]);

                for(int tileIndex=0; tileIndex < this.tempFloors[floorIndex][lineIndex].Length; tileIndex++) // Loop tiles
                {
                    var currentChar = this.tempFloors[floorIndex][lineIndex][tileIndex];
                    var newChar = currentChar;

                    bool isFirstTileOfLine = (tileIndex == 0);
                    bool isLastTileOfLine = (tileIndex == this.tempFloors[floorIndex][lineIndex].Length - 1);

                    if(currentChar == '1' || currentChar == '2')
                    {
                        bool isHalfLineBelow = (currentChar == '2');

                        
                        bool hasActiveTileAbove = false;
                        //Debug.Log($"floorIndex: {floorIndex}, j: {j}, i: {i}, isLastFloor: {isLastFloor}");
                        if(!isLastFloor)
                        {
                            // check if there is an active tile above (both with and without halfTileBelow flag)
                            if(this.tempFloors[floorIndex + 1][lineIndex][tileIndex] == '1' || this.tempFloors[floorIndex + 1][lineIndex][tileIndex] == '2')
                            {
                                hasActiveTileAbove = true;
                            }
                            else if(!isHalfLineBelow)
                            {
                                // check if there is an active tile above (previous line with halfTileBelow flag)
                                if(!isFirstLineOfFloor && this.tempFloors[floorIndex + 1][lineIndex - 1][tileIndex] == '2')
                                {
                                    hasActiveTileAbove = true;
                                }
                            }
                            else
                            {
                                // check if there is an active tile above (next line without halfTileBelow flag)
                                if(!isLastLineOfFloor && this.tempFloors[floorIndex + 1][lineIndex + 1][tileIndex] == '1')
                                {
                                    hasActiveTileAbove = true;
                                }
                            }
                        }

                        bool hasActiveTileAtLeft = false;
                        if(!isFirstTileOfLine)
                        {
                            // check if there is tile in the same line at left
                            // it doesn't matter the HalfLineBellow of neither of them
                            if(this.tempFloors[floorIndex][lineIndex][tileIndex-1] == '1' ||
                               this.tempFloors[floorIndex][lineIndex][tileIndex-1] == '2')
                            {
                                hasActiveTileAtLeft = true;
                            }

                            // check if there is tile in the line above at left
                            // only if the above is HalfLineBellow and the current is not
                            else if(!isFirstLineOfFloor && !isHalfLineBelow && this.tempFloors[floorIndex][lineIndex-1][tileIndex-1] == '2')
                            {
                                hasActiveTileAtLeft = true;
                            }
                        }



                        bool hasActiveTileAtRight = false;
                        if(!isLastTileOfLine) 
                        {
                            // similar logic of left tile (check previous comment)
                            if(this.tempFloors[floorIndex][lineIndex][tileIndex+1] == '1' ||
                               this.tempFloors[floorIndex][lineIndex][tileIndex+1] == '2')
                            {
                                hasActiveTileAtRight = true;
                            }
                            else if(!isFirstLineOfFloor && !isHalfLineBelow && this.tempFloors[floorIndex][lineIndex-1][tileIndex-1] == '2')
                            {
                                hasActiveTileAtLeft = true;
                            }
                        }






                        if((!hasActiveTileAtLeft || !hasActiveTileAtRight) && !hasActiveTileAbove)
                        {
                            //Debug.Log($"Free tile found at line {i}, cell {j}");
                            

                            // add to free tiles list
                            freeTiles.Add(
                                new TilePosition
                                {
                                    Floor = floorIndex,
                                    Line = lineIndex,
                                    Tile = tileIndex,
                                    IsHalfLineBelow = isHalfLineBelow
                                }
                            );

                            newChar = '0';

                            //Debug.Log("Line after: " + this.tempFloors[floorIndex][i]);
                        }
                    }


                    newLine += newChar;
                }

                outputFloors[floorIndex].Add(newLine);

                //Debug.Log("Line after: " + newLine);
            }
        }

        // copy outputFloors to tempFloors
        for(int floorIndex=0; floorIndex < this.tempFloors.Count; floorIndex++)
        {
            for(int lineIndex=0; lineIndex < this.tempFloors[floorIndex].Count; lineIndex++)
            {
                this.tempFloors[floorIndex][lineIndex] = outputFloors[floorIndex][lineIndex];
            }
        }

        // freeTiles.Count must be even (?)
        Debug.Log($"freeTiles.Count: {freeTiles.Count}");
        if(freeTiles.Count % 2 != 0)
        {
            Debug.Log("Free tiles not even.");
            //throw new Exception($"Free tiles not even.");
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
            // Debug.Log($"this.tileTypes_Regular_Temp: {this.tileTypes_Regular_Temp.Count}");

            if(this.tileTypes_Regular_Temp.Count == 0)
            {
                // Debug.Log("this.tileTypes_Regular_Temp = 0");

                // Reload list of tiles
                this.tileTypes_Regular_Temp = this.tileTypes_Regular.ToList();
            }

            // Debug.Log($"{newPair}");
            
            // --- remove these 2 free tiles from list
            
            // first, set them to null
            freeTiles[freeTile_1] = null;
            freeTiles[freeTile_2] = null;

            // loop all list removing nulls
            int i=0;
            do
            {
                if(freeTiles[i] == null)
                {
                    freeTiles.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            } while (i <= freeTiles.Count - 1);

            // Debug.Log($"freeTiles.Count: {freeTiles.Count}");

        } while (freeTiles.Count > 1);
         
        Debug.Log($"END OF METHOD freeTiles.Count: {freeTiles.Count}");

    }

    public static Table LoadTable_SingleFloorTest()
    {
        var table = new Table(
            numberOfFloors: 1,
            numberOfLines: 5,
            numberOfColumns: 6 // tile width = 1/6 of screen
        );
        // 0 = empty, 1 = tile, 2 = tile half line below
        // Must have an even number of tiles
        
        IList<string> floor_0 = new List<string>
        {
            "211110",
            "001111",
            "011110",
            "210011",
            "021112"
        };
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
        // 0 = empty, 1 = tile, 2 = tile half line below
        // Must have an even number of tiles
        IList<string> floor_0 = new List<string>
        {
            "011110",
            "111111",
            "011110",
            "111111",
            "110011"
        };
        table.Floors.Add(floor_0);

        IList<string> floor_1 = new List<string>
        {
            "000000",
            "001100",
            "011110",
            "001100",
            "000000"
        };
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
        // 0 = empty, 1 = tile, 2 = tile half line below
        // Must have an even number of tiles
        IList<string> floor_0 = new List<string>
        {
            "00011000",
            "00111100",
            "01111110",
            "11111111",
            "11111111",
            "01100110"
        };
        table.Floors.Add(floor_0);

        IList<string> floor_1 = new List<string>
        {
            "00000000",
            "00011000",
            "00111100",
            "00111100",
            "00011000",
            "00000000"
        };
        table.Floors.Add(floor_1);


        IList<string> floor_2 = new List<string>
        {
            "00000000",
            "00000000",
            "00011000",
            "00011000",
            "00000000",
            "00000000"
        };
        table.Floors.Add(floor_2);

        return table;
    }

    public static Table LoadTable_Turtle()
    {
        var table = new Table(
            numberOfFloors: 5,
            numberOfLines: 9,
            numberOfColumns: 8 // tile width = 1/8 of screen
        );
        // 0 = empty, 1 = tile, 2 = tile half line below
        // Must have an even number of tiles
        IList<string> floor_0 = new List<string>
        {
            "11111111",
            "01111110",
            "11111111",
            "11111111",
            "11111111",
            "11111111",
            "11111111",
            "01111110",
            "11111111"
        };
        table.Floors.Add(floor_0);

        IList<string> floor_1 = new List<string>
        {
            "10011001",
            "00111100",
            "01111110",
            "01111110",
            "01111110",
            "01111110",
            "01111110",
            "00111100",
            "10011001"
        };
        table.Floors.Add(floor_1);

        IList<string> floor_2 = new List<string>
        {
            "10000001",
            "00000000",
            "00011000",
            "00111100",
            "00111100",
            "00111100",
            "00011000",
            "00000000",
            "10000001"
        };
        table.Floors.Add(floor_2);

        return table;
    }
}

public class TilePosition
{
    public int Floor { get; set; }
    public int Line { get; set; }
    public int Tile { get; set; }
    public bool IsHalfLineBelow { get; set; }

    public override string ToString()
    {
        return $"TilePosition, Floor: {this.Floor}, Line: {this.Line}, Tile: {this.Tile}, IsHalfLineBelow: {this.IsHalfLineBelow}";
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