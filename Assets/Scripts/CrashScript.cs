using UnityEngine;

public class CrashScript : MonoBehaviour
{
    public Rigidbody2D rb;                       // Assign in Inspector or auto-detect
    public float forceAmount = 10f;              // How strong the downward force is

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb == null)
        {
            Debug.LogError("DownwardBoost2D: No Rigidbody2D assigned or found on this GameObject.");
        }
    }

    void Update()
    {
        if (rb == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            rb.AddForce(Vector2.down * forceAmount, ForceMode2D.Impulse);
        }
    }
}
