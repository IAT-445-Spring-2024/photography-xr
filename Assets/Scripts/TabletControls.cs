using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TabletControls: MonoBehaviour {
    [SerializeField] private GameObject photoDisplay;
    private PlayerInputActions inputActions;

    private void Awake() {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.ShowAlbum.performed += OnMenuPressed;
    }

    private void OnMenuPressed(InputAction.CallbackContext context) {
        photoDisplay.SetActive(!photoDisplay.activeSelf);
    }
}
