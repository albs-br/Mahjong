using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

// Alias EnhancedTouch.Touch to "Touch" for less typing.
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchDetector : MonoBehaviour
{

    void Awake()
    {
        // Note that enhanced touch support needs to be explicitly enabled.
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        // Illustrates how to examine all active touches once per frame and show their last recorded position
        // in the associated screen-space.
        foreach (var touch in Touch.activeTouches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Debug.Log($"Frame {Time.frameCount}: Touch {touch} started this frame at ({touch.screenPosition.x}, {touch.screenPosition.y})");

                    // convert screen coordinate system to world coordinate system
                    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.screenPosition.x, touch.screenPosition.y, Camera.main.nearClipPlane));
                    // Debug.Log($"worldPoint: {worldPoint}");

                    Vector2 touchPosWorld2D = new Vector2(worldPoint.x, worldPoint.y);


                    // Raycast at the touch position
                    RaycastHit2D hit = Physics2D.Raycast(touchPosWorld2D, Vector2.zero);


                    if (hit.collider != null) {
                        //Debug.Log("Touched GameObject: " + hit.collider.gameObject.name);

                        var tile = hit.collider.gameObject.GetComponent<Tile>();

                        if(tile.IsBlocked)
                        {
                            Debug.Log("IsBlocked");
                            return;
                        }


                        SpriteRenderer renderer = hit.collider.gameObject.GetComponent<SpriteRenderer>();

                        // Debug.Log($"tile.Index: {tile.Index}");
                        var game = tile.Game; //tile.TileLine.TileFloor.Game;

                        if(!tile.IsSelected)
                        {
                            if(game.TileSelected == null)
                            {
                                // Set sprite gray (selected)
                                renderer.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                                tile.IsSelected = true;
                                game.TileSelected = tile;
                            }
                            else
                            {
                                // if both tiles are the same type, remove them
                                // or if both tiles are flowers (or both are seasons), remove them
                                if(
                                    (game.TileSelected.TileType == tile.TileType)  // || 
                                    //( ... ) //TODO
                                )
                                {
                                    // Remove both tiles
                                    
                                    // Debug.Log("Remove both tiles");
                                    
                                    // // Call this method to hide/deactivate the GameObject
                                    // this.gameObject.SetActive(false);

                                    //Debug.Log("Removing tile " + this.gameObject.GetComponent<Tile>().Index);
                                    // this.gameObject.GetComponent<Tile>().Remove();
                                    tile.Remove();

                                    game.TileSelected.Remove();
                                    game.TileSelected = null;
                                }
                            }
                        }
                        else
                        {
                            // Remove gray (unselected)
                            renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                            tile.IsSelected = false;
                            game.TileSelected = null;
                        }

                    }


                    break;
                case TouchPhase.Ended:
                    // Debug.Log($"Frame {Time.frameCount}:Touch {touch} ended this frame at ({touch.screenPosition.x}, {touch.screenPosition.y})");
                    break;
                case TouchPhase.Moved:
                    // Debug.Log($"Frame {Time.frameCount}: Touch {touch} moved this frame to ({touch.screenPosition.x}, {touch.screenPosition.y})");
                    break;
                case TouchPhase.Canceled:
                    // Debug.Log($"Frame {Time.frameCount}: Touch {touch} was canceled this frame");
                    break;
                case TouchPhase.Stationary:
                    // Debug.Log($"Frame {Time.frameCount}: ouch {touch} was not updated this frame");
                    break;
            }
        }
    }
}
