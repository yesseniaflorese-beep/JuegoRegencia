using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    [Header("Frame de corte")]
    [Tooltip("En este frame el telón cubre todo — aquí se activa la nueva escena")]
    public int sceneChangeFrame = 12;

    void Start()
    {
        chapterTitle.text = SceneController.instance.GetCurrentChapterTitle();
        chapterSubtitle.text = SceneController.instance.GetCurrentChapterSubtitle();

        SetTextAlpha(0f);
        StartCoroutine(NextChapterAfterDelay());
    }

    IEnumerator NextChapterAfterDelay()
    {
        // 1 — Telón se cierra (frames 0 → sceneChangeFrame)
        yield return StartCoroutine(PlayFrames(0, sceneChangeFrame));

        // 2 — Fade in texto
        yield return StartCoroutine(FadeText(0f, 1f, 0.4f));

        // 3 — Esperar
        yield return new WaitForSeconds(transitionTime);

        // 4 — Fade out texto
        yield return StartCoroutine(FadeText(1f, 0f, 0.3f));

        // 5 — Cargar siguiente escena en memoria sin activarla todavía
        string nextScene = SceneController.instance.GetNextSceneName();
        AsyncOperation load = SceneManager.LoadSceneAsync(nextScene);
        load.allowSceneActivation = false;

        // Esperar a que esté lista en memoria
        while (load.progress < 0.9f)
            yield return null;

        // Mover el Canvas del telón al root para que DontDestroyOnLoad funcione
        Transform root = curtainImage.canvas.transform;
        root.SetParent(null);
        DontDestroyOnLoad(root.gameObject);
        DontDestroyOnLoad(gameObject);

        // 6 — Activar la escena
        load.allowSceneActivation = true;

        // Esperar un frame para que la escena termine de inicializarse
        yield return null;
        yield return null;

        // 7 — Telón se abre (frames sceneChangeFrame → último)
        yield return StartCoroutine(PlayFrames(sceneChangeFrame, curtainFrames.Length - 1));

        // 8 — Destruir el Canvas del telón y este GameObject
        Destroy(curtainImage.canvas.gameObject);
        Destroy(gameObject);
    }

    IEnumerator PlayFrames(int from, int to)
    {
        if (curtainFrames == null || curtainFrames.Length == 0)
        {
            Debug.LogError("[Telón] curtainFrames está vacío");
            yield break;
        }

        float delay = 1f / fps;
        bool forward = to >= from;
        int step = forward ? 1 : -1;

        for (int i = from; forward ? i <= to : i >= to; i += step)
        {
            if (i >= 0 && i < curtainFrames.Length && curtainFrames[i] != null)
            {
                curtainImage.sprite = curtainFrames[i];
                curtainImage.color = Color.white;
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