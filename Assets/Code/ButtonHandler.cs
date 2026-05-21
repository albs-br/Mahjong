using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void ButtonNewGame_Click()
    {
        Game gameScript = GetComponent<Game>();
        gameScript.StartNewGame();
        
        //Debug.Log("Button was clicked!");
    }
}
