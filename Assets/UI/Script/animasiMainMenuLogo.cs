using System.Collections;
using UnityEngine;

// Animate a UI logo from center to top while scaling down
public class animasiMainMenuLogo : MonoBehaviour
{
    [Tooltip("Target anchored Y position in local space (relative to RectTransform anchors)")]
    public float targetY = 200f;

    [Tooltip("Target scale (uniform)")]
    public float targetScale = 0.6f;

    [Tooltip("Duration of the animation in seconds")]
    public float duration = 1f;

    [Tooltip("Delay before the animation starts")]
    public float delay = 0.2f;

    [Tooltip("Optional easing curve (0..1 input, 0..1 output). Use linear if null.")]
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    RectTransform rt;
    Vector2 startPos;
    Vector3 startScale;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (rt == null)
        {
            Debug.LogWarning("animasiMainMenuLogo requires a RectTransform on the same GameObject.");
            enabled = false;
            return;
        }

        startPos = rt.anchoredPosition;
        startScale = rt.localScale;
    }

    void OnEnable()
    {
        // start the animation
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        Vector2 targetPos = new Vector2(startPos.x, targetY);
        Vector3 targetSc = Vector3.one * targetScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float e = ease != null ? ease.Evaluate(t) : t;

            rt.anchoredPosition = Vector2.LerpUnclamped(startPos, targetPos, e);
            rt.localScale = Vector3.LerpUnclamped(startScale, targetSc, e);

            yield return null;
        }

        rt.anchoredPosition = targetPos;
        rt.localScale = targetSc;
    }
}
