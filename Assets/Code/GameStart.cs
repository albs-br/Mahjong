using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameStart : MonoBehaviour
{
    float cameraHeight;
    float cameraWidth;

    float minX;
    float maxX;
    float minY;
    float maxY;


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

        CreateSprite("Sprite 1", 0f, 0f);
        CreateSprite("Sprite 2", minX, maxY);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update method");
    }

    void CreateSprite(string name, float x, float y)
    {
        // 1. Create a new GameObject
        GameObject spriteObject = new GameObject(name);

        // 2. Add SpriteRenderer component
        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();

        // 3. Load Sprite from Assets/Resources/name.png / bmp
        // (Do not include "Resources" or file extension in path)
        Sprite loadedSprite = Resources.Load<Sprite>("fulltiles/bamboo1");

        if (loadedSprite != null)
        {
            renderer.sprite = loadedSprite;
            
            // Set position
            spriteObject.transform.position = new Vector2(x, y);

            // Set scale
            float desiredWidth = cameraWidth / 4.0f; // tile width = 1/4 of screen
            float scaleFactor = desiredWidth / renderer.bounds.size.x;

            // Apply the new scale to the sprite's transform
            // If you want to maintain the sprite's original aspect ratio, apply the same scale to Y
            spriteObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);            



            // Add a BoxCollider2D component to this GameObject
            BoxCollider2D boxCollider = spriteObject.AddComponent<BoxCollider2D>();

            // Set the size of the box collider to match the size of the sprite bounds
            boxCollider.size = renderer.sprite.bounds.size;
            
            // Set the offset of the box collider to match the center of the sprite bounds
            boxCollider.offset = renderer.sprite.bounds.center;


            // You can also adjust its properties, like size or offset
            //boxCollider.size = spriteObject.transform.size; //new Vector2(2f, 2f);
            // boxCollider.offset = new Vector2(0f, 0.5f);



            // Add Sprite Touch Handler Script
            // spriteObject.AddComponent<SpriteTouchHandler>();
            spriteObject.AddComponent<TouchDetector>();
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
