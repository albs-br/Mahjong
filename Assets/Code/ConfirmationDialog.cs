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
    [SerializeField] private Button okButton;

    private Action onYesCallback;
    private Action onNoCallback;
    private Action onOkCallback;

    private void Awake()
    {
        // Debug.Log("Awake called in ConfirmationDialog");


        // Setup Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Assign button click listeners
        yesButton.onClick.AddListener(OnYesPressed);
        noButton.onClick.AddListener(OnNoPressed);
        okButton.onClick.AddListener(OnOkPressed);

        gameObject.SetActive(false); // Hide the dialog
    }

    public void ShowYesNo(string message, Action yesAction, Action noAction = null)
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        okButton.gameObject.SetActive(false);

        messageText.text = message;
        onYesCallback = yesAction;
        onNoCallback = noAction;

        gameObject.SetActive(true); // Reveal the dialog
    }

    public void ShowOK(string message, Action okAction = null)
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        okButton.gameObject.SetActive(true);

        messageText.text = message;
        onOkCallback = okAction;

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

    private void OnOkPressed()
    {
        gameObject.SetActive(false); // Hide the dialog
        onOkCallback?.Invoke();      // Execute "OK" action (if one was given)
    }
}
