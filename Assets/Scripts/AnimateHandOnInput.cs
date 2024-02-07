using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput: MonoBehaviour {
    [SerializeField] private InputActionProperty pinchAnimationAction;
    [SerializeField] private InputActionProperty gripAnimationAction;
    [SerializeField] private Animator handAnimator;

    // Keys for animation.
    private const string TRIGGER = "Trigger";
    private const string GRIP = "Grip";

    private void Update() {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat(TRIGGER, triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat(GRIP, gripValue);
    }
}
