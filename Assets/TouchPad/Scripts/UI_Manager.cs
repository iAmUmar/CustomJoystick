using UnityEngine;

public class UI_Manager : MonoBehaviour {
    [SerializeField] private GameObject[] nonAimObjs;
    [SerializeField] private GameObject[] aimObjs;
    
    public void ToggleAimVisibility(bool toggleStatus) {
        foreach (var obj in nonAimObjs) 
            obj.SetActive(!toggleStatus);
        
        foreach (var obj in aimObjs) 
            obj.SetActive(toggleStatus);
    }
}
