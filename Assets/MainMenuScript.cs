using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;

    void Start()
    {
        // Assign the methods to the buttons
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
