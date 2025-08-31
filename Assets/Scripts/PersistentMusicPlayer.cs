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
    public SceneMusicMapping[] sceneTracks; // Scene  AudioClip mapping

    private AudioSource audioSource;
    private static PersistentMusicPlayer instance;
    private int currentTrackIndex = 0;
    private Dictionary<string, AudioClip> sceneMusicDict;
    private Coroutine musicCoroutine;
    private bool usingSceneTrack = false;

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

        // Start music loop immediately
        if (playlist.Length > 0)
        {
            musicCoroutine = StartCoroutine(PlayMusicLoop());
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If this scene has a specific track  play that
        if (sceneMusicDict.TryGetValue(scene.name, out AudioClip specificClip))
        {
            usingSceneTrack = true;

            if (musicCoroutine != null)
            {
                StopCoroutine(musicCoroutine);
                musicCoroutine = null;
            }

            if (audioSource.clip != specificClip)
            {
                audioSource.clip = specificClip;
                audioSource.loop = true; // keep looping scene track
                audioSource.Play();
            }
        }
        else
        {
            // Back to general playlist mode
            if (usingSceneTrack && playlist.Length > 0)
            {
                usingSceneTrack = false;
                musicCoroutine = StartCoroutine(PlayMusicLoop());
            }
        }
    }

    IEnumerator PlayMusicLoop()
    {
        while (true)
        {
            // If nothing playing, or clip finished, move to next
            if (!audioSource.isPlaying)
            {
                audioSource.clip = playlist[currentTrackIndex];
                audioSource.loop = false;
                audioSource.Play();

                currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
            }

            yield return null; // check every frame
        }
    }
}
