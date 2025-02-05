using UnityEngine;

public class JoystickController : MonoBehaviour {
    public CustomJoystick joystick;
    public float moveSpeed = 5f;
    public Transform player;
    
    void Update() {
        Vector2 moveDirection = joystick.GetJoystickInput();
        
        if (moveDirection.magnitude > 0.1f) {
            player.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            player.forward = moveDirection; // Rotate towards movement direction
        }
    }
}
