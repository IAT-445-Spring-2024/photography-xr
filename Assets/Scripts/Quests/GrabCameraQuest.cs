using System;
using UnityEngine;

class GrabCameraQuest: Quest {
    public override string Identifier { get { return "GrabCameraQuest"; } }
    public override string Title { get { return "Welcome"; } }
    public override string Description { get { return "Use your right controller to grab the camera. Reach to the camera and press the grab button on the side."; } }
    public override int Order { get { return 0; } }
    [SerializeField] private PhysicalCamera physicalCamera;
    Vector3 cameraPosition;
    private bool hasCompleted = false;

    private void Start() {
        cameraPosition = physicalCamera.transform.position;
    }

    private void Update() {
        if (physicalCamera.transform.position != cameraPosition && !hasCompleted) {
            CompleteQuest();
            hasCompleted = true;
        }
    }
}