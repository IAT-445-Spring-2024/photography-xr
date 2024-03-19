using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;
using System;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;

public class PhysicalCamera: MonoBehaviour {

    [SerializeField] private Volume volume;
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private float focusSpeed = 3f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private CameraInput cameraInput; // The lifecycle is binded to a game object.
    [SerializeField] private Camera cameraFX;
    [SerializeField] private Camera viewCamera;
    [SerializeField] private float lowestFocalLength = 20f;
    [SerializeField] private float highestFocalLength = 200f;
    private DepthOfField depthOfField;

    // TODO: This is not the best separation of concerns. 
    [SerializeField] private Slider zoomSlider;

    // MARK: Image Capture Properties
    private int currentFileNumber = 0;
    [SerializeField] private GameObject printedPhotoPrefab;
    [SerializeField] private GameObject photoPrintAnchor;
    private GameObject photoPrinting;
    [SerializeField] private float printSpeed = 1f;
    [SerializeField] private float printOffset = 5f;

    private void Start() {
        RegisterShutterAction();
        RegisterControls();
        RegisterEffects();
    }
    
    private void Update() {
        if (photoPrinting != null) {
            Vector3 targetDestination = photoPrintAnchor.transform.localPosition;
            targetDestination.y += printOffset;
            photoPrinting.transform.localPosition = Vector3.Lerp(
                photoPrinting.transform.localPosition,
                targetDestination,
                Time.deltaTime * printSpeed
            );

            float absoluteDistance = Vector3.Distance(targetDestination, photoPrinting.transform.localPosition);
            float distanceToStartFade = 0.1f;
            if (absoluteDistance < distanceToStartFade) {
                Color color = photoPrinting.GetComponent<Renderer>().material.color;
                color.a = 1 - (distanceToStartFade - absoluteDistance);
                photoPrinting.GetComponent<Renderer>().material.color = color;
            }
        }
    }

    private void RegisterControls() {
        cameraInput.OnZoomInPressed += PerformZoomIn;
        cameraInput.OnZoomOutPressed += PerformZoomOut;
    }

    private void PerformZoomIn(object sender, EventArgs args) {
        if (cameraFX.focalLength >= highestFocalLength) { return; }
        cameraFX.focalLength += 1 * zoomSpeed;
        viewCamera.focalLength += 1 * zoomSpeed;
        textMesh.text = "Zooming In";
        UpdateSliderValue();
    }

    private void PerformZoomOut(object sender, EventArgs args) {
        if (cameraFX.focalLength <= lowestFocalLength) { return; }
        cameraFX.focalLength -= 1 * zoomSpeed;
        viewCamera.focalLength -= 1 * zoomSpeed;
        textMesh.text = "Zooming Out";
        UpdateSliderValue();
    }

    private void UpdateSliderValue() {
        float value = (cameraFX.focalLength - lowestFocalLength) / (highestFocalLength - lowestFocalLength);
        zoomSlider.value = value;
    }

    private void RegisterShutterAction() {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(OnActivate);
    }

    private void RegisterEffects() {
        volume.profile.TryGet(out depthOfField);
    }

    private void OnActivate(ActivateEventArgs args) {
        // TODO: update action.
        textMesh.text = "Took Photo";
        CaptureImage();
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

    // MARK: - Image Capture
    private void CaptureImage() {
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = cameraFX.targetTexture;
        cameraFX.Render();

        Texture2D image = new(cameraFX.targetTexture.width, cameraFX.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cameraFX.targetTexture.width, cameraFX.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG();
        // Destroy(image);

        string filePath = Application.dataPath + "photos" + currentFileNumber + ".png";
        File.WriteAllBytes(filePath, bytes);
        currentFileNumber += 1;
        PrintPhoto(image);
    }

    private void PrintPhoto(Texture2D imageTexture) {
        photoPrinting = Instantiate(printedPhotoPrefab);
        photoPrinting.transform.SetParent(transform);
        photoPrinting.transform.localPosition = photoPrintAnchor.transform.localPosition;
        photoPrinting.GetComponent<Renderer>().material.mainTexture = imageTexture;
    }
}
