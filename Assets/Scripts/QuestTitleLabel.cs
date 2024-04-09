using System;
using TMPro;
using UnityEngine;

public class QuestTitleLabel : MonoBehaviour {
     private void OnEnable() {
        QuestManager.Instance.RegisterTitleLabel(GetComponent<TextMeshPro>());
    }
}