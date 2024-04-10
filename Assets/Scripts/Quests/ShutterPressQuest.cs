using System;
using UnityEngine;

class ShutterPressQuest: Quest {
    public override string Identifier { get { return "ShutterPressQuest"; } }
    public override string Title { get { return "Press Shutter"; } }
    public override string Description { get { return "Point at something you are interested in and press the shutter."; } }
    public override int Order { get { return 1; } }

    [SerializeField] private PhysicalCamera physicalCamera;

    private void Awake() {
        physicalCamera.DidPressTrigger += OnPressTrigger;
    }

    private void OnPressTrigger() {
        CompleteQuest();
    }
}