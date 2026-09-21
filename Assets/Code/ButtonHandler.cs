using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void ButtonNewGame_Click()
    {
        //Debug.Log("Button was clicked!");

        ConfirmationDialog.Instance.Show(
            "Are you sure you want to start a new game?", 
            () => { 
                // What happens if they click YES
                //Debug.Log("Starting new game...");
                Game gameScript = GetComponent<Game>();
                gameScript.StartNewGame();
                //Application.Quit(); 
            },
            () => { 
                // What happens if they click NO
                //Debug.Log("Cancelled new game."); 
            }
        );
    }
}
