using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelLoader : MonoBehaviour
{
    public GameObject player;  // Assign the player object in the Inspector
    public float delay = 5f;   // Delay before loading the next level

    private bool playerTouched = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && !playerTouched)
        {
            playerTouched = true;
            Debug.Log("Player touched the Finish. Next level loading in 5 seconds...");
            StartCoroutine(LoadNextLevelAfterDelay());
        }
    }

    private System.Collections.IEnumerator LoadNextLevelAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
