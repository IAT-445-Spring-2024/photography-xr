using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeReplacer : MonoBehaviour {
    [SerializeField] private GameObject terrain;
    [SerializeField] private GameObject treePrefab;

    [ContextMenu("Replace Trees")]
    private void Replace() {
        TerrainData terrainData = terrain.GetComponent<Terrain>().terrainData;
        foreach (TreeInstance treeInstance in terrainData.treeInstances) {
            Vector3 treeWorldPosition = Vector3.Scale(treeInstance.position, terrainData.size) + Terrain.activeTerrain.transform.position;
            Instantiate(treePrefab, treeWorldPosition, Quaternion.identity);
            Quaternion rotation = Quaternion.Euler(0, treeInstance.rotation * Mathf.Rad2Deg, 0);
            treePrefab.transform.rotation = rotation;
        }

        terrainData.treeInstances = new TreeInstance[0];
    }
}
