using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashForce = 8f;
    public float bounceFactor = 8f;
    public TMP_Text timerText;
    public TMP_Text faceText;
    private bool timerRunning = true;
    private float timeRemaining = 0f;
    private string[] faces = { "o0", "$$", "..", "00", "oo", "><", "ಠಠ", "--", "xx" };
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
        Move();
        Jump();
        Timer();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if(moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void Timer()
    {
        if (timerRunning)
        {
            timeRemaining += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        int milliseconds = Mathf.FloorToInt((timeRemaining * 100) % 100); // Get two-digit milliseconds

        timerText.text = string.Format("{0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
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

            faceText.text = faces[Random.Range(0, 9)];

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
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * bounceFactor, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EndLevel")
        {
            timerRunning = false;
        }
    }
    
}

