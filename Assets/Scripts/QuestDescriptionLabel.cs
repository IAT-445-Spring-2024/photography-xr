using System;
using TMPro;
using UnityEngine;

public class QuestDescriptionLabel : MonoBehaviour {
     private void OnEnable() {
        QuestManager.Instance.RegisterDescriptionLabel(GetComponent<TextMeshProUGUI>());
    }
}