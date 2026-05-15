using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChapterTransition : MonoBehaviour
{
    public TMP_Text texto;

    [TextArea]
    public string mensaje;

    public float tiempoEspera = 4f;

    void Start()
    {
        texto.text = mensaje;

        Invoke(nameof(LoadNextChapter), tiempoEspera);
    }

    void LoadNextChapter()
    {
        SceneController.instance.LoadRealNextChapter();
    }
}