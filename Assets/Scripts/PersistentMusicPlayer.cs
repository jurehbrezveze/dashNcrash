using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PersistentMusicPlayer : MonoBehaviour
{
    [Header("Default Playlist (used in general scenes)")]
    public AudioClip[] playlist;
    public float volume = 0.5f;

    [Header("Scene-Specific Tracks")]
    public SceneMusicMapping[] sceneTracks; // Scene -> AudioClip mapping

    private AudioSource audioSource;
    private static PersistentMusicPlayer instance;
    private int currentTrackIndex = 0;
    private Dictionary<string, AudioClip> sceneMusicDict;
    private Coroutine playlistCoroutine;

    [System.Serializable]
    public class SceneMusicMapping
    {
        public string sceneName;
        public AudioClip track;
    }

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

        sceneMusicDict = new Dictionary<string, AudioClip>();
        foreach (var mapping in sceneTracks)
        {
            if (!string.IsNullOrEmpty(mapping.sceneName) && mapping.track != null)
            {
                sceneMusicDict[mapping.sceneName] = mapping.track;
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        // Start the playlist coroutine immediately
        if (playlist.Length > 0)
        {
            playlistCoroutine = StartCoroutine(PlaylistLoop());
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Scene-specific track
        if (sceneMusicDict.TryGetValue(scene.name, out AudioClip specificClip))
        {
            // Stop playlist coroutine if running
            if (playlistCoroutine != null)
            {
                StopCoroutine(playlistCoroutine);
                playlistCoroutine = null;
            }

            audioSource.Stop();
            audioSource.clip = specificClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            // Back to general playlist
            if ((playlistCoroutine == null) && playlist.Length > 0)
            {
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = playlist[currentTrackIndex];
                audioSource.Play();
                playlistCoroutine = StartCoroutine(PlaylistLoop());
            }
        }
    }

    IEnumerator PlaylistLoop()
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                // Play next song in order
                audioSource.clip = playlist[currentTrackIndex];
                audioSource.Play();

                // Move to next track for the following iteration
                currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
            }

            yield return null;
        }
    }
}
