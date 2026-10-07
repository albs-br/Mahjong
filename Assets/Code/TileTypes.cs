using System.Linq;

public static class TileTypes
{
    public static string[] TileTypes_Regular => new[] { 
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
        "pinyin2",
        "pinyin3",
        "pinyin4",
        "pinyin5",
        "pinyin6",
        "pinyin7",
        "pinyin8",
        "pinyin9",
        "pinyin10",
        "pinyin11",
        "pinyin12",
        "pinyin13",
        "pinyin14",
        "pinyin15",
    };

    public static string[] TileTypes_Flowers => new[] { 
        "lotus",
        "orchid",
        "peony",
        "chrysanthemum",
    };

    public static string[] TileTypes_Seasons => new[] {
        "spring",
        "summer",
        "winter",
        "fall",
    };

    public static bool TileTypesAreSame(Tile tile_1, Tile tile_2)
    {
        return  (
                    tile_1.TileType == tile_2.TileType || // same exact type (works for regular tiles)
                    (TileTypes.TileTypes_Flowers.Contains(tile_1.TileType) && TileTypes.TileTypes_Flowers.Contains(tile_2.TileType)) || // or both are flowers
                    (TileTypes.TileTypes_Seasons.Contains(tile_1.TileType) && TileTypes.TileTypes_Seasons.Contains(tile_2.TileType))    // or both are seasons
                );
    }
}

public enum TileTypeClass_Enum
{
    Regular,
    Flower,
    Season
}
