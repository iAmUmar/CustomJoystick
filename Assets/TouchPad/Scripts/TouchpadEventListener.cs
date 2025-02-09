using TMPro;
using UnityEngine;
using CustomTouchpad.Touchpad;
using UnityEngine.EventSystems;

public class TouchpadEventListener : MonoBehaviour {
    [SerializeField] private UI_Manager uiManagerObj;
    [SerializeField] private Touchpad touchpadObj;
    [SerializeField] private TextMeshProUGUI statusText;

    private bool isSpecialArea = false;
    
    private void OnEnable() {
        touchpadObj.OnPointerDownEvent += PointerDown;
        touchpadObj.OnDragEvent += PointerDrag;
        touchpadObj.OnPointerUpEvent += PointerUp;
        touchpadObj.OnSpecialAreaSelection += ToggleSpecialArea;
    }
    
    private void OnDisable() {
        touchpadObj.OnPointerDownEvent -= PointerDown;
        touchpadObj.OnDragEvent -= PointerDrag;
        touchpadObj.OnPointerUpEvent -= PointerUp;
        touchpadObj.OnSpecialAreaSelection -= ToggleSpecialArea;
    }

    private void ToggleSpecialArea(PointerEventData eventData) {
        statusText.text = "Pointer Down";
        uiManagerObj.ToggleAimVisibility(true);
        isSpecialArea = true;
        // Debug.Log($"Special Area Got Hit =>  x: {eventData.delta.x}, y: {eventData.delta.y}");
    }

    private void PointerDown() {
        statusText.text = "Pointer Down";
        isSpecialArea = false;
    }
    
    private void PointerUp() {
        statusText.text = "Pointer Up";
        uiManagerObj.ToggleAimVisibility(false);
        isSpecialArea = false;
    }
    
    private void PointerDrag(Vector2 vectorDrag) {
        statusText.text = "Pointer Dragging";
        // Debug.Log($"EventData= x: {vectorDrag.x}, y: {vectorDrag.y}");
    }
}
