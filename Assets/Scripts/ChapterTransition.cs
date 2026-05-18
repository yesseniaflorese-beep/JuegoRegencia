using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ChapterTransition : MonoBehaviour
{
    [Header("Texto")]
    public TMP_Text chapterTitle;
    public TMP_Text chapterSubtitle;

    [Header("Telón")]
    public RectTransform curtainLeft;
    public RectTransform curtainRight;
    public float curtainOpenDuration = 1.2f;
    public float curtainCloseDuration = 0.8f;

    [Header("Tiempos")]
    public float transitionTime = 2f;

    void Start()
    {
        chapterTitle.text = SceneController.instance.GetCurrentChapterTitle();
        chapterSubtitle.text = SceneController.instance.GetCurrentChapterSubtitle();

        SetTextAlpha(0f);
        StartCoroutine(NextChapterAfterDelay());
    }

    IEnumerator NextChapterAfterDelay()
    {
        // 1 — Abrir telón
        yield return StartCoroutine(AnimateCurtain(opening: true));

        // 2 — Fade in texto
        yield return StartCoroutine(FadeText(0f, 1f, 0.4f));

        // 3 — Esperar
        yield return new WaitForSeconds(transitionTime);

        // 4 — Fade out texto
        yield return StartCoroutine(FadeText(1f, 0f, 0.3f));

        // 5 — Cerrar telón
        yield return StartCoroutine(AnimateCurtain(opening: false));

        SceneController.instance.LoadRealNextChapter();
    }

    IEnumerator AnimateCurtain(bool opening)
    {
        float duration = opening ? curtainOpenDuration : curtainCloseDuration;
        float screenHalf = Screen.width / 2f;

        // Al abrir: van de centro hacia afuera. Al cerrar: de afuera al centro.
        Vector2 leftFrom  = opening ? Vector2.zero : new Vector2(-screenHalf, 0f);
        Vector2 leftTo    = opening ? new Vector2(-screenHalf, 0f) : Vector2.zero;
        Vector2 rightFrom = opening ? Vector2.zero : new Vector2(screenHalf, 0f);
        Vector2 rightTo   = opening ? new Vector2(screenHalf, 0f) : Vector2.zero;

        curtainLeft.anchoredPosition  = leftFrom;
        curtainRight.anchoredPosition = rightFrom;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = EaseInOut(Mathf.Clamp01(elapsed / duration));
            curtainLeft.anchoredPosition  = Vector2.Lerp(leftFrom, leftTo, t);
            curtainRight.anchoredPosition = Vector2.Lerp(rightFrom, rightTo, t);
            yield return null;
        }

        curtainLeft.anchoredPosition  = leftTo;
        curtainRight.anchoredPosition = rightTo;
    }

    IEnumerator FadeText(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetTextAlpha(Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }
        SetTextAlpha(to);
    }

    void SetTextAlpha(float alpha)
    {
        var c1 = chapterTitle.color;    c1.a = alpha; chapterTitle.color = c1;
        var c2 = chapterSubtitle.color; c2.a = alpha; chapterSubtitle.color = c2;
    }

    float EaseInOut(float t) => t * t * (3f - 2f * t);
}