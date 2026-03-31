using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

public class Game : MonoBehaviour
{
    // UI fields
    private TextMeshProUGUI textTilesLeft;
    private TextMeshProUGUI textOpenMatches;



    private float cameraHeight;
    private float cameraWidth;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private IList<TileFloor> tileFloors;

    private Vector3 tileScaleFactor;

    private int tilesRemaining;
    private int openMatches;




    public Table Table;
    public Tile TileSelected;




    const string TILE_IMGS_BASE_PATH = "fulltiles/";

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        //Debug.Log("Awake: Code is running at game start (before Start)!");

        // UI field initialization        
        this.textTilesLeft = GameObject.Find("TilesLeftText").GetComponent<TextMeshProUGUI>();
        this.textOpenMatches = GameObject.Find("OpenMatchesText").GetComponent<TextMeshProUGUI>();

        // GameObject objFound = GameObject.Find("TilesLeftText");
        // if (objFound != null)
        // {
        //     Debug.Log("Object found: " + objFound.name);

        //     //this.textObj = (TextMeshProUGUI)objFound;
        //     this.textTilesLeft = objFound.GetComponent<TextMeshProUGUI>();


        //     // You can also get a component from the found object
        //     // For example, getting a Rigidbody component:
        //     // Rigidbody rb = foundObject.GetComponent<Rigidbody>();
        // }
        // else
        // {
        //     Debug.Log("Object not found.");
        // }        




        // The EnhancedTouch API is specifically designed for handling multi-touch 
        // scenarios and is a clean way to manage touches.
        EnhancedTouchSupport.Enable();        



        GetScreenBoundaries();
    }
    
    // Start is called before the first frame update
    void Start()
    {

        // //debug
        // TilesLeftTextScript objTextScript;
        // objTextScript = gameObject.AddComponent<TilesLeftTextScript>();
        // objTextScript.UpdateText("ewrfwerfwerfwe");



        //Debug.Log("Start method");

        //this.Table = Table.LoadTable_SingleFloorTest();
        this.Table = Table.LoadTable_DoubleFloorTest();
        //this.Table = Table.LoadTable_TripleFloorTest();
        //this.Table = Table.LoadTable_Turtle();


        var pairs = this.Table.SortTable();



        SetTileScaleFactor(this.Table.NumberOfColumns);




        
        this.tileFloors = new List<TileFloor>();
        for(int floorIndex=0; floorIndex < this.Table.Floors.Count; floorIndex++)
        {
            Debug.Log($"k: {floorIndex}");
            
            var lines = this.Table.Floors[floorIndex];

            var tileFloor = new TileFloor();
            tileFloor.Index = floorIndex;
            tileFloor.Game = this;
            this.tileFloors.Add(tileFloor);

            for(int lineIndex=0; lineIndex < lines.Count; lineIndex++)
            {
                var line = lines[lineIndex];

                var tileLine = tileFloor.AddTileLine();
                
                for(int tileIndex=0; tileIndex < line.Length; tileIndex++)
                {
                    var chr = line[tileIndex];
                    if(chr == '1')
                    {
                        // get tile type from Pairs list previously sorted
                        string tileType = pairs.Where(x => 
                            (
                                x.Tile_1.Floor == floorIndex &&
                                x.Tile_1.Line == lineIndex &&
                                x.Tile_1.Tile == tileIndex
                            ) ||
                            (
                                x.Tile_2.Floor == floorIndex &&
                                x.Tile_2.Line == lineIndex &&
                                x.Tile_2.Tile == tileIndex
                            )
                        ).FirstOrDefault().TileType;

                        // if(tileType == null)
                        // {
                        //     tileType = "bamboo2"; // debug
                        // }

                        
                        // debug
                        // var tileType = "bamboo2";
                        // if(k == 1) tileType = "circle5";

                        //Add tile
                        CreateTile(tileLine, tileIndex, tileType);
                    }

                }
            }
        }



        // this.tileFloor = new TileFloor();
        // this.tileFloor.Game = this;

        // var tileLine_0 = this.tileFloor.AddTileLine();
        // CreateTile(tileLine_0, "bamboo1");
        // CreateTile(tileLine_0, "bamboo2");
        // CreateTile(tileLine_0, "bamboo3");
        // CreateTile(tileLine_0, "bamboo4");
        // CreateTile(tileLine_0, "bamboo5");
        // CreateTile(tileLine_0, "bamboo6");

        // var tileLine_1 = this.tileFloor.AddTileLine();
        // CreateTile(tileLine_1, "circle1");
        // CreateTile(tileLine_1, "pinyin1");
        // CreateTile(tileLine_1, "circle3");
        // CreateTile(tileLine_1, "circle4");

        // var tileLine_2 = this.tileFloor.AddTileLine();
        // CreateTile(tileLine_2, "pinyin1");
        // CreateTile(tileLine_2, "pinyin2");
        // CreateTile(tileLine_2, "pinyin3");
        // CreateTile(tileLine_2, "pinyin4");

        // var tileLine_3 = this.tileFloor.AddTileLine();
        // CreateTile(tileLine_3, "bamboo6");
        // CreateTile(tileLine_3, "bamboo5");
        // CreateTile(tileLine_3, "bamboo4");
        // CreateTile(tileLine_3, "pinyin6");
        // CreateTile(tileLine_3, "pinyin7");
        // CreateTile(tileLine_3, "bamboo1");

        // var tileLine_4 = this.tileFloor.AddTileLine();
        // CreateTile(tileLine_4, "pinyin5");
        // CreateTile(tileLine_4, "pinyin6");
        // CreateTile(tileLine_4, "pinyin7");
        // CreateTile(tileLine_4, "pinyin8");
        // CreateTile(tileLine_4, "pinyin9");
        // //CreateTile(tileLine_4, "pinyin10");

        this.UpdateGame();
    }

    private void UpdateUI()
    {
        // Set Open Matches label color
        if(this.openMatches <= 3)
        {
            this.textOpenMatches.color = Color.red;
        }
        else
        {
            this.textOpenMatches.color = Color.white;
        }

        if(this.tilesRemaining == 0)
        {
            // You win
        }
        else if(this.openMatches == 0)
        {
            // Game over
        }

        this.textTilesLeft.text = "Tiles Left: " + this.tilesRemaining;
        this.textOpenMatches.text = "Open Matches: " + this.openMatches;
        //this.textObj.SetText("set via SetText");
    }

    private void SetTileScaleFactor(int numberOfColumns)
    {
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

    public void UpdateGame()
    {
        this.tilesRemaining = 0;
        this.openMatches = 0;

        // Update IsBlocked of All tiles
        for(int floorIndex=0; floorIndex < this.tileFloors.Count; floorIndex++)
        {
            var tileFloor = this.tileFloors[floorIndex];

            for(int lineIndex=0; lineIndex < tileFloor.TileLines.Count; lineIndex++)
            {
                // var listTilesActive = 
                //     this.tileFloor.TileLines[j].Tiles
                //         .Where(x => x.IsActive);

                for(int tileIndex=0; tileIndex < tileFloor.TileLines[lineIndex].Tiles.Count; tileIndex++)
                {
                    var currentTile = tileFloor.TileLines[lineIndex].Tiles[tileIndex];
                    if(currentTile != null && currentTile.IsActive)
                    {
                        this.tilesRemaining++;

                        // check if there is an active tile above
                        bool hasActiveTileAbove = false;
                        if(((floorIndex + 1) <= this.tileFloors.Count - 1) && this.tileFloors[floorIndex + 1].TileLines[lineIndex].Tiles[tileIndex] != null)
                        {
                            hasActiveTileAbove = this.tileFloors[floorIndex + 1].TileLines[lineIndex].Tiles[tileIndex].IsActive;
                        }

                        if(hasActiveTileAbove)
                        {
                            currentTile.IsBlocked = true;
                        }
                        else
                        {
                            // check if there is an active tile at left or right
                            bool hasActiveTileAtLeft = false;
                            if(tileIndex > 0 && tileFloor.TileLines[lineIndex].Tiles[tileIndex - 1] != null)
                            {
                                hasActiveTileAtLeft = tileFloor.TileLines[lineIndex].Tiles[tileIndex - 1].IsActive;
                            }
                            
                            bool hasActiveTileAtRight = false;
                            if(tileIndex < tileFloor.TileLines[lineIndex].Tiles.Count - 1 && tileFloor.TileLines[lineIndex].Tiles[tileIndex + 1] != null)
                            {
                                hasActiveTileAtRight = tileFloor.TileLines[lineIndex].Tiles[tileIndex + 1].IsActive;
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

                        if(!currentTile.IsBlocked)
                        {
                            this.openMatches++;
                        }
                    }
                }
            }
        }

        this.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update method");


        // TODO:
        // put this method here (??); will lead to waste CPU usage
        // may use a counter to trigger only at a tenth of second for example
        // this.UpdateGame();

    }

    private void CreateTile(TileLine tileLine, int index, string tileType)
    {
        string name = $"Tile {index} of Line {tileLine.Index}, of Floor {tileLine.TileFloor.Index}";
        GameObject gameObject = new GameObject(name);

        var tile = gameObject.AddComponent<Tile>();

        tile.Index = index;

        tile.TileType = tileType;

        tile.TileLine = tileLine;
        tileLine.Tiles[index] = tile;


        SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();

        // Load Sprite from Assets/Resources/name / bmp
        // (Do not include "Resources" or file extension in path)
        Sprite loadedSprite = Resources.Load<Sprite>(TILE_IMGS_BASE_PATH + tileType);

        if (loadedSprite != null)
        {
            renderer.sprite = loadedSprite;
            
            // sortingOrder = 5 renders on top of other sprites in the same layer with order < 5
            renderer.sortingOrder = 
                (tile.TileLine.TileFloor.Index * (this.Table.NumberOfLines * this.Table.NumberOfColumns))        // sortingOrder of the floor 
                + this.Table.NumberOfColumns - tile.Index;
            
            // Set scale
            // Apply the previously calculated scale to the sprite's transform
            gameObject.transform.localScale = tileScaleFactor;




            // float x = this.minX + (renderer.bounds.size.x/2) + ((tile.TileLine.Tiles.Count - 1) * renderer.bounds.size.x);
            float x = this.minX + (Tile.TotalWidth/2) + (tile.Index * Tile.Width_2D);

            // adjust X based on floor index:
            x += (Tile.TotalWidth - Tile.Width_2D) * tileLine.TileFloor.Index;

            float y = 0f + (Tile.Height_2D * ((float)this.Table.NumberOfLines/2.0f)) - (tileLine.Index * Tile.Height_2D);
            //Debug.Log($"Tile.TotalHeight: {Tile.TotalHeight}");
            //Debug.Log($"Tile.Height_2D: {Tile.Height_2D}");
            //Debug.Log($"y: {y}");
            
            // adjust Y based on floor index:
            y += (Tile.TotalHeight - Tile.Height_2D) * tileLine.TileFloor.Index;


            // lower z is closer to the camera
            float z = this.Table.NumberOfFloors - tile.TileLine.TileFloor.Index; 

            // Set position
            gameObject.transform.position = new Vector3(x, y, z);




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
