using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TMPro;
using System;

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

        //UIManager.Instance.OpenCanvas("Canvas");

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
        //Debug.Log("Start method");

        //this.Table = LoadTable.LoadTable_Test_03();
        //this.Table = LoadTable.LoadTable_SingleFloorTest();
        //this.Table = LoadTable.LoadTable_DoubleFloorTest();
        //this.Table = LoadTable.LoadTable_TripleFloorTest();
        this.Table = LoadTable.LoadTable_Turtle();


        try
        {
            this.Table.ValidateTable();
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            return;
        }

        var pairs = this.Table.SortTable();



        SetTileScaleFactor(this.Table.NumberOfColumns);




        
        this.tileFloors = new List<TileFloor>();
        for(int floorIndex=0; floorIndex < this.Table.Floors.Count; floorIndex++)
        {
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
                    if(chr != '0')
                    {
                        // get tile type from Pairs list previously sorted
                        string tileType = pairs.FirstOrDefault(x => 
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
                        ).TileType;

                        // if(tileType == null)
                        // {
                        //     tileType = "bamboo2"; // debug
                        // }

                        
                        // debug
                        // var tileType = "bamboo2";
                        // if(k == 1) tileType = "circle5";

                        //Add tile
                        CreateTile(
                            tileLine, 
                            tileIndex, 
                            tileType, 
                            (chr == '2' || chr == '4'), 
                            (chr == '3' || chr == '4')
                        );
                    }

                }
            }
        }

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

        // Update properties of All tiles
        for(int floorIndex=0; floorIndex < this.tileFloors.Count; floorIndex++) // Loop floors
        {
            var tileFloor = this.tileFloors[floorIndex];

            bool isFirstFloor = (floorIndex == 0);
            bool isLastFloor = (floorIndex == this.tileFloors.Count - 1);

            for(int lineIndex=0; lineIndex < tileFloor.TileLines.Count; lineIndex++) // Loop lines
            {

                bool isFirstLineOfFloor = (lineIndex == 0);
                bool isLastLineOfFloor = (lineIndex == tileFloor.TileLines.Count - 1);

                for(int tileIndex=0; tileIndex < tileFloor.TileLines[lineIndex].Tiles.Count; tileIndex++) // Loop tiles
                {
                    var currentTile = tileFloor.TileLines[lineIndex].Tiles[tileIndex];

                    bool isFirstTileOfLine = (tileIndex == 0);
                    bool isLastTileOfLine = (tileIndex == tileFloor.TileLines[lineIndex].Tiles.Count - 1);

                    if(currentTile != null && currentTile.IsActive)
                    {
                        this.tilesRemaining++;

                        // check if there is an active tile above
                        bool hasActiveTileAbove = false;
                        if(!isLastFloor)
                        {
                            if(this.tileFloors[floorIndex + 1].TileLines[lineIndex].Tiles[tileIndex] != null)
                            {
                                hasActiveTileAbove = this.tileFloors[floorIndex + 1].TileLines[lineIndex].Tiles[tileIndex].IsActive;
                            }
                            
                            if(!currentTile.IsHalfLineBelow)
                            {
                                // check if there is an active tile above (previous line with halfTileBelow flag)
                                if(!isFirstLineOfFloor)
                                {
                                    var tileToBeChecked = this.tileFloors[floorIndex + 1].TileLines[lineIndex - 1].Tiles[tileIndex];

                                    if(tileToBeChecked != null && tileToBeChecked.IsActive && tileToBeChecked.IsHalfLineBelow)
                                    {
                                        hasActiveTileAbove = true;
                                    }
                                }

                                // check if there is an active tile above (same line, column at left with halfTileColumnRight and halfTileBelow flags)
                                if(!isFirstTileOfLine)
                                {
                                    var tileToBeChecked = this.tileFloors[floorIndex + 1].TileLines[lineIndex].Tiles[tileIndex - 1];
                                    
                                    if(tileToBeChecked != null && tileToBeChecked.IsActive && tileToBeChecked.IsHalfColumnRight && tileToBeChecked.IsHalfLineBelow)
                                    {
                                        hasActiveTileAbove = true;
                                    }
                                }

                                // check if there is an active tile above (previous line, column at left with halfTileColumnRight and halfTileBelow flags)
                                if(!isFirstTileOfLine && !isFirstLineOfFloor)
                                {
                                    var tileToBeChecked = this.tileFloors[floorIndex + 1].TileLines[lineIndex - 1].Tiles[tileIndex - 1];
                                    
                                    if(tileToBeChecked != null && tileToBeChecked.IsActive && tileToBeChecked.IsHalfColumnRight && tileToBeChecked.IsHalfLineBelow)
                                    {
                                        hasActiveTileAbove = true;
                                    }
                                }
                            }
                            else
                            {
                                // check if there is an active tile above (next line without halfTileBelow flag)
                                if(!isLastLineOfFloor)
                                {
                                    var tileToBeChecked = this.tileFloors[floorIndex + 1].TileLines[lineIndex + 1].Tiles[tileIndex];
                                    
                                    if(tileToBeChecked != null && tileToBeChecked.IsActive && !tileToBeChecked.IsHalfLineBelow)
                                    {
                                        hasActiveTileAbove = true;
                                    }
                                }
                                
                                // check if there is an active tile above (same line, column at left with halfTileColumnRight flag)
                                if(!isFirstTileOfLine)
                                {
                                    var tileToBeChecked = this.tileFloors[floorIndex + 1].TileLines[lineIndex].Tiles[tileIndex - 1];
                                    
                                    if(tileToBeChecked != null && tileToBeChecked.IsActive && tileToBeChecked.IsHalfColumnRight)
                                    {
                                        hasActiveTileAbove = true;
                                    }
                                }

                                // check if there is an active tile above (next line, same column with halfTileColumnRight flag)
                                if(!isLastLineOfFloor && !isFirstTileOfLine)
                                {
                                    var tileToBeChecked = this.tileFloors[floorIndex + 1].TileLines[lineIndex + 1].Tiles[tileIndex - 1];
                                    
                                    if(tileToBeChecked != null && tileToBeChecked.IsActive && tileToBeChecked.IsHalfColumnRight)
                                    {
                                        hasActiveTileAbove = true;
                                    }
                                }

                            }
                        }

                        if(hasActiveTileAbove)
                        {
                            currentTile.IsBlocked = true;
                        }
                        else
                        {
                            // check if there is an active tile at left or right
                            bool hasActiveTileAtLeft = false;
                            if(!isFirstTileOfLine)
                            {
                                if(tileFloor.TileLines[lineIndex].Tiles[tileIndex - 1] != null)
                                {
                                    hasActiveTileAtLeft = tileFloor.TileLines[lineIndex].Tiles[tileIndex - 1].IsActive;
                                }

                                if(!isFirstLineOfFloor && !currentTile.IsHalfLineBelow)
                                {
                                    var tileToBeChecked = tileFloor.TileLines[lineIndex - 1].Tiles[tileIndex - 1];

                                    if(tileToBeChecked != null && tileToBeChecked.IsHalfLineBelow)
                                    {
                                        hasActiveTileAtLeft = tileToBeChecked.IsActive;
                                    }
                                }
                            }
                            
                            bool hasActiveTileAtRight = false;
                            if(!isLastTileOfLine)
                            {
                                if(tileFloor.TileLines[lineIndex].Tiles[tileIndex + 1] != null)
                                {
                                    hasActiveTileAtRight = tileFloor.TileLines[lineIndex].Tiles[tileIndex + 1].IsActive;
                                }
                                
                                if(!isFirstLineOfFloor && !currentTile.IsHalfLineBelow)
                                {
                                    var tileToBeChecked = tileFloor.TileLines[lineIndex - 1].Tiles[tileIndex + 1];

                                    if(tileToBeChecked != null && tileToBeChecked.IsHalfLineBelow)
                                    {
                                        hasActiveTileAtRight = tileToBeChecked.IsActive;
                                    }
                                }
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

    private void CreateTile(TileLine tileLine, int index, string tileType, bool isHalfLineBelow, bool isHalfColumnRight)
    {
        string name = $"Tile {index} of Line {tileLine.Index}, of Floor {tileLine.TileFloor.Index}";
        GameObject gameObject = new GameObject(name);

        var tile = gameObject.AddComponent<Tile>();

        tile.Index = index;

        tile.TileType = tileType;

        tile.IsHalfLineBelow = isHalfLineBelow;
        tile.IsHalfColumnRight = isHalfColumnRight;

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

            if(tile.IsHalfColumnRight)
            {
                x += Tile.Width_2D / 2.0f;
            }

            float y = 0f + (Tile.Height_2D * ((float)this.Table.NumberOfLines/2.0f)) - (tileLine.Index * Tile.Height_2D);
            //Debug.Log($"Tile.TotalHeight: {Tile.TotalHeight}");
            //Debug.Log($"Tile.Height_2D: {Tile.Height_2D}");
            //Debug.Log($"y: {y}");
            
            // adjust Y based on floor index:
            y += (Tile.TotalHeight - Tile.Height_2D) * tileLine.TileFloor.Index;

            if(tile.IsHalfLineBelow)
            {
                y -= Tile.Height_2D / 2.0f;
            }


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
