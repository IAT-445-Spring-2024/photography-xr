using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Reference: https://www.youtube.com/watch?v=TtuOX3pNMDE&t=63s
public class MeshCombiner : MonoBehaviour {
    [SerializeField] private List<MeshFilter> sourceMeshFilters;
    [SerializeField] private MeshFilter targetMeshFilter;

    [ContextMenu("Combine Meshes")]
    private void CombineMeshes() {
        var combineInstances = new CombineInstance[sourceMeshFilters.Count];

        for (var i = 0; i < sourceMeshFilters.Count; i++) {
            // Should be mesh if we want to use this in the game.
            combineInstances[i].mesh = sourceMeshFilters[i].sharedMesh;
            combineInstances[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combineInstances);
        targetMeshFilter.sharedMesh = mesh;
        targetMeshFilter.sharedMesh.name = "Combined Mesh";
        SaveMesh(targetMeshFilter.sharedMesh, gameObject.name, false, true);
    }

    private void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh) {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}
