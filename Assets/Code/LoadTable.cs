using System.Collections.Generic;

public static class LoadTable
{
    // 0 = empty, 1 = tile
    // 2 = tile half line below, 3 = tile half column to the right
    // 4 = tile half line below and half column to the right
    // Must have an even number of tiles

    public static Table LoadTable_Test_01()
    {
        var table = new Table(
            numberOfFloors: 1,
            numberOfLines: 3,
            numberOfColumns: 6 // tile width = 1/6 of screen
        );
        
        IList<string> floor_0 = new List<string>
        {
            "222222",
            "222222",
            "000000",
        };
        table.Floors.Add(floor_0);

        return table;
    }

    public static Table LoadTable_SingleFloorTest()
    {
        var table = new Table(
            numberOfFloors: 1,
            numberOfLines: 5,
            numberOfColumns: 6 // tile width = 1/6 of screen
        );
        
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
            "21111112",
            "21111112",
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
            "00111101",
            "00111100",
            "00011000",
            "00000000",
            "10000001"
        };
        table.Floors.Add(floor_2);

        IList<string> floor_3 = new List<string>
        {
            "00000000",
            "00000000",
            "00000000",
            "00022000",
            "00022000",
            "00000000",
            "00000000",
            "00000000",
            "00000000"
        };
        table.Floors.Add(floor_3);

        IList<string> floor_4 = new List<string>
        {
            "00000000",
            "00000000",
            "00000000",
            "00000000",
            "00030000",
            "00000000",
            "00000000",
            "00000000",
            "00000000"
        };
        table.Floors.Add(floor_4);

        return table;
    }
}