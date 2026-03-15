// using UnityEngine;
// using UnityEngine.InputSystem.EnhancedTouch;
// using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

// public class SpriteTouchHandler : MonoBehaviour
// {
//     private Camera mainCamera;

//     void Start()
//     {
//         mainCamera = Camera.main;
//         // Subscribe to the finger down event
//         Touch.onFingerDown += HandleFingerDown;
//     }

//     void OnDestroy()
//     {
//         // Unsubscribe to prevent memory leaks
//         Touch.onFingerDown -= HandleFingerDown;
//     }

//     private void HandleFingerDown(Finger finger)
//     {
//         // Convert screen position to world point
//         Vector3 worldPoint = mainCamera.ScreenToWorldPoint(finger.screenPosition);
//         Vector2 touchPosWorld2D = new Vector2(worldPoint.x, worldPoint.y);



//         // // Check if a 2D collider was hit at the touch position
//         // Collider2D hitCollider = Physics2D.OverlapPoint(touchPosWorld2D);

//         // if (hitCollider != null && hitCollider.gameObject == this.gameObject)
//         // {
//         //     // The touch was on this specific sprite
//         //     Debug.Log("Touched " + this.gameObject.name);
//         //     Debug.Log("Touch position " + touchPosWorld2D);
            
            
            
//         //     // Add your touch logic here

//         //     // Call this method to hide/deactivate the GameObject
//         //     //this.gameObject.SetActive(false);             
//         // }
//     }
// }
