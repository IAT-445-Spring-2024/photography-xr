using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CaptureCamera : MonoBehaviour {
    
    private int currentFileNumber = 0;

    [ContextMenu("Capture")]
    private void Capture() {
        Camera camera = GetComponent<Camera>();
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
        camera.Render();

        Texture2D image = new(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG();
        // Destroy(image);

        string filePath = Application.dataPath + "photo" + currentFileNumber + ".png";
        File.WriteAllBytes(filePath, bytes);
        currentFileNumber += 1;
    }
}
