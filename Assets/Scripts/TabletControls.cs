using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TabletControls: MonoBehaviour {
    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private GameObject photoDisplay;
    [SerializeField] private GameObject sceneDisplay;
    private PlayerInputActions inputActions;

    private void Awake() {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.ShowAlbum.performed += OnMenuPressed;
        inputActions.Player.PreviousPhoto.performed += OnYPressed;
        inputActions.Player.NextPhoto.performed += OnXPressed;
        inputActions.Player.ConfirmScene.performed += OnTriggerPressed;
    }

    private void OnMenuPressed(InputAction.CallbackContext context) {
        if (!sceneDisplay.activeSelf && !photoDisplay.activeSelf) {
            photoDisplay.SetActive(true);
        } else if (photoDisplay.activeSelf) {
            photoDisplay.SetActive(false);
            sceneDisplay.SetActive(true);
        } else {
            sceneDisplay.SetActive(false);
        }
    }

    private void OnTriggerPressed(InputAction.CallbackContext context) {
        if (sceneDisplay.activeSelf) {
            transitionManager.Transition(sceneDisplay.GetComponent<SceneDisplay>().selectedSceneSO.index);
        }
    }

    private void OnYPressed(InputAction.CallbackContext context) {
        if (photoDisplay.activeSelf) {
            photoDisplay.GetComponent<PhotoDisplay>().DisplayPreviousPhoto();
        } else if (sceneDisplay.activeSelf) {
            sceneDisplay.GetComponent<SceneDisplay>().SelectPreviousScene();
        }
    }

    private void OnXPressed(InputAction.CallbackContext context) {
        if (photoDisplay.activeSelf) {
            photoDisplay.GetComponent<PhotoDisplay>().DisplayNextPhoto();
        } else if (sceneDisplay.activeSelf) {
            sceneDisplay.GetComponent<SceneDisplay>().SelectNextScene();
        }
    }
}
