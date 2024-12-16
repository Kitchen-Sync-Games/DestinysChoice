using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject audioSourcePrefab;  


    public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip == null || audioSourcePrefab == null)
        {
            Debug.LogWarning("Clip or AudioSourcePrefab is missing.");
            return;
        }

        // Instantiate the audio source prefab at the given position
        GameObject audioObject = Instantiate(audioSourcePrefab, position, Quaternion.identity);

        // Get the AudioSource component from the instantiated prefab
        AudioSource audioSource = audioObject.GetComponent<AudioSource>();

        if (audioSource != null)
        {
            // Assign the clip to the AudioSource and play it
            audioSource.clip = clip;
            audioSource.Play();

            // Destroy the audio object after the clip finishes playing
            Destroy(audioObject, clip.length);
        }
        else
        {
            Debug.LogWarning("AudioSource component not found on prefab.");
        }
    }
}