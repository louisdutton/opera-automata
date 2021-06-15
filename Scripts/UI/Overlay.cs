using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Overlay : MonoBehaviour
{
    public float fadeInterval = .05f;

    private CanvasGroup canvasGroup;

    private void Awake() => canvasGroup = GetComponent<CanvasGroup>();
    private void Start()
    {
        canvasGroup.alpha = 1;
        EventManager.instance.OnGenerate += FadeIn;
    }

    private void OnDisable()
    {
        EventManager.instance.OnGenerate -= FadeIn;
    }

    private void FadeIn() => StartCoroutine(FadeRoutine(0));
    private void FadeOut() => StartCoroutine(FadeRoutine(1));

    private IEnumerator FadeRoutine(float target)
    {
        float increment = (target - canvasGroup.alpha) * fadeInterval;
        while (canvasGroup.alpha != target)
        {
            canvasGroup.alpha += increment;
            yield return null;
        }

        gameObject.SetActive(target != 0);
    }

}
