using UnityEngine;
using System.Collections;

public class CursorArrow : MonoBehaviour
{
    public Transform target;
    public PlayerMovement playerAmmo;
    private SpriteRenderer spriteRenderer;
    private PauseMenu pauseMenu;
    private float startTime;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Cursor.visible = false;
    }

    void Update()
    {
        if(PauseMenu.isPaused == false)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            transform.position = mouseWorldPos;

            if (Input.GetMouseButton(1))
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                Vector3 direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            if(playerAmmo.ammo == 2)
            {
                spriteRenderer.color = new Color(0f, 1f, 0.498f);
            }
            if(playerAmmo.ammo == 1)
            {
                spriteRenderer.color = new Color(0.345f, 0.886f, 1f);
            }
            if(playerAmmo.ammo < 1)
            {
                spriteRenderer.color = new Color(0.753f, 1f, 0.965f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                transform.localScale = new Vector3(0.15f, 0.15f, 1f);
                StartCoroutine(ScaleBack(0.1f));
            }

            if (Input.GetMouseButtonDown(1))
            {
                transform.localScale = new Vector3(0.15f, 0.15f, 1f);
                StartCoroutine(ScaleBack(0.1f));
            }
        }
    }

    IEnumerator ScaleBack(float time)
    {
        yield return new WaitForSeconds(time);
        transform.localScale = new Vector3(0.1f, 0.1f, 1f);
    }

}
