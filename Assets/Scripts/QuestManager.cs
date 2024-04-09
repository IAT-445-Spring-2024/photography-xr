using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour {
    public static QuestManager Instance { get; private set; }
    private List<Quest> activeQuests = new List<Quest>();
    private Dictionary<string, bool> identifierToCompletionStatus = new();
    private TextMeshProUGUI titleLabel;
    private TextMeshProUGUI descriptionLabel;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        UpdateQuestUI();
    }

    private void OnQuestComplete(Quest quest) {
        identifierToCompletionStatus[quest.Identifier] = true;
        UpdateQuestUI();
    }

    private void UpdateQuestUI() {
        Quest nextQuest = GetNextUncompletedQuest();
        if (nextQuest != null) {
            Display(nextQuest);
        } else {
            titleLabel.text = "You have completed all the quests!";
            descriptionLabel.text = "Check out the other scenes or your album.";
        }
    }

    private void Display(Quest quest) {
        titleLabel.text = quest.Title;
        descriptionLabel.text = quest.Description;
    }
    
    private Quest GetNextUncompletedQuest() {
        activeQuests.Sort((quest1, quest2) => quest1.Order.CompareTo(quest2.Order));
        foreach (Quest quest in activeQuests) {
            if (!HasCompleted(quest)) {
                return quest;
            }
        }
        return null;
    }

    public void RegisterQuest(Quest quest) {
        if (!activeQuests.Contains(quest)) {
            activeQuests.Add(quest);
            quest.OnComplete += OnQuestComplete;
        }
    }

    public void DeregisterQuest(Quest quest) {
        if (activeQuests.Contains(quest)) {
            activeQuests.Remove(quest);
        }
    }

    public void RegisterTitleLabel(TextMeshProUGUI label) {
        titleLabel = label;
    }

    public void RegisterDescriptionLabel(TextMeshProUGUI label) {
        descriptionLabel = label;
    }

    private bool HasCompleted(Quest quest) {
        if (!identifierToCompletionStatus.ContainsKey(quest.Identifier)) {
            return false;
        } else {
            return identifierToCompletionStatus[quest.Identifier];
        }
    }
}