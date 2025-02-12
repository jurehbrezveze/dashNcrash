using UnityEngine;

public class PlatformSpin : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public bool spinClockwise = false;

    void Update()
    {
        if(!spinClockwise)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }
}