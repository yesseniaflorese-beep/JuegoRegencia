using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [Header("Capítulos en orden")]
    public List<string> capitulos;

    private int currentChapter = 0;

    [Header("Escenas fijas")]
    public string menuScene = "MenuInicial";
    public string instruccionesScene = "Instrucciones";
    public string seleccionScene = "SeleccionPersonaje";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ================================
    // ESCENAS FIJAS
    // ================================
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

    // ================================
    // INICIAR JUEGO
    // ================================
    public void StartGame()
    {
        currentChapter = 0;

        if (capitulos.Count > 0)
        {
            LoadSceneSafe(capitulos[currentChapter]);
        }
        else
        {
            Debug.LogError("❌ No hay capítulos asignados");
        }
    }

    // ================================
    // SIGUIENTE CAPÍTULO
    // ================================
    public void LoadNextChapter()
    {
        currentChapter++;

        if (currentChapter < capitulos.Count)
        {
            LoadSceneSafe(capitulos[currentChapter]);
        }
        else
        {
            Debug.Log("🏁 Fin del juego");
            LoadMenu();
        }
    }

    // ================================
    // MÉTODO SEGURO
    // ================================
    private void LoadSceneSafe(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("❌ Nombre de escena vacío");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByName(string sceneName)
    {
        LoadSceneSafe(sceneName);
    }
}