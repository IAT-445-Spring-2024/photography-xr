using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRotator : MonoBehaviour {
    [SerializeField] private GameObject terrain;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private Camera targetCamera;
    
    [ContextMenu("Rotate Trees")]
    private void Rotate() {
        if (targetCamera == null) return;

        GameObject treeParent = new GameObject();
        treeParent.name = "Flat Trees";
        treeParent.transform.SetParent(terrain.transform);
        TerrainData terrainData = terrain.GetComponent<Terrain>().terrainData;
        foreach (TreeInstance treeInstance in terrainData.treeInstances) {
            Vector3 treeWorldPosition = Vector3.Scale(treeInstance.position, terrainData.size) + Terrain.activeTerrain.transform.position;
            GameObject tree = Instantiate(treePrefab, treeWorldPosition, Quaternion.identity);
            tree.transform.SetParent(treeParent.transform);
            tree.transform.rotation = GetQuaternion(tree);
            tree.transform.localScale = new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale);
        }

        terrainData.treeInstances = new TreeInstance[0];
    }

    private Quaternion GetQuaternion(GameObject tree) {
        // Get the direction from the object to the camera
        Vector3 directionToCamera = tree.transform.position - targetCamera.transform.position;

        // Zero out the Y component of the direction vector to keep the object's Y axis rotation unchanged
        directionToCamera.y = 0;

        // Check if the direction vector is not too small to avoid floating point errors
        if (directionToCamera.sqrMagnitude < 0.0001f) return Quaternion.identity;

        // Calculate the new rotation towards the camera on the XZ plane
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

        // Apply the rotation
        return targetRotation;
    }

    // private Quaternion Negate(Quaternion quaternion) {
    //     quaternion.y *= -1;
    //     return quaternion;
    // }
}
