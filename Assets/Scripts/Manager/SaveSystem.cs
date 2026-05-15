using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // =========================
    // GUARDAR PARTIDA
    // =========================
    public void SaveGame()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("❌ No se encontró GameManager");
            return;
        }

        if (SceneController.instance == null)
        {
            Debug.LogError("❌ No se encontró SceneController");
            return;
        }

        if (DialogueSystem.instance == null)
        {
            Debug.LogError("❌ No se encontró DialogueSystem");
            return;
        }

        // Escena actual
        PlayerPrefs.SetString(
            "SavedScene",
            SceneManager.GetActiveScene().name
        );

        // Capítulo actual
        PlayerPrefs.SetInt(
            "CurrentChapter",
            SceneController.instance.currentChapter
        );

        // Índice diálogo
        PlayerPrefs.SetInt(
            "CurrentDialogueIndex",
            Mathf.Max(DialogueSystem.instance.index - 1, 0)
        );
        Debug.Log("GUARDANDO INDEX: " + DialogueSystem.instance.index);

        // Stats
        PlayerPrefs.SetInt("amor", GameManager.instance.amor);
        PlayerPrefs.SetInt("reputacion", GameManager.instance.reputacion);
        PlayerPrefs.SetInt("dinero", GameManager.instance.dinero);
        PlayerPrefs.SetInt("ambicion", GameManager.instance.ambicion);

        // Routes
        PlayerPrefs.SetInt("theoPoints", GameManager.instance.theoPoints);
        PlayerPrefs.SetInt("sebastianPoints", GameManager.instance.sebastianPoints);

        PlayerPrefs.SetInt("routeTheo", GameManager.instance.routeTheo);
        PlayerPrefs.SetInt("routeSebastian", GameManager.instance.routeSebastian);

        // Decisiones
        PlayerPrefs.SetInt("honesty", GameManager.instance.honesty);
        PlayerPrefs.SetInt("lie", GameManager.instance.lie);

        PlayerPrefs.Save();

        Debug.Log("✅ Partida guardada correctamente");
    }

    // =========================
    // CARGAR PARTIDA
    // =========================
    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SavedScene"))
        {
            Debug.LogWarning("⚠ No hay partida guardada");
            return;
        }

        // Restaurar capítulo
        SceneController.instance.currentChapter =
            PlayerPrefs.GetInt("CurrentChapter", 0);

        // Restaurar stats
        GameManager.instance.amor =
            PlayerPrefs.GetInt("amor", 0);

        GameManager.instance.reputacion =
            PlayerPrefs.GetInt("reputacion", 0);

        GameManager.instance.dinero =
            PlayerPrefs.GetInt("dinero", 0);

        GameManager.instance.ambicion =
            PlayerPrefs.GetInt("ambicion", 0);

        // Restaurar rutas
        GameManager.instance.theoPoints =
            PlayerPrefs.GetInt("theoPoints", 0);

        GameManager.instance.sebastianPoints =
            PlayerPrefs.GetInt("sebastianPoints", 0);

        GameManager.instance.routeTheo =
            PlayerPrefs.GetInt("routeTheo", 0);

        GameManager.instance.routeSebastian =
            PlayerPrefs.GetInt("routeSebastian", 0);

        // Restaurar decisiones
        GameManager.instance.honesty =
            PlayerPrefs.GetInt("honesty", 0);

        GameManager.instance.lie =
            PlayerPrefs.GetInt("lie", 0);

        // Escuchar carga de escena
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Cargar escena
        string savedScene =
            PlayerPrefs.GetString("SavedScene");

        SceneManager.LoadScene(savedScene);

        Debug.Log("✅ Partida cargada correctamente");
    }

    // =========================
    // ESCENA CARGADA
    // =========================
private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    StartCoroutine(RestoreEverything());

    SceneManager.sceneLoaded -= OnSceneLoaded;
}

private IEnumerator RestoreEverything()
{
    // Esperar un frame
    yield return null;

    // Restaurar diálogo
    if (DialogueSystem.instance != null)
    {
        DialogueSystem.instance.RestoreVisualState();
        DialogueSystem.instance.RestoreDialoguePosition();
    }

    // Restaurar historial
    if (DialogueHistoryManager.instance != null)
    {
        DialogueHistoryManager.instance.LoadHistory();
    }

    // Restaurar UI del diálogo
    DialogueRunner runner =
        FindFirstObjectByType<DialogueRunner>();

    if (runner != null)
    {
        runner.RestoreDialogueUI();
    }

    Debug.Log("✅ Progreso completo restaurado");
}
    // =========================
    // BORRAR SAVE
    // =========================
    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("🗑 Partida eliminada");
    }
}