using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashForce = 8f;
    public float bounceFactor = 8f;
    public TMP_Text faceText;
    private string[] faces = { "o0", "$$", "..", "00", "oo", "><", "ಠಠ", "--", "xx" };

    public Timer timer;
    //Random.Range(0, 9)

    public int ammo;
    
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Jump();

        if(ammo == 2)
        {
            faceText.text = faces[3];
        }

        if(ammo == 1)
        {
            faceText.text = faces[4];
        }
        if(ammo == 0)
        {
            faceText.text = faces[5];
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if(moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }
    void Jump()
    {
        {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float distanceToMouse = Vector2.Distance(rb.position, mousePosition);

        if (Input.GetMouseButtonDown(0) && ammo > 0) // Left mouse button
        {
            Vector2 direction = (mousePosition - rb.position);

            Vector2 jumpVelocity = new Vector2(direction.x, direction.y).normalized;

            if(ammo == 2)
            {
                jumpVelocity *= -dashForce;
            }

            if(ammo == 1)
            {
                jumpVelocity *= -jumpForce;
            }
            
            rb.velocity = Vector2.zero;
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
            rb.velocity = jumpVelocity;
            rb.angularVelocity = -jumpVelocity.x;
            ammo -= 1;

            //faceText.text = faces[Random.Range(0, 9)];

        }
    }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Wall"))
        {
            ammo = 2;
        }
        if(collision.gameObject.CompareTag("Bounce"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;

                if (normal.y > 0.5f) // Touching the bottom of the platform
                {
                    rb.AddForce(Vector2.up * bounceFactor, ForceMode2D.Impulse);
                }
                else if (normal.y < -0.5f) // Touching the top of the platform
                {
                    rb.AddForce(Vector2.down * bounceFactor, ForceMode2D.Impulse);
                }
                else if (normal.x > 0.5f) // Touching the left of the platform
                {
                    rb.AddForce(Vector2.right * bounceFactor, ForceMode2D.Impulse);
                }
                else if (normal.x < -0.5f) // Touching the right of the platform
                {
                    rb.AddForce(Vector2.left * bounceFactor, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "EndLevel")
        {
            timer.timerRunning = false;
        }
    }

}

