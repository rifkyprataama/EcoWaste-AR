using UnityEngine;
using System.Collections;
public class AnimasiButton : MonoBehaviour
{
    // Assign the GameObject asset (the button) in the inspector
    public GameObject button;

    // delay before the button becomes visible and starts animation
    public float delay = 2f;

    private bool isAnimating = false;
    private bool timerFinished = false;
    private float timer;
    private Vector3 originalScale;
    private bool useScaleHide = false;

    void Start()
    {
        timer = delay;
        if (button)
        {
            originalScale = button.transform.localScale;
            if (button == gameObject)
            {
                button.transform.localScale = Vector3.zero;
                useScaleHide = true;
            }
            else
            {
                button.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (!timerFinished && button)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timerFinished = true;
                button.SetActive(true);
                StartCoroutine(AnimateButton());
            }
        }
    }

    System.Collections.IEnumerator AnimateButton()
    {
        if (!button || isAnimating)
            yield break;

        isAnimating = true;

        Vector3 startScale = button.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        float duration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            button.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            button.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        button.transform.localScale = originalScale;
        isAnimating = false;
    }
}
