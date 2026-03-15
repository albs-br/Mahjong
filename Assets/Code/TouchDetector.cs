using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    void OnMouseDown()
    {
        // This code runs when the sprite is touched or clicked
        Debug.Log("Sprite Touched: " + gameObject.name);



        // Add your specific action here, e.g., load a level or change a state

        // Call this method to hide/deactivate the GameObject
        this.gameObject.SetActive(false);             

    }
}
