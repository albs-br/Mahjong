// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections.Generic;
// using System.Linq;

// public class UIManager : MonoBehaviour
// {
//     // Static instance of UIManager, accessible from any other script
//     public static UIManager Instance { get; private set; }

//     [Tooltip("Drag all UI Canvases (panels/menus) here via the Inspector.")]
//     [SerializeField] public List<Canvas> uiCanvases;

//     private void Awake()
//     {
//         // Enforce the singleton pattern
//         if (Instance != null && Instance != this)
//         {
//             Destroy(this.gameObject);
//         }
//         else
//         {
//             Instance = this;
//             // Optional: use DontDestroyOnLoad if the UI manager persists across scenes
//             // DontDestroyOnLoad(this.gameObject); 
//         }

//         // Ensure all canvases are initialized and set to inactive at start
//         foreach (var canvas in uiCanvases)
//         {
//             if (canvas != null)
//             {
//                 canvas.gameObject.SetActive(false);
//             }
//         }
//     }

//     /// <summary>
//     /// Opens a specific UI canvas by name and optionally closes others.
//     /// </summary>
//     /// <param name="canvasName">The name of the Canvas GameObject to open.</param>
//     /// <param name="closeOthers">Whether to close all other canvases.</param>
//     public void OpenCanvas(string canvasName, bool closeOthers = true)
//     {
//         Canvas targetCanvas = uiCanvases.FirstOrDefault(c => c.name == canvasName);

//         if (targetCanvas != null)
//         {
//             if (closeOthers)
//             {
//                 CloseAllCanvases();
//             }
//             targetCanvas.gameObject.SetActive(true);
//         }
//         else
//         {
//             Debug.LogWarning($"Canvas with name {canvasName} not found in UIManager list.");
//         }
//     }

//     /// <summary>
//     /// Closes a specific UI canvas by name.
//     /// </summary>
//     /// <param name="canvasName">The name of the Canvas GameObject to close.</param>
//     public void CloseCanvas(string canvasName)
//     {
//         Canvas targetCanvas = uiCanvases.FirstOrDefault(c => c.name == canvasName);
//         if (targetCanvas != null)
//         {
//             targetCanvas.gameObject.SetActive(false);
//         }
//     }

//     /// <summary>
//     /// Deactivates all managed UI canvases.
//     /// </summary>
//     public void CloseAllCanvases()
//     {
//         foreach (var canvas in uiCanvases)
//         {
//             if (canvas != null && canvas.gameObject.activeSelf)
//             {
//                 canvas.gameObject.SetActive(false);
//             }
//         }
//     }
// }
