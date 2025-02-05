using TMPro;
using UnityEngine;

public class TouchpadEventListener : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI statusText;
    
    private void OnEnable() {
        Touchpad.OnPointerDownEvent += PointerDown;
        Touchpad.OnDragEvent += PointerDrag;
        Touchpad.OnPointerUpEvent += PointerUp;
    }
    
    private void OnDisable() {
        Touchpad.OnPointerDownEvent -= PointerDown;
        Touchpad.OnDragEvent -= PointerDrag;
        Touchpad.OnPointerUpEvent -= PointerUp;
    }

    private void PointerDown() {
        statusText.text = "Pointer Down";
    }
    
    private void PointerUp() {
        statusText.text = "Pointer Up";
    }
    
    private void PointerDrag(Vector2 vectorDrag) {
        statusText.text = "Pointer Dragging";
    }
}
