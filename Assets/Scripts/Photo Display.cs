using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotoDisplay: MonoBehaviour {
    [SerializeField] private PhysicalCamera physicalCamera;

    private int currentPhotoNumber = 0;

    private void OnEnable() {
        DisplayPhoto();
    }

    private void Start() {
        currentPhotoNumber = physicalCamera.currentFileNumber;
    }

    public void DisplayNextPhoto() {
        if (currentPhotoNumber < physicalCamera.currentFileNumber) {
            currentPhotoNumber += 1;
        }
        DisplayPhoto();
    }

    public void DisplayPreviousPhoto() {
        if (currentPhotoNumber > 0) {
            currentPhotoNumber -= 1;
        }
        DisplayPhoto();
    }

    private void DisplayPhoto() {
        if (!(currentPhotoNumber >= 0 && currentPhotoNumber <= physicalCamera.currentFileNumber)) {
            Debug.Log("No photo to be displayed.");
            return;
        }

        string filePath = Application.persistentDataPath + "photo" + currentPhotoNumber + ".png";
        byte[] imageBytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2); // The size here is irrelevant; it will be replaced.
        bool isLoaded = texture.LoadImage(imageBytes); // Returns false if the image couldn't be loaded
        
        if (isLoaded) {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = texture;
        }
    }
}
