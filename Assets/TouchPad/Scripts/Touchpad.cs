using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.EventSystems;

namespace CustomTouchpad.Touchpad {
    public class Touchpad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
        [SerializeField] private float sensitivity = 0.2f; // Adjust for smoother/faster aiming
        [SerializeField] private float verticalClamp = 80f; // Clamp vertical rotation to avoid flipping

        [SerializeField] private RectTransform specialArea;
        private CinemachineBrain cinemachineBrain;
        private CinemachineCamera playerCamera;

        private bool isDragging = false;
        private Vector2 lastTouchPosition;
        private float cameraPitch = 0f; // Track vertical rotation

        // Events for notifying other components
        public System.Action OnPointerDownEvent;
        public System.Action<Vector2> OnDragEvent;
        public System.Action OnPointerUpEvent;
        public System.Action<PointerEventData> OnSpecialAreaSelection;

        private void Awake() {
            cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }

        private void Start() {
            playerCamera = cinemachineBrain.ActiveVirtualCamera as CinemachineCamera;
            // Debug.Log($"Obj: {gameObject.name}, Camera: {playerCamera.gameObject.name}");
        }
        
        private bool IsTouchInAllowedArea(PointerEventData eventData) {
            if (specialArea == null) return false; // No restriction if RectTransform is not assigned
            
            return RectTransformUtility.RectangleContainsScreenPoint(specialArea, eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData) {
            lastTouchPosition = eventData.position; // Reset to avoid sudden jumps
            isDragging = true;
            if (IsTouchInAllowedArea(eventData)) 
                OnSpecialAreaSelection?.Invoke(eventData);
            else 
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

            playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch,
                playerCamera.transform.localRotation.eulerAngles.y + rotationY, 0);

            OnDragEvent?.Invoke(delta);
        }

        public void UpdateSensitivity(bool isAiming) {
            if (isAiming)
                sensitivity = 0.01f;
            else
                sensitivity = 0.075f;
        }
    }
}