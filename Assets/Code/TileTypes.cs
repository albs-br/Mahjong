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
}

public enum TileTypeClass_Enum
{
    Regular,
    Flower,
    Season
}
