using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public enum ControllerMode {
    PC, Mobile
}
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private void Awake () {
        instance = this;
    }

    public bool verifiedTouch (Touch t, float whereScreen){ 
        if(whereScreen < t.position.x) {
            if(!EventSystem.current.IsPointerOverGameObject(t.fingerId)) {
                return true;
            } 
        }
        return false;
    }
}
