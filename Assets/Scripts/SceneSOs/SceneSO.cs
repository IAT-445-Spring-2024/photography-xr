using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scene Scriptable Object")]
public class SceneSO: ScriptableObject {
    public Texture2D texture;
    public int index;
    public string displayName;
}
