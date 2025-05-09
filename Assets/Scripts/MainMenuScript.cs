using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public int maxLevel;

    void Start()
    {
        maxLevel = PlayerPrefs.GetInt("LastLevel", 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }

    public void PlayGame()
    {
        Debug.Log("Loading next scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        int levelNumber = 0;

        if (sceneName.StartsWith("Level"))
        {
            string numberPart = sceneName.Substring("Level".Length);
            int.TryParse(numberPart, out levelNumber);
        }
        if(levelNumber <= maxLevel)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
