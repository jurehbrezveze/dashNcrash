using UnityEngine;
using UnityEngine.SceneManagement;

public class AlwaysLoadedScript : MonoBehaviour
{
    public static AlwaysLoadedScript instance;
    private string currentScene;
    string lastSavedScene;
    int maxLevel = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        lastSavedScene = "";
        maxLevel = PlayerPrefs.GetInt("LastLevel", 1);
    }

    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != "MainMenu" && currentScene != lastSavedScene)
        {
            int levelNumber = 0;

            if (currentScene.StartsWith("Level"))
            {
                string numberPart = currentScene.Substring("Level".Length);
                int.TryParse(numberPart, out levelNumber);
            }

            if(levelNumber > maxLevel)
            {
                PlayerPrefs.SetInt("LastLevel", levelNumber);
                PlayerPrefs.Save();
                lastSavedScene = currentScene;
            }
        }

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
