using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    [Header("Audio Setup")]
    // Drag your AudioSource component here in the Inspector
    [SerializeField] private AudioSource audioSource; 
    
    // Drag your sound effect asset (.mp3, .wav) here in the Inspector
    [SerializeField] private AudioClip soundEffect;   

    void Update()
    {
        // // Example trigger: Plays when the Spacebar is pressed
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     PlaySFX();
        // }
    }

    // Call this function from another script, a UI button, or an event
    public void PlaySFX()
    {
        if (audioSource != null && soundEffect != null)
        {
            // PlayOneShot takes the clip and a volume scale (1.0f is full volume)
            audioSource.PlayOneShot(soundEffect, 1.0f);
        }
    }
}
