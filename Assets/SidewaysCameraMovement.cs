using UnityEngine;

public class SidewaysCameraMovement : MonoBehaviour
{
    public float speed = 5.0f; // Speed of the camera movement

    // Update is called once per frame
    void Update()
    {
        // Get horizontal input (A/D, Left Arrow/Right Arrow)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Move the camera sideways based on input
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
    }
}