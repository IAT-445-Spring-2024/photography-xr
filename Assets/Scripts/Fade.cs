using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Fade: MonoBehaviour {
    private Renderer fadeRenderer;
    public float duration = 0.5f;

    private void Start() {
        fadeRenderer = GetComponent<Renderer>();
        FadeIn();
    }

    public void FadeIn() {
        PerformFade(1, 0);
    }

    public void FadeOut() {
        PerformFade(0, 1);
    }

    private void PerformFade(float alphaIn, float alphaOut) {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut) {
        float timer = 0f;
        while (timer <= duration) {
            Color color = fadeRenderer.material.color;
            color.a = Mathf.Lerp(alphaIn, alphaOut, timer / duration);
            fadeRenderer.material.color = color;

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
