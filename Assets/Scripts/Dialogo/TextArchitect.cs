using UnityEngine;
using System.Collections;
using TMPro;

public class TextArchitect
{
    public bool isBuilding => buildProcess != null;
    private TMP_Text tmpro;
    private MonoBehaviour runner;

    private Coroutine buildProcess;

    // ================= DATA =================

    public string preText { get; private set; } = "";
    public string targetText { get; private set; } = "";

    public enum BuildMethod { instant, typewriter, fade }
    public BuildMethod buildMethod = BuildMethod.typewriter;

    public float speed = 1f;
    public bool rapido = false;

    private int charactersPerCycle = 1;

    // ================= CONSTRUCTOR =================

    public TextArchitect(TMP_Text text, MonoBehaviour coroutineRunner)
    {
        tmpro = text;
        runner = coroutineRunner;
    }

    // ================= PUBLIC API =================

    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();
        buildProcess = runner.StartCoroutine(Building());
        return buildProcess;
    }

    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();
        buildProcess = runner.StartCoroutine(Building());
        return buildProcess;
    }

    public void Stop()
    {
        if (buildProcess != null)
        {
            runner.StopCoroutine(buildProcess);
            buildProcess = null;
        }
    }

    // ================= CORE =================

    IEnumerator Building()
    {
        Prepare();

        switch (buildMethod)
        {
            case BuildMethod.instant:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                OnComplete();
                break;

            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;

            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }
    }

    private void OnComplete()
    {
        buildProcess = null;
        rapido = false;
    }

    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                break;
        }

        Stop();
        OnComplete();
    }

    // ================= PREPARE =================

    private void Prepare()
    {
        Prepare_Typewriter();
    }

    private void Prepare_Typewriter()
    {
        tmpro.text = preText;
        tmpro.maxVisibleCharacters = preText.Length;

        if (!string.IsNullOrEmpty(preText))
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }

    private void Prepare_Fade()
    {
        tmpro.alpha = 0;
    }

    // ================= BUILD =================

    private IEnumerator Build_Typewriter()
    {
        int totalChars = tmpro.textInfo.characterCount;

        while (tmpro.maxVisibleCharacters < totalChars)
        {
            int step = rapido ? charactersPerCycle * 5 : charactersPerCycle;
            tmpro.maxVisibleCharacters += step;

            yield return new WaitForSeconds(0.015f / speed);
        }

        tmpro.maxVisibleCharacters = totalChars;
        OnComplete();
    }

    private IEnumerator Build_Fade()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            tmpro.alpha = t;
            yield return null;
        }

        tmpro.alpha = 1f;
        OnComplete();
    }
}
