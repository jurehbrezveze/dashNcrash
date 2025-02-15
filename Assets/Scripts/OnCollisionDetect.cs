using UnityEngine;

public class OnCollisionDetect : MonoBehaviour
{
    public GameObject childPrefab; // Assign this in the inspector
    private bool hasCollided = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided) // Prevent multiple collisions from adding multiple children
        {
            hasCollided = true;

            // Change color to white
            GetComponent<SpriteRenderer>().color = Color.white;

            // Instantiate the child object
            if (childPrefab != null)
            {
                GameObject child = Instantiate(childPrefab, transform.position, Quaternion.identity);
                child.transform.SetParent(transform);
            }
            else
            {
                Debug.LogWarning("Child Prefab is not assigned in the inspector!");
            }
        }
    }
}
