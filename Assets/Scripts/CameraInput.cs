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
    }

    private void Update() {
        if (primaryButtonActionProperty.action.IsPressed()) {
            OnZoomOutPressed?.Invoke(this, EventArgs.Empty);
        } else if (secondaryButtonActionProperty.action.IsPressed()) {
            OnZoomInPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
