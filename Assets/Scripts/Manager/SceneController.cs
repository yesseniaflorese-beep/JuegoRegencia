using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [Header("Capítulos")]
    public string capitulo1;

    [Header("Capítulo 2 por ruta")]
    public string capitulo2_Hombre;
    public string capitulo2_Mujer;

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
            DontDestroyOnLoad(gameObject); // 🔥 Persiste entre escenas
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
        LoadSceneSafe(capitulo1);
    }

    // ================================
    // SIGUIENTE CAPÍTULO
    // ================================
    public void LoadNextChapter()
    {
        currentChapter++;

        switch (currentChapter)
        {
            case 1:
                LoadCapitulo2PorRuta();
                break;

            default:
                Debug.Log("🏁 Fin del juego");
                LoadMenu();
                break;
        }
    }

    private void LoadCapitulo2PorRuta()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("❌ GameManager no encontrado");
            return;
        }

        string nextScene = "";

        if (GameManager.instance.selectedRoute == GameManager.PlayerRoute.Hombre)
            nextScene = capitulo2_Hombre;
        else
            nextScene = capitulo2_Mujer;

        LoadSceneSafe(nextScene);
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