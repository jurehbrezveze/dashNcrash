using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform player; // Assign the player's Transform in the Inspector
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(0, Mathf.Max(player.position.y,0), transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}

