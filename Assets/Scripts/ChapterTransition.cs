using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ChapterTransition : MonoBehaviour
{
    [Header("Texto")]
    public TMP_Text chapterTitle;
    public TMP_Text chapterSubtitle;

    [Header("Telón (Sprite Sheet)")]
    public Image curtainImage;
    public Sprite[] curtainFrames;
    public float fps = 12f;

    [Header("Tiempos")]
    public float transitionTime = 2f;

    void Start()
    {
        chapterTitle.text = SceneController.instance.GetCurrentChapterTitle();
        chapterSubtitle.text = SceneController.instance.GetCurrentChapterSubtitle();

        // Debug para verificar configuración
        Debug.Log($"[Telón] curtainImage: {(curtainImage != null ? "✅" : "❌ NULL")}");
        Debug.Log($"[Telón] curtainFrames: {(curtainFrames != null ? curtainFrames.Length.ToString() : "❌ NULL")} sprites");
        if (curtainFrames != null && curtainFrames.Length > 0)
            Debug.Log($"[Telón] frame 0: {(curtainFrames[0] != null ? curtainFrames[0].name : "❌ NULL")}");

        SetTextAlpha(0f);
        StartCoroutine(NextChapterAfterDelay());
    }

    IEnumerator NextChapterAfterDelay()
    {
        yield return StartCoroutine(PlayFrames(forward: true));
        yield return StartCoroutine(FadeText(0f, 1f, 0.4f));
        yield return new WaitForSeconds(transitionTime);
        yield return StartCoroutine(FadeText(1f, 0f, 0.3f));
        yield return StartCoroutine(PlayFrames(forward: false));
        SceneController.instance.LoadRealNextChapter();
    }

    IEnumerator PlayFrames(bool forward)
    {
        if (curtainFrames == null || curtainFrames.Length == 0)
        {
            Debug.LogError("[Telón] ❌ curtainFrames está vacío");
            yield break;
        }

        float delay = 1f / fps;
        int start = forward ? 0 : curtainFrames.Length - 1;
        int end   = forward ? curtainFrames.Length - 1 : 0;
        int step  = forward ? 1 : -1;

        for (int i = start; forward ? i <= end : i >= end; i += step)
        {
            if (curtainFrames[i] != null)
            {
                curtainImage.sprite = curtainFrames[i];
                curtainImage.color = Color.white; // asegura que sea visible
            }
            else
            {
                Debug.LogWarning($"[Telón] frame {i} es null");
            }
            yield return new WaitForSeconds(delay);
        }
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
}