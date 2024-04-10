using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager: MonoBehaviour {
    [SerializeField] Fade fade;

    public void transition(int sceneIndex) {
        StartCoroutine(SceneTransitionRoutine(sceneIndex));
    }

    IEnumerator SceneTransitionRoutine(int sceneIndex) {
        fade.FadeOut();
        yield return new WaitForSeconds(fade.duration);
        SceneManager.LoadScene(sceneIndex);
    }
}
