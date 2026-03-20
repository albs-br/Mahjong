using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Game : MonoBehaviour
{
    float cameraHeight;
    float cameraWidth;

    float minX;
    float maxX;
    float minY;
    float maxY;

    //int numberOfColumns, numberOfLines;
    Table table;

    TileFloor tileFloor;

    public Tile TileSelected;

    Vector3 tileScaleFactor;

    const string TILE_IMGS_BASE_PATH = "fulltiles/";

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

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        //Debug.Log("Awake: Code is running at game start (before Start)!");
        
        
        // Add your initialization code here


        // The EnhancedTouch API is specifically designed for handling multi-touch 
        // scenarios and is a clean way to manage touches.
        EnhancedTouchSupport.Enable();        



        GetScreenBoundaries();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start method");

        // this.numberOfColumns = 6;
        // this.numberOfLines = 5;

        this.table = new Table(
            numberOfColumns: 6, // tile width = 1/6 of screen
            numberOfLines: 5
        );
        // 0 = empty, 1 = tile
        // Must have an even number of tiles
        this.table.Lines.Add("111110");
        this.table.Lines.Add("001111");
        this.table.Lines.Add("011110");
        this.table.Lines.Add("111111");
        this.table.Lines.Add("011111");


        this.table.SortTable();



        SetTileScaleFactor(this.table.NumberOfColumns);




        // // TODO: loop through Table randomly adding tiles
        // this.tileFloor = new TileFloor();
        // this.tileFloor.Game = this;
        // for(int i=0; i<this.table.Lines.Count; i++)
        // {
        //     var line = this.table.Lines[i];

        //     var tileLine = this.tileFloor.AddTileLine();
            
        //     // Counts characters from the start of the string as long as they are '\0'
        //     int n = line.TakeWhile(c => c == '0').Count();            
        //     tileLine.TileOffsetLeft = n;

        //     for(int j=0; j<line.Length; j++)
        //     {
        //         var chr = line[j];
        //         if(chr == '1')
        //         {
        //             //Add tile
        //             CreateTile(tileLine, "bamboo1");
        //         }

        //     }
        // }




        this.tileFloor = new TileFloor();
        this.tileFloor.Game = this;

        var tileLine_0 = this.tileFloor.AddTileLine();
        CreateTile(tileLine_0, "bamboo1");
        CreateTile(tileLine_0, "bamboo2");
        CreateTile(tileLine_0, "bamboo3");
        CreateTile(tileLine_0, "bamboo4");
        CreateTile(tileLine_0, "bamboo5");
        CreateTile(tileLine_0, "bamboo6");

        var tileLine_1 = this.tileFloor.AddTileLine();
        tileLine_1.TileOffsetLeft = 2;
        CreateTile(tileLine_1, "circle1");
        CreateTile(tileLine_1, "pinyin1");
        CreateTile(tileLine_1, "circle3");
        CreateTile(tileLine_1, "circle4");

        var tileLine_2 = this.tileFloor.AddTileLine();
        tileLine_2.TileOffsetLeft = 1;
        CreateTile(tileLine_2, "pinyin1");
        CreateTile(tileLine_2, "pinyin2");
        CreateTile(tileLine_2, "pinyin3");
        CreateTile(tileLine_2, "pinyin4");

        var tileLine_3 = this.tileFloor.AddTileLine();
        CreateTile(tileLine_3, "bamboo6");
        CreateTile(tileLine_3, "bamboo5");
        CreateTile(tileLine_3, "bamboo4");
        CreateTile(tileLine_3, "pinyin6");
        CreateTile(tileLine_3, "pinyin7");
        CreateTile(tileLine_3, "bamboo1");

        var tileLine_4 = this.tileFloor.AddTileLine();
        tileLine_4.TileOffsetLeft = 1;
        CreateTile(tileLine_4, "pinyin5");
        CreateTile(tileLine_4, "pinyin6");
        CreateTile(tileLine_4, "pinyin7");
        CreateTile(tileLine_4, "pinyin8");
        CreateTile(tileLine_4, "pinyin9");
        //CreateTile(tileLine_4, "pinyin10");

        UpdateTilesStatus();
    }

    private void SetTileScaleFactor(int numberOfColumns)
    {
        //this.numberOfColumns = numberOfColumns;

        string sampleTile = "bamboo1";
        Sprite loadedSprite = Resources.Load<Sprite>(TILE_IMGS_BASE_PATH + sampleTile);

        if (loadedSprite != null)
        {
            SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = loadedSprite;
            
            
            float totalWidth_px = (Tile.TotalWidth_px - Tile.Width_2D_px) + (numberOfColumns * Tile.Width_2D_px); // in pixels
            //Debug.Log($"totalWidth_px: {totalWidth_px}");

            float temp = 1 + (((Tile.TotalWidth_px - Tile.Width_2D_px) / totalWidth_px) / numberOfColumns); // factor to take into account one 3d border
            //Debug.Log($"temp: {temp}");


            // Set scale
            float desiredWidth = this.cameraWidth / (numberOfColumns / temp);
            //Debug.Log($"desiredWidth: {desiredWidth}");
            float scaleFactor = desiredWidth / renderer.bounds.size.x;

            float f = this.cameraWidth / totalWidth_px; // in screen units/pixel
            Tile.TotalWidth = Tile.TotalWidth_px * f;
            Tile.Width_2D = Tile.Width_2D_px * f;
            // Debug.Log($"Tile.TotalWidth: {Tile.TotalWidth}");
            // Debug.Log($"Tile.Width_2D: {Tile.Width_2D}");
            Tile.TotalHeight = Tile.TotalHeight_px * f;
            Tile.Height_2D = Tile.Height_2D_px * f;

            // scaleFactor = scaleFactor * (Tile.TotalWidth / Tile.Width_2D);

            // Apply the new scale to the sprite's transform
            // If you want to maintain the sprite's original aspect ratio, apply the same scale to Y
            this.tileScaleFactor = new Vector3(scaleFactor, scaleFactor, 1f);            

            Destroy(renderer);
        }
    }

    public void UpdateTilesStatus()
    {
        // Update IsBlocked of All tiles
        for(int j=0; j < this.tileFloor.TileLines.Count; j++)
        {
            // var listTilesActive = 
            //     this.tileFloor.TileLines[j].Tiles
            //         .Where(x => x.IsActive);

            for(int i=0; i < this.tileFloor.TileLines[j].Tiles.Count; i++)
            {
                var currentTile = this.tileFloor.TileLines[j].Tiles[i];
                if(currentTile.IsActive)
                {
                    bool hasActiveTileAtLeft = false;
                    if(i > 0) 
                    {
                        hasActiveTileAtLeft = this.tileFloor.TileLines[j].Tiles[i - 1].IsActive;
                    }
                    
                    bool hasActiveTileAtRight = false;
                    if(i < this.tileFloor.TileLines[j].Tiles.Count - 1)
                    {
                        hasActiveTileAtRight = this.tileFloor.TileLines[j].Tiles[i + 1].IsActive;
                    }

                    if(!hasActiveTileAtLeft || !hasActiveTileAtRight)
                    {
                        currentTile.IsBlocked = false;
                    }
                    else
                    {
                        currentTile.IsBlocked = true;
                    }
                }

                // if(i == 0 || i == this.tileFloor.TileLines[j].Tiles.Count - 1)
                // {
                //     this.tileFloor.TileLines[j].Tiles[i].IsBlocked = false;
                // }
                // else
                // {
                //     this.tileFloor.TileLines[j].Tiles[i].IsBlocked = true;
                // }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update method");
    }

    private void CreateTile(TileLine tileLine, string tileType)
    {
        string name = $"Tile {tileLine.Tiles.Count + 1}";
        GameObject gameObject = new GameObject(name);

        var tile = gameObject.AddComponent<Tile>();

        tile.Index = tileLine.Tiles.Count;

        tile.TileType = tileType;

        tile.TileLine = tileLine;
        tileLine.Tiles.Add(tile);


        SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();

        // Load Sprite from Assets/Resources/name / bmp
        // (Do not include "Resources" or file extension in path)
        Sprite loadedSprite = Resources.Load<Sprite>(TILE_IMGS_BASE_PATH + tileType);

        if (loadedSprite != null)
        {
            renderer.sprite = loadedSprite;
            
            renderer.sortingOrder = this.table.NumberOfColumns - tile.Index; // sortingOrder = 5 renders on top of other sprites in the same layer with order < 5
            
            // Set scale
            // Apply the previously calculated scale to the sprite's transform
            gameObject.transform.localScale = tileScaleFactor;




            // float x = this.minX + (renderer.bounds.size.x/2) + ((tile.TileLine.Tiles.Count - 1) * renderer.bounds.size.x);
            float x = this.minX + (Tile.TotalWidth/2) + (tile.Index * Tile.Width_2D);

            x += tile.TileLine.TileOffsetLeft * Tile.Width_2D;

            float y = 0f + (Tile.Height_2D * ((float)this.table.NumberOfLines/2.0f)) - (tileLine.Index * Tile.Height_2D);
            //Debug.Log($"Tile.TotalHeight: {Tile.TotalHeight}");
            //Debug.Log($"Tile.Height_2D: {Tile.Height_2D}");
            //Debug.Log($"y: {y}");

            // Set position
            gameObject.transform.position = new Vector2(x, y);




            // Add a BoxCollider2D component to this GameObject
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

            // Set the size of the box collider to match the size of the sprite bounds
            boxCollider.size = renderer.sprite.bounds.size;
            
            // Set the offset of the box collider to match the center of the sprite bounds
            boxCollider.offset = renderer.sprite.bounds.center;





            // Add Touch Handler Script
            gameObject.AddComponent<TouchDetector>();
        }
        else
        {
            Debug.LogError("Sprite not found");
        }        
    }

    private void GetScreenBoundaries()
    {
        Camera cam = Camera.main;
        
        // orthographicSize is half the vertical size (height) of the camera's view.
        cameraHeight = cam.orthographicSize * 2;
        
        // Calculate half the horizontal size (width) based on the aspect ratio.
        cameraWidth = cameraHeight * cam.aspect;

        // Calculate min and max bounds in world space
        minX = cam.transform.position.x - (cameraWidth/2);
        maxX = cam.transform.position.x + (cameraWidth/2);
        minY = cam.transform.position.y - (cameraHeight/2);
        maxY = cam.transform.position.y + (cameraHeight/2);
        
        //Debug.Log("Bounds: Left=" + minX + ", Right=" + maxX + ", Bottom=" + minY + ", Top=" + maxY);
        //Debug.Log($"Camera width: {cameraWidth}, Camera height: {cameraHeight}");
    }        
}
