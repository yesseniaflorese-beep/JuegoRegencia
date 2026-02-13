using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // [Header("Capítulos")]
    // public string[] chapters;

    [Header("Capítulos")]
    public string capitulo1;

    [Header("Capítulo 2 por ruta")]
    public string capitulo2_Hombre;
    public string capitulo2_Mujer;

    public int currentChapter = 0;
    // public int currentChapter = 0;

    [Header("Escenas fijas")]
    public string menuScene = "MenuInicial";
    public string instruccionesScene = "Instrucciones";
    public string seleccionScene = "SeleccionPersonaje";

    // ▶ MENÚ
    public void LoadMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    public void LoadInstrucciones()
    {
        SceneManager.LoadScene(instruccionesScene);
    }

    public void LoadSeleccionPersonaje()
    {
        SceneManager.LoadScene(seleccionScene);
    }
    public void StartGame()
    {
        currentChapter = 0;
        SceneManager.LoadScene(capitulo1);
    }

    public void LoadCurrentChapter()
    {
        currentChapter = 0;
        SceneManager.LoadScene(capitulo1);
    }

    public void LoadNextChapter()
    {
        if (currentChapter == 0)
    {
        currentChapter = 1;

        string nextScene = "";

        if (GameManager.instance.selectedRoute == GameManager.PlayerRoute.Hombre)
            nextScene = capitulo2_Hombre;
        else
            nextScene = capitulo2_Mujer;

        if (string.IsNullOrEmpty(nextScene))
        {
            Debug.LogError("❌ Capítulo 2 no asignado en SceneController");
            return;
        }

        SceneManager.LoadScene(nextScene);
        return;
    }

    Debug.Log("🏁 Fin del juego");
    LoadMenu();
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}


