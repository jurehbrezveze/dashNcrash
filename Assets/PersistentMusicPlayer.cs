using UnityEngine;
using System.Collections;

public class PersistentMusicPlayer : MonoBehaviour
{
    public AudioClip[] playlist; // Assign multiple music tracks in the Inspector
    public float volume = 0.5f;

    private AudioSource audioSource;
    private static PersistentMusicPlayer instance;
    private int currentTrackIndex = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        if (playlist.Length > 0)
        {
            StartCoroutine(PlayMusicLoop());
        }
        else
        {
            Debug.LogWarning("No audio clips assigned to the music playlist.");
        }
    }

    IEnumerator PlayMusicLoop()
    {
        while (true)
        {
            audioSource.clip = playlist[currentTrackIndex];
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);

            currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
        }
    }
}
