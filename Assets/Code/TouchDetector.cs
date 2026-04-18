using UnityEngine;
using UnityEngine.InputSystem;

public class TouchDetector : MonoBehaviour
{
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // Subscribe to the press action
        inputActions.Player.Press.performed += OnTouchPressed;
    }

    private void OnDisable()
    {
        inputActions.Player.Press.performed -= OnTouchPressed;
        inputActions.Disable();
    }

    private void OnTouchPressed(InputAction.CallbackContext context)
    {
        //inputActions.Player.Position.Enable();

        // Get the current touch position from the Position action
        Vector2 touchPosition = inputActions.Player.Position.ReadValue<Vector2>();

        //Vector2 touchPosition = playerInput.actions["Point"].ReadValue<Vector2>();


        
        Debug.Log($"touchPosition: {touchPosition}");

        // Convert screen position to a Ray
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        // Perform the Raycast (for 3D objects)
        if (Physics.Raycast(ray, out hit))
        {

            Debug.Log($"hit.collider: {hit.collider}");

            if (hit.collider != null)
            {
                Debug.Log("Touched GameObject: " + hit.collider.gameObject.name);
                // Perform your logic here
            }
        }
    }
}


// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.InputSystem.LowLevel;

// public class TouchDetector : MonoBehaviour
// {
//     public void OnTouch(InputAction.CallbackContext context)
//     {
//         if (context.performed)
//         {
//             // Access touch position
//             Vector2 touchPos = context.ReadValue<TouchState>().position;
//             Debug.Log($"Touch Detected at: {touchPos}");
//         }
//     }

//     public void OnTouchAction(InputAction.CallbackContext context)
//     {
//         // Read screen position from the touch
//         Vector2 touchPos = context.ReadValue<Vector2>(); 
//         Debug.Log($"Screen Position: {touchPos}");
//     }

// }


// using UnityEngine;

// public class TouchDetector : MonoBehaviour
// {
//     void OnMouseDown()
//     {
//         // This code runs when the sprite is touched or clicked
        
//         // Debug.Log("Sprite Touched: " + gameObject.name);
//         // Debug.Log("sorting order: " + this.gameObject.GetComponent<Renderer>().sortingOrder);
//         // Debug.Log("IsBlocked: " + this.gameObject.GetComponent<Tile>().IsBlocked);

//         var tile = gameObject.GetComponent<Tile>();

//         if(tile.IsBlocked)
//         {
//             Debug.Log("IsBlocked");
//             return;
//         }


//         SpriteRenderer renderer = GetComponent<SpriteRenderer>();

//         // Debug.Log($"tile.Index: {tile.Index}");
//         var game = tile.Game; //tile.TileLine.TileFloor.Game;

//         if(!tile.IsSelected)
//         {
//             if(game.TileSelected == null)
//             {
//                 // Set sprite gray (selected)
//                 renderer.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
//                 tile.IsSelected = true;
//                 game.TileSelected = tile;
//             }
//             else
//             {
//                 if(game.TileSelected.TileType == tile.TileType)
//                 {
//                     // Remove both tiles
                    
//                     // Debug.Log("Remove both tiles");
                    
//                     // // Call this method to hide/deactivate the GameObject
//                     // this.gameObject.SetActive(false);

//                     //Debug.Log("Removing tile " + this.gameObject.GetComponent<Tile>().Index);
//                     this.gameObject.GetComponent<Tile>().Remove();

//                     game.TileSelected.Remove();
//                     game.TileSelected = null;
//                 }
//             }
//         }
//         else
//         {
//             // Remove gray (unselected)
//             renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
//             tile.IsSelected = false;
//             game.TileSelected = null;
//         }
//     }
// }
