using UnityEngine;

public class CameraLook : MonoBehaviour {
    public CustomJoystick joystick; // Reference to the joystick
    public float sensitivity = 300f; // Adjust for smoother rotation

    // Vertical limits
    public float minVerticalAngle = -30f; // Lower limit
    public float maxVerticalAngle = 80f;  // Upper limit

    // Horizontal limits
    public float minHorizontalAngle = -90f; // Left limit
    public float maxHorizontalAngle = 90f;  // Right limit

    private float rotationX = 0f;
    private float rotationY = 0f;

    
    void Update() {
        // Get joystick input
        Vector2 input = joystick.GetJoystickInput();

        // Multiply by Time.deltaTime to ensure frame rate independence
        float frameRateFactor = Time.deltaTime * 60f; // Normalize to 60 FPS

        // Adjust rotation
        rotationX += input.x * sensitivity * frameRateFactor;
        rotationY -= input.y * sensitivity * frameRateFactor;

        // Clamp rotations
        rotationY = Mathf.Clamp(rotationY, minVerticalAngle, maxVerticalAngle);
        rotationX = Mathf.Clamp(rotationX, minHorizontalAngle, maxHorizontalAngle);

        // Apply rotation
        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
    }
}
