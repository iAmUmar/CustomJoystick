using UnityEngine;
using UnityEngine.EventSystems;

public class CustomJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {
    
    public RectTransform background; // Joystick background
    public RectTransform handle; // Joystick handle (movable part)
    private Vector2 startPosition;
    private Vector2 inputVector = Vector2.zero;
    public float maxRange = 100f; // Max joystick movement range
    public float dragThreshold = 10f; // Minimum movement required to start dragging

    private bool isDragging = false;

    // Events for notifying other components
    public static System.Action OnPointerDownEvent;
    public static System.Action<Vector2> OnDragEvent;
    public static System.Action OnPointerUpEvent;

    public void OnPointerDown(PointerEventData eventData) {
        startPosition = eventData.position; // Store initial touch position
        isDragging = false; // Reset dragging state
        OnPointerDownEvent?.Invoke(); // Notify listeners
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 dragPos = eventData.position - (Vector2)background.position;
        float distance = Vector2.Distance(eventData.position, startPosition);

        // Check if the movement exceeds the drag threshold
        if (!isDragging && distance < dragThreshold) {
            return; // Ignore small movements
        }

        isDragging = true; // Start dragging

        inputVector = Vector2.ClampMagnitude(dragPos, maxRange);
        handle.anchoredPosition = inputVector;

        OnDragEvent?.Invoke(inputVector.normalized); // Notify listeners with direction
    }

    public void OnPointerUp(PointerEventData eventData) {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        isDragging = false;

        OnPointerUpEvent?.Invoke(); // Notify listeners
    }

    public Vector2 GetJoystickInput() {
        return inputVector.normalized; // Get direction
    }
}
