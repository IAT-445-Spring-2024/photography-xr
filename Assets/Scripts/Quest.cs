using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Quest {
    public string title;
    public string description;
    public bool isCompleted = false;
    public Func<bool> CheckCompletion;

    public Quest(string title, string description, Func<bool> CheckCompletion) {
        this.title = title;
        this.description = description;
        this.CheckCompletion = CheckCompletion;
    }
}