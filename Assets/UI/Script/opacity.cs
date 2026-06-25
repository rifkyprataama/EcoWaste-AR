using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Opacity : MonoBehaviour, IPointerClickHandler
{
    public float openDuration = 1f;
    public TextMeshProUGUI uiText;
    public float fadeDuration = 1f;
    public float holdDuration = 0.5f;
    public bool loop = true;

    bool isFadingOut;

    void Start()
    {
        if (uiText == null)
            uiText = GetComponent<TextMeshProUGUI>();

        if (uiText != null)
        {
            uiText.enabled = false;
            StartCoroutine(OpenAnimation());
        }
    }

    IEnumerator OpenAnimation()
    {
        float elapsed = 0f;
        Color color = uiText.color;
        color.a = 0f;
        uiText.color = color;
        uiText.enabled = true;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / openDuration);
            color.a = Mathf.Lerp(0f, 1f, t);
            uiText.color = color;
            yield return null;
        }

        color.a = 1f;
        uiText.color = color;

        StartCoroutine(FadeLoop());
    }

    public void OnPointerClick(PointerEventData _)
    {
        if (isFadingOut || uiText == null)
            return;

        StopAllCoroutines();
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return Fade(0f, 1f);
            yield return new WaitForSeconds(holdDuration);
            yield return Fade(1f, 0f);
            yield return new WaitForSeconds(holdDuration);

            if (!loop)
                break;
        }
    }

    IEnumerator FadeOutAndDestroy()
    {
        isFadingOut = true;
        yield return Fade(uiText.color.a, 0f);
        Destroy(gameObject);
    }

    IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color color = uiText.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = Mathf.Lerp(from, to, t);
            uiText.color = color;
            yield return null;
        }

        color.a = to;
        uiText.color = color;
    }
}
