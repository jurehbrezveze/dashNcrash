using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AlwaysLoadedScript : MonoBehaviour
{
    void Awake()
{
    if (FindObjectsOfType<AlwaysLoadedScript>().Length > 1)
    {
        Destroy(gameObject); // Prevent duplicates
    }
    else
    {
        DontDestroyOnLoad(gameObject);
    }
}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadScene("MainMenu");
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

