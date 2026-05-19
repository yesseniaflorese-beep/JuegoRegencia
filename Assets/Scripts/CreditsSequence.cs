using UnityEngine;
using TMPro;
using System.Collections;

public class CreditsSequence : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text introText;
    public TMP_Text titleText;

    [Header("Credits")]
    public RectTransform creditsTransform;
    public float creditsSpeed = 40f;

    [Header("Times")]
    public float introDuration = 2f;
    public float titleDuration = 2f;
    public float fadeDuration = 1f;
    [Tooltip("Segundos en negro después de que los créditos salgan de pantalla")]
    public float endDelay = 1f;

    private bool startCredits = false;
    private bool goingToMenu = false;
    private float creditsHeight = 0f;

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    void Update()
    {
        if (!startCredits) return;

        creditsTransform.Translate(Vector3.up * creditsSpeed * Time.deltaTime);

        // Cuando el contenido salió completamente de pantalla
        if (!goingToMenu &&
            creditsTransform.anchoredPosition.y > creditsHeight + Screen.height)
        {
            goingToMenu = true;
            StartCoroutine(GoToMenuDelayed());
        }
    }

    IEnumerator GoToMenuDelayed()
    {
        yield return new WaitForSeconds(endDelay);
        SceneController.instance.LoadMenu();
    }

    IEnumerator PlaySequence()
    {
        // Ocultar todo al inicio
        introText.alpha = 0;
        titleText.alpha = 0;
        creditsTransform.gameObject.SetActive(false);

        // INTRO
        yield return StartCoroutine(FadeText(introText, 0, 1));
        yield return new WaitForSeconds(introDuration);
        yield return StartCoroutine(FadeText(introText, 1, 0));

        // TITULO
        yield return StartCoroutine(FadeText(titleText, 0, 1));
        yield return new WaitForSeconds(titleDuration);
        yield return StartCoroutine(FadeText(titleText, 1, 0));

        // CREDITOS
        creditsTransform.gameObject.SetActive(true);

        // Esperar un frame para que Unity calcule el layout
        yield return null;

        creditsHeight = creditsTransform.rect.height;
        startCredits = true;
    }

    IEnumerator FadeText(TMP_Text text, float start, float end)
    {
        float elapsed = 0;
        Color color = text.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            text.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        text.color = new Color(color.r, color.g, color.b, end);
    }
}