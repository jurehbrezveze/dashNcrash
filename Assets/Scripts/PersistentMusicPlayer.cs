using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PersistentMusicPlayer : MonoBehaviour
{
    [Header("Default Playlist (used in general scenes)")]
    public Track[] playlist;
    public float volume = 0.5f;

    [Header("Scene-Specific Tracks")]
    public SceneMusicMapping[] sceneTracks;

    private AudioSource audioSource;
    private static PersistentMusicPlayer instance;
    private Dictionary<string, Track> sceneMusicDict;
    private Coroutine playlistCoroutine;
    private Coroutine beatCoroutine;
    private List<Track> shufflePool = new List<Track>();

    [System.Serializable]
    public class Track
    {
        public AudioClip clip;
        public float bpm = 120f;
    }

    [System.Serializable]
    public class SceneMusicMapping
    {
        public string sceneName;
        public Track track;
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

        sceneMusicDict = new Dictionary<string, Track>();
        foreach (var mapping in sceneTracks)
        {
            if (!string.IsNullOrEmpty(mapping.sceneName) && mapping.track != null && mapping.track.clip != null)
            {
                sceneMusicDict[mapping.sceneName] = mapping.track;
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (playlist.Length > 0)
        {
            ResetShufflePool();
            playlistCoroutine = StartCoroutine(PlaylistLoop());
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneMusicDict.TryGetValue(scene.name, out Track specificTrack))
        {
            if (playlistCoroutine != null)
            {
                StopCoroutine(playlistCoroutine);
                playlistCoroutine = null;
            }

            PlayTrack(specificTrack, true);
        }
        else
        {
            if (playlist.Length > 0)
            {
                if (playlistCoroutine != null)
                {
                    StopCoroutine(playlistCoroutine);
                }

                audioSource.Stop();
                audioSource.loop = false;

                ResetShufflePool();
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
                if (shufflePool.Count == 0)
                    ResetShufflePool();

                int randomIndex = Random.Range(0, shufflePool.Count);
                Track nextTrack = shufflePool[randomIndex];
                shufflePool.RemoveAt(randomIndex);

                PlayTrack(nextTrack, false);
            }

            yield return null;
        }
    }

    void PlayTrack(Track track, bool loop)
    {
        audioSource.Stop();
        audioSource.clip = track.clip;
        audioSource.loop = loop;
        audioSource.Play();

        if (beatCoroutine != null)
            StopCoroutine(beatCoroutine);

        beatCoroutine = StartCoroutine(BeatPulse(track.bpm));
    }

    IEnumerator BeatPulse(float bpm)
    {
        float interval = 60f / bpm;

        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(interval);

            foreach (Camera cam in Camera.allCameras)
            {
                if (cam != null)
                {
                    StartCoroutine(PulseLeftRight(cam.transform, 0.1f, 0.05f));
                }
            }
        }
    }

    IEnumerator PulseLeftRight(Transform cam, float duration, float strength)
    {
        Vector3 originalPos = cam.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Mathf.Sin((elapsed / duration) * Mathf.PI * 2) * strength;
            cam.position = originalPos + new Vector3(offsetX, 0f, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.position = originalPos;
    }



    void ResetShufflePool()
    {
        shufflePool.Clear();
        shufflePool.AddRange(playlist);
    }
}
