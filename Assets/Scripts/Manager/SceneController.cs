using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ChapterData
{
    public string sceneName;

    [Header("Texto transición")]
    public string titulo;
    public string subtitulo;
}

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public bool isLoadingGame = false;

    [Header("Capítulos en orden")]
    public List<ChapterData> capitulos;

    [Header("Capítulo actual")]
    public int currentChapter = 0;

    [Header("Escenas fijas")]
    public string menuScene = "MenuInicial";
    public string instruccionesScene = "Instrucciones";
    public string seleccionScene = "SeleccionPersonaje";
    public string creditsScene = "CreditsScene";

    [Header("Escena transición")]
    public string transitionScene = "TransitionScene";

    // capítulo pendiente
    private string pendingChapter;

    // ==================================================
    // AWAKE
    // ==================================================
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // ==================================================
    // ESCENAS FIJAS
    // ==================================================
    public void LoadCredits()
    {
        LoadSceneSafe(creditsScene);
    }

    public void LoadMenu()
    {
        LoadSceneSafe(menuScene);
    }

    public void LoadInstrucciones()
    {
        LoadSceneSafe(instruccionesScene);
    }

    public void LoadSeleccionPersonaje()
    {
        LoadSceneSafe(seleccionScene);
    }

    // ==================================================
    // NUEVA PARTIDA
    // ==================================================
    public void StartGame()
    {
        currentChapter = 0;

        // ⚠ No persistimos CurrentChapter aquí; solo SaveGame() lo hace.

        if (capitulos.Count > 0)
        {
            LoadSceneSafe(
                capitulos[currentChapter].sceneName
            );
        }
        else
        {
            Debug.LogError("❌ No hay capítulos asignados");
        }
    }

    [Header("Transición")]
    public float transitionDelay = 0.5f;

    // ==================================================
    // SIGUIENTE CAPÍTULO
    // ==================================================
    public void LoadNextChapter()
{
    if (isLoadingGame)
        return;

    currentChapter++;

    // ⚠ NO guardamos CurrentChapter aquí — solo SaveGame() lo persiste.
    // Guardar aquí causaba que avanzar sin guardar corrompiera el save.

    if (currentChapter < capitulos.Count)
    {
        pendingChapter = capitulos[currentChapter].sceneName;
        StartCoroutine(LoadNextChapterDelayed());
    }
    else
    {
        StartCoroutine(LoadMenuDelayed());
    }
}

    IEnumerator LoadNextChapterDelayed()
    {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(transitionScene);
    }

    IEnumerator LoadMenuDelayed()
    {
        yield return new WaitForSeconds(transitionDelay);
        LoadSceneSafe(creditsScene);
    }

    // ==================================================
    // CARGAR CAPÍTULO REAL
    // ==================================================
    public void LoadRealNextChapter()
    {
        if (!string.IsNullOrEmpty(pendingChapter))
        {
            SceneManager.LoadScene(pendingChapter);
        }
    }

    // ==================================================
    // DATOS TRANSICIÓN
    // ==================================================
    public string GetCurrentChapterTitle()
    {
        if (
            currentChapter >= 0 &&
            currentChapter < capitulos.Count
        )
        {
            return capitulos[currentChapter].titulo;
        }

        return "";
    }

    public string GetCurrentChapterSubtitle()
    {
        if (
            currentChapter >= 0 &&
            currentChapter < capitulos.Count
        )
        {
            return capitulos[currentChapter].subtitulo;
        }

        return "";
    }

    // ==================================================
    // CARGAR ESCENA SEGURA
    // ==================================================
    private void LoadSceneSafe(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("❌ Nombre de escena vacío");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    // ==================================================
    // CARGAR POR NOMBRE
    // ==================================================
    public void LoadSceneByName(string sceneName)
    {
        LoadSceneSafe(sceneName);
    }
}