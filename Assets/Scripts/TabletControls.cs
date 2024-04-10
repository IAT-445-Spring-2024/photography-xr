using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TabletControls: MonoBehaviour {
    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private GameObject photoDisplay;
    private PlayerInputActions inputActions;

    private void Awake() {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.ShowAlbum.performed += OnMenuPressed;
        inputActions.Player.PreviousPhoto.performed += OnYPressed;
        inputActions.Player.NextPhoto.performed += OnXPressed;
    }

    private void OnMenuPressed(InputAction.CallbackContext context) {
        // photoDisplay.SetActive(!photoDisplay.activeSelf);
        transitionManager.Transition(1);
    }

    private void OnYPressed(InputAction.CallbackContext context) {
        photoDisplay.GetComponent<PhotoDisplay>().DisplayPreviousPhoto();
    }

    private void OnXPressed(InputAction.CallbackContext context) {
        photoDisplay.GetComponent<PhotoDisplay>().DisplayNextPhoto();
    }
}
