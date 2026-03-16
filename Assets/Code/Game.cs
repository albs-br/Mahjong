using System.Collections;
using System.Collections.Generic;
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


    TileLine tileLine;

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

        tileLine = new TileLine();

        CreateTile("Sprite 1", 0f, 0f);
        CreateTile("Sprite 2", minX, maxY);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update method");
    }

    void CreateTile(string name, float x, float y)
    {
        GameObject gameObject = new GameObject(name);

        var tile = gameObject.AddComponent<Tile>();

        tileLine.Tiles.Add(tile);
        tile.TileLine = tileLine;

        SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();

        // Load Sprite from Assets/Resources/name.png / bmp
        // (Do not include "Resources" or file extension in path)
        const string BASE_PATH = "fulltiles/";
        Sprite loadedSprite = Resources.Load<Sprite>(BASE_PATH + "bamboo1");

        if (loadedSprite != null)
        {
            renderer.sprite = loadedSprite;
            
            // Set position
            gameObject.transform.position = new Vector2(x, y);

            // Set scale
            float desiredWidth = cameraWidth / 4.0f; // tile width = 1/4 of screen
            float scaleFactor = desiredWidth / renderer.bounds.size.x;

            // Apply the new scale to the sprite's transform
            // If you want to maintain the sprite's original aspect ratio, apply the same scale to Y
            gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);            



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
