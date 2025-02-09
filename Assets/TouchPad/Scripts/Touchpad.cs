using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.EventSystems;

public class Touchpad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    [SerializeField] private float sensitivity = 0.2f; // Adjust for smoother/faster aiming
    [SerializeField] private float verticalClamp = 80f; // Clamp vertical rotation to avoid flipping
    
    private CinemachineBrain cinemachineBrain;
    private CinemachineCamera playerCamera;

    private bool isDragging = false;
    private Vector2 lastTouchPosition;
    private float cameraPitch = 0f; // Track vertical rotation

    // Events for notifying other components
    public static System.Action OnPointerDownEvent;
    public static System.Action<Vector2> OnDragEvent;
    public static System.Action OnPointerUpEvent;
    
    private void Awake() {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void Start() {
        playerCamera = cinemachineBrain.ActiveVirtualCamera as CinemachineCamera;
    }

    public void OnPointerDown(PointerEventData eventData) {
        isDragging = true;
        lastTouchPosition = eventData.position; // Reset to avoid sudden jumps
        OnPointerDownEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData) {
        isDragging = false;
        OnPointerUpEvent?.Invoke();
    }

    public void OnDrag(PointerEventData eventData) {
        if (!isDragging || playerCamera == null) return;

        Vector2 delta = eventData.position - lastTouchPosition;
        lastTouchPosition = eventData.position;

        // Multiply by Time.deltaTime to ensure frame rate independence
        float frameRateFactor = Time.deltaTime * 60f; // Normalize to 60 FPS
        
        float rotationX = delta.y * sensitivity * frameRateFactor;
        float rotationY = delta.x * sensitivity * frameRateFactor;

        // Apply rotation
        cameraPitch -= rotationX;
        cameraPitch = Mathf.Clamp(cameraPitch, -verticalClamp, verticalClamp);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, playerCamera.transform.localRotation.eulerAngles.y + rotationY, 0);
        
        OnDragEvent?.Invoke(delta);
    }
}