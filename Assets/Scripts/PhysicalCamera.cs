using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhysicalCamera: MonoBehaviour {

    private void Start() {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(OnActivate);
    }

    private void OnActivate(ActivateEventArgs args) {
        // TODO: Take photo.

    }
}
