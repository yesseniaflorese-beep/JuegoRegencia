using UnityEngine;
using TMPro;
using System.Collections;

public class ChapterTransition : MonoBehaviour
{
    public TMP_Text chapterTitle;
    public TMP_Text chapterSubtitle;

    public float transitionTime = 2f;

    void Start()
    {
        // Mostrar título del capítulo
        chapterTitle.text =
            SceneController.instance.GetCurrentChapterTitle();

        // Mostrar subtítulo
        chapterSubtitle.text =
            SceneController.instance.GetCurrentChapterSubtitle();

        // Esperar 2 segundos y cambiar de capítulo
        StartCoroutine(NextChapterAfterDelay());
    }

    IEnumerator NextChapterAfterDelay()
    {
        yield return new WaitForSeconds(transitionTime);

        SceneController.instance.LoadRealNextChapter();
    }
}