using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;
using System;

public class PhysicalCamera: MonoBehaviour {

    [SerializeField] private Volume volume;
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private float focusSpeed = 3f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private CameraInput cameraInput; // The lifecycle is binded to a game object.
    [SerializeField] private Camera viewingCamera;
    [SerializeField] private float lowestFocalLength = 20f;
    [SerializeField] private float highestFocalLength = 200;
    private DepthOfField depthOfField;

    private void Start() {
        RegisterShutterAction();
        RegisterControls();
        RegisterEffects();
    }

    private void RegisterControls() {
        cameraInput.OnZoomInPressed += PerformZoomIn;
        cameraInput.OnZoomOutPressed += PerformZoomOut;
    }

    private void PerformZoomIn(object sender, EventArgs args) {
        if (viewingCamera.focalLength >= highestFocalLength) { return; }
        viewingCamera.focalLength += 1 * zoomSpeed;
        textMesh.text = "Zooming In";
    }

    private void PerformZoomOut(object sender, EventArgs args) {
        if (viewingCamera.focalLength <= lowestFocalLength) { return; }
        viewingCamera.focalLength -= 1 * zoomSpeed;
        textMesh.text = "Zooming Out";
    }

    private void RegisterShutterAction() {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(OnActivate);
    }

    private void RegisterEffects() {
        volume.profile.TryGet(out depthOfField);
    }

    private void OnActivate(ActivateEventArgs args) {
        // TODO: Take photo.
        textMesh.text = "Took Photo";
    }

    private void FixedUpdate() {
        Ray ray = new(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            float hitDistance = Vector3.Distance(transform.position, hit.point);
            float currentFocusDistance = depthOfField.focusDistance.value;
            depthOfField.focusDistance.value += Time.deltaTime * (hitDistance - currentFocusDistance) * focusSpeed;
            // textMesh.text = hitDistance.ToString();
        }
    }
}
