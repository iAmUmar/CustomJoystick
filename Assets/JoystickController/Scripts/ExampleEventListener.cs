using TMPro;
using UnityEngine;

public class ExampleEventListener : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI statusText;
    
    private void OnEnable() {
        CustomJoystick.OnPointerDownEvent += PointerDown;
        CustomJoystick.OnDragEvent += PointerDrag;
        CustomJoystick.OnPointerUpEvent += PointerUp;
    }
    
    private void OnDisable() {
        CustomJoystick.OnPointerDownEvent -= PointerDown;
        CustomJoystick.OnDragEvent -= PointerDrag;
        CustomJoystick.OnPointerUpEvent -= PointerUp;
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
