using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public int maxLevel;
    private AlwaysLoadedScript alwaysLoadedScript;
    public GameObject pivot;
    public float rotationSpeed = 180f;
    public float turnAngle = 70f;

    private bool isRotating = false;
    private float rotatedAmount = 0f;

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
        //Cursor.lockState = CursorLockMode.Locked;
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
        if (isRotating)
        {
            float delta = rotationSpeed * Time.deltaTime;
            float step = Mathf.Sign(turnAngle) * delta;

            rotatedAmount += Mathf.Abs(step);

            if (rotatedAmount >= Mathf.Abs(turnAngle))
            {
                step -= Mathf.Sign(step) * (rotatedAmount - Mathf.Abs(turnAngle));
                isRotating = false;
            }

            pivot.transform.Rotate(0, 0, step);
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

    public void LvlShow()    
    {
        if (isRotating) return;
        turnAngle = 70f;
        rotatedAmount = 0f;
        isRotating = true;
    }
    public void LvlHide()    
    {
        if (isRotating) return;
        turnAngle = -70f;
        isRotating = true;
        rotatedAmount = 0f;
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
