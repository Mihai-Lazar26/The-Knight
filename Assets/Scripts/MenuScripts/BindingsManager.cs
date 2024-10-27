using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingsManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;
    public void ResetAllBindings() {
        foreach (InputActionMap map in _inputActions.actionMaps) {
            map.RemoveAllBindingOverrides();
        }
        PlayerPrefs.DeleteKey("rebinds");
    }
}
