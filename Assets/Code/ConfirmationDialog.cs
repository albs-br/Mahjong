using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationDialog : MonoBehaviour
{
    // Singleton instance for easy access from other scripts
    public static ConfirmationDialog Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action onYesCallback;
    private Action onNoCallback;

    private void Awake()
    {
        // Debug.Log("Awake called in ConfirmationDialog");


        // Setup Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Assign button click listeners
        yesButton.onClick.AddListener(OnYesPressed);
        noButton.onClick.AddListener(OnNoPressed);

        gameObject.SetActive(false); // Hide the dialog
    }

    // Call this method from any script to open the dialog window
    public void Show(string message, Action yesAction, Action noAction = null)
    {
        messageText.text = message;
        onYesCallback = yesAction;
        onNoCallback = noAction;

        gameObject.SetActive(true); // Reveal the dialog
    }

    private void OnYesPressed()
    {
        gameObject.SetActive(false); // Hide the dialog
        onYesCallback?.Invoke();     // Execute the "Yes" action
    }

    private void OnNoPressed()
    {
        gameObject.SetActive(false); // Hide the dialog
        onNoCallback?.Invoke();      // Execute "No" action (if one was given)
    }
}
