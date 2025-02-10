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
            
            ApplyRotation(Vector2.zero);
        }
        
        private bool IsTouchInAllowedArea(PointerEventData eventData) {
            if (specialArea == null) return false; // No restriction if RectTransform is not assigned
            
            return RectTransformUtility.RectangleContainsScreenPoint(specialArea, eventData.position);
        }

        private void ApplyRotation(Vector2 delta) {
            float rotationX = delta.y * sensitivity;
            float rotationY = delta.x * sensitivity;

            // Apply vertical clamping
            cameraPitch = Mathf.Clamp(cameraPitch - rotationX, -verticalClamp, verticalClamp);

            // Rotate camera
            playerCamera.transform.localRotation = Quaternion.Euler(
                cameraPitch, playerCamera.transform.localRotation.eulerAngles.y + rotationY, 0);
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
            
            ApplyRotation(delta);

            OnDragEvent?.Invoke(delta);
        }

        public void UpdateSensitivity(bool isAiming) {
            sensitivity = isAiming ? 0.01f : 0.05f;
        }
    
    }
}