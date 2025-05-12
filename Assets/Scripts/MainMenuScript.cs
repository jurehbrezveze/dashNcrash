using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public int maxLevel;
    private AlwaysLoadedScript alwaysLoadedScript;

    void Start()
    {
        GameObject obj = GameObject.Find("AlwaysLoaded");

        if (obj != null)
        {
            alwaysLoadedScript = obj.GetComponent<AlwaysLoadedScript>();
        }
        alwaysLoadedScript.time = 0f;
        alwaysLoadedScript.sMode = false;

        Cursor.visible = true;
        maxLevel = PlayerPrefs.GetInt("LastLevel", 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2");
            alwaysLoadedScript.sMode = true;
            SceneManager.LoadScene("Level20");
        }
    }

    public void PlayGame()
    {
        alwaysLoadedScript.sMode = true;
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
