using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;

public class PhysicalCamera: MonoBehaviour {

    [SerializeField] private Volume volume;
    [SerializeField] private TextMeshPro textMesh;
    private DepthOfField depthOfField;

    private void Start() {
        RegisterShutterAction();
        SetUpFocus();
    }

    private void RegisterShutterAction() {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(OnActivate);
    }

    private void SetUpFocus() {
        volume.profile.TryGet(out depthOfField);
    }

    private void OnActivate(ActivateEventArgs args) {
        // TODO: Take photo.

    }

    private void FixedUpdate() {
        Ray ray = new(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            float hitDistance = Vector3.Distance(transform.position, hit.point);
            depthOfField.focusDistance.value = hitDistance;
            textMesh.text = hitDistance.ToString();
        }
    }
}
