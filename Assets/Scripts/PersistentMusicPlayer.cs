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
    private Coroutine musicCoroutine;

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

        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
        }

        if (sceneMusicDict.TryGetValue(scene.name, out AudioClip specificClip))
        {
            audioSource.clip = specificClip;
            audioSource.Play();
        }
        else if (playlist.Length > 0)
        {
            musicCoroutine = StartCoroutine(PlayMusicLoop());
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
