using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDisplay: MonoBehaviour {
    public SceneSO selectedSceneSO;
    [SerializeField] private List<SceneSO> sceneSOs;
    private Renderer displayRenderer;

    private void Start() {
        displayRenderer = GetComponent<Renderer>();
        if (sceneSOs.Count != 0) {
            selectedSceneSO = sceneSOs[0];
            displayRenderer.material.mainTexture = selectedSceneSO.texture;
        }
    }

    public void SelectPreviousScene() {
        // TODO: This is bad in many ways. Please improve it. 
        if ((selectedSceneSO.index - 1) >= 0) {
            selectedSceneSO = sceneSOs[selectedSceneSO.index - 1];
            displayRenderer.material.mainTexture = selectedSceneSO.texture;
        }
    }

    public void SelectNextScene() {
        // TODO: This is bad in many ways. Please improve it. 
        if ((selectedSceneSO.index + 1) < sceneSOs.Count) {
            selectedSceneSO = sceneSOs[selectedSceneSO.index + 1];
            displayRenderer.material.mainTexture = selectedSceneSO.texture;
        }
    }
}
