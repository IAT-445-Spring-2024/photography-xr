using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    public static QuestManager Instance { get; private set; }
    private List<Quest> activeQuests = new List<Quest>();
    private Dictionary<string, bool> identifierToCompletionStatus = new();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    public void RegisterQuest(Quest quest) {
        if (!activeQuests.Contains(quest)) {
            activeQuests.Add(quest);
            quest.OnComplete += OnQuestComplete;
        }
    }

    private void OnQuestComplete(Quest quest) {
        
    }

    public void DeregisterQuest(Quest quest) {
        if (activeQuests.Contains(quest)) {
            activeQuests.Remove(quest);
        }
    }

    private bool HasCompleted(Quest quest) {
        if (!identifierToCompletionStatus.ContainsKey(quest.Identifier)) {
            return false;
        } else {
            return identifierToCompletionStatus[quest.Identifier];
        }
    }
}