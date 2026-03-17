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

    int numberOfColumns, numberOfLines;

    TileFloor tileFloor;
    //TileLine tileLine;

    Vector3 tileScaleFactor;

    const string TILE_IMGS_BASE_PATH = "fulltiles/";

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        Debug.Log("Awake: Code is running at game start (before Start)!");
        
        
        // Add your initialization code here


        // The EnhancedTouch API is specifically designed for handling multi-touch 
        // scenarios and is a clean way to manage touches.
        EnhancedTouchSupport.Enable();        



        GetScreenBoundaries();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start method");

        this.numberOfColumns = 4;
        this.numberOfLines = 2;
        SetTileScaleFactor(this.numberOfColumns); // tile width = 1/4 of screen

        this.tileFloor = new TileFloor();

        this.tileFloor.TileLines.Add(new TileLine(0));
        CreateTile(this.tileFloor.TileLines[0], "bamboo1");
        CreateTile(this.tileFloor.TileLines[0], "bamboo2");
        CreateTile(this.tileFloor.TileLines[0], "bamboo3");
        CreateTile(this.tileFloor.TileLines[0], "bamboo4");

        this.tileFloor.TileLines.Add(new TileLine(1));
        CreateTile(this.tileFloor.TileLines[1], "circle1");
        CreateTile(this.tileFloor.TileLines[1], "circle2");
        CreateTile(this.tileFloor.TileLines[1], "circle3");
        CreateTile(this.tileFloor.TileLines[1], "circle4");

        UpdateTilesStatus();
    }

    void SetTileScaleFactor(int numberOfColumns)
    {
        this.numberOfColumns = numberOfColumns;

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

    void UpdateTilesStatus()
    {
        // Update IsBlocked of All tiles
        for(int j=0; j < this.tileFloor.TileLines.Count; j++)
        {
            for(int i=0; i < this.tileFloor.TileLines[j].Tiles.Count; i++)
            {
                if(i == 0 || i == this.tileFloor.TileLines[j].Tiles.Count - 1)
                {
                    this.tileFloor.TileLines[j].Tiles[i].IsBlocked = false;
                }
                else
                {
                    this.tileFloor.TileLines[j].Tiles[i].IsBlocked = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update method");
    }

    void CreateTile(TileLine tileLine, string tileType)
    {
        string name = $"Tile {tileLine.Tiles.Count + 1}";
        GameObject gameObject = new GameObject(name);

        var tile = gameObject.AddComponent<Tile>();

        tile.Index = tileLine.Tiles.Count;

        tile.TileType = tileType;

        // Add tile to tileLine, update tile properties
        tile.TileLine = tileLine;
        // if(tileLine.Tiles.Count > 0)
        // {
        //     var previousTile = tileLine.Tiles.Last();
        //     tile.TileAtLeft = previousTile;
        //     tile.TileAtRight = null;
        //     previousTile.TileAtRight = tile;
        // }
        // else
        // {
        //     tile.TileAtLeft = null;
        //     tile.TileAtRight = null;
        // }
        tileLine.Tiles.Add(tile);


        SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();

        // Load Sprite from Assets/Resources/name.png / bmp
        // (Do not include "Resources" or file extension in path)
        Sprite loadedSprite = Resources.Load<Sprite>(TILE_IMGS_BASE_PATH + tileType);

        if (loadedSprite != null)
        {
            renderer.sprite = loadedSprite;
            
            renderer.sortingOrder = numberOfColumns - tile.Index; // sortingOrder = 5 renders on top of other sprites in the same layer with order < 5
            
            // // Set scale
            // float desiredWidth = cameraWidth / 4.0f; // tile width = 1/4 of screen
            // float scaleFactor = desiredWidth / renderer.bounds.size.x;

            // // scaleFactor = scaleFactor * (Tile.TotalWidth / Tile.Width_2D);

            // Apply the new scale to the sprite's transform
            // If you want to maintain the sprite's original aspect ratio, apply the same scale to Y
            gameObject.transform.localScale = tileScaleFactor;




            // float x = this.minX + (renderer.bounds.size.x/2) + ((tile.TileLine.Tiles.Count - 1) * renderer.bounds.size.x);
            float x = this.minX + (Tile.TotalWidth/2) + (tile.Index * Tile.Width_2D);

            float y = 0f + (Tile.TotalHeight * (this.numberOfLines/2)) - (tileLine.Index * Tile.TotalHeight);
            //Debug.Log($"tileLine.Index: {tileLine.Index}");

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

    void GetScreenBoundaries()
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
        
        Debug.Log("Bounds: Left=" + minX + ", Right=" + maxX + ", Bottom=" + minY + ", Top=" + maxY);
        Debug.Log($"Camera width: {cameraWidth}, Camera height: {cameraHeight}");
    }        
}
