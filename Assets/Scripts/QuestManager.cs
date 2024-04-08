using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    public enum SceneName {
        forest,
        snowyLand,
        studio
    }
    [SerializeField] private SceneName sceneName = SceneName.studio;
    private Dictionary<SceneName, List<Quest>> allQuests = new();

    [Header("Forest Objects")]
    [SerializeField] PhysicalCamera physicalCamera;
    

    private void Start() {
        LoadQuestsForSceneWithName(sceneName);
    }

    private void LoadQuestsForSceneWithName(SceneName sceneName) {
        if (allQuests[sceneName] != null) { return; }
        switch(sceneName) {
            case SceneName.forest:
                break;
            case SceneName.snowyLand:
                List<Quest> forestQuests = new();
                allQuests.Add(sceneName, forestQuests);

                forestQuests.Add(new(
                    "Grab the Camera", 
                    "Put your right hand near the camera and press the grab key. Point to a subject you like.", 
                    () => {
                        return true;
                    }
                ));
                
                forestQuests.Add(new(
                    "Reduce Focal Length", 
                    "Press the A key to reduce the focal length of your camera. This effectively zooms in the view.", 
                    () => {
                        return true;
                    }
                ));

                forestQuests.Add(new(
                    "Increase Focal Length", 
                    "Press the B key on your right controller to increase the focal length. Aside from zooming in, notice how the background appear to be closer to your subject? People often use this effect to \"compress\" the space in a photo.", 
                    () => {
                        return true;
                    }
                ));

                forestQuests.Add(new(
                    "Zoom Out", 
                    "Press the A key on your right controller to zoom out.", 
                    () => {
                        return true;
                    }
                ));

                break;
            case SceneName.studio:
                break;
        }
    }

    public List<Quest> GetQuestsForSceneWithName(SceneName sceneName) {
        if (allQuests[sceneName] == null) {
            LoadQuestsForSceneWithName(sceneName);
        }
        return allQuests[sceneName];
    }
}