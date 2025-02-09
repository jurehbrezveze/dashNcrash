using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelLoader : MonoBehaviour
{
    public GameObject player;          // Assign the player object in the Inspector
    public string nextSceneName = "";  // Name of the next scene to load (assign in the Inspector)
    public float delay = 5f;           // Delay before loading the next level

    private bool playerTouched = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && !playerTouched)
        {
            playerTouched = true;
            Debug.Log($"Player touched the Finish. Next level '{nextSceneName}' loading in {delay} seconds...");
            StartCoroutine(LoadNextLevelAfterDelay());
        }
    }

    private System.Collections.IEnumerator LoadNextLevelAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not assigned!");
        }
    }
}
