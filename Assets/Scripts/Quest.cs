using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Quest: MonoBehaviour {
    public abstract string Identifier { get; }
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract int Order { get; }
    public event Action<Quest> OnComplete;

    private void OnEnable() {
        QuestManager.Instance.RegisterQuest(this);
    }

    private void OnDisable() {
        QuestManager.Instance.DeregisterQuest(this);
    }
}