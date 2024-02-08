using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInput : MonoBehaviour {
    public event EventHandler<EventArgs> OnZoomInPressed;
    public event EventHandler<EventArgs> OnZoomOutPressed;
    [SerializeField] private InputActionReference primaryButtonActionProperty;
    [SerializeField] private InputActionReference secondaryButtonActionProperty;

    private void Start() {
        primaryButtonActionProperty.action.Enable();
        secondaryButtonActionProperty.action.Enable();
        primaryButtonActionProperty.action.performed += Primary_Performed;
        secondaryButtonActionProperty.action.performed += Secondary_Performed;
    }

    private void Primary_Performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        OnZoomOutPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Secondary_Performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        OnZoomInPressed?.Invoke(this, EventArgs.Empty);
    }
}
