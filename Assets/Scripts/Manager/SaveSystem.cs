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
        if (GameManager.instance == null ||
            SceneController.instance == null ||
            DialogueSystem.instance == null)
        {
            Debug.LogError("❌ Faltan managers para guardar");
            return;
        }

        PlayerPrefs.SetString(
            "SavedScene",
            SceneManager.GetActiveScene().name
        );

        PlayerPrefs.SetInt(
            "CurrentChapter",
            SceneController.instance.currentChapter
        );

        // 🔥 Si estamos en un @CHOICE, guardar la línea de diálogo anterior
        // para que al restaurar el jugador llegue a las opciones naturalmente
        int indexToSave = DialogueSystem.instance.lastDisplayedIndex;
        if (DialogueSystem.instance.waitingForChoice)
        {
            indexToSave = DialogueSystem.instance.lastDisplayedIndex;
            Debug.Log("GUARDANDO INDEX: " + indexToSave + " (retrocediendo antes del @CHOICE)");
        }
        else
        {
            Debug.Log("GUARDANDO INDEX: " + indexToSave + " (línea mostrada)");
        }

        PlayerPrefs.SetInt("CurrentDialogueIndex", indexToSave);

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

        // Decisions
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

        // 🔥 BLOQUEO ANTI-SKIP (CLAVE)
        if (SceneController.instance != null)
            SceneController.instance.isLoadingGame = true;

        // Stats
        GameManager.instance.amor = PlayerPrefs.GetInt("amor", 0);
        GameManager.instance.reputacion = PlayerPrefs.GetInt("reputacion", 0);
        GameManager.instance.dinero = PlayerPrefs.GetInt("dinero", 0);
        GameManager.instance.ambicion = PlayerPrefs.GetInt("ambicion", 0);

        GameManager.instance.theoPoints = PlayerPrefs.GetInt("theoPoints", 0);
        GameManager.instance.sebastianPoints = PlayerPrefs.GetInt("sebastianPoints", 0);

        GameManager.instance.routeTheo = PlayerPrefs.GetInt("routeTheo", 0);
        GameManager.instance.routeSebastian = PlayerPrefs.GetInt("routeSebastian", 0);

        GameManager.instance.honesty = PlayerPrefs.GetInt("honesty", 0);
        GameManager.instance.lie = PlayerPrefs.GetInt("lie", 0);

        SceneController.instance.currentChapter =
            PlayerPrefs.GetInt("CurrentChapter", 0);

        SceneManager.sceneLoaded += OnSceneLoaded;

        string savedScene = PlayerPrefs.GetString("SavedScene");
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

    // =========================
    // RESTORE SEGURO
    // =========================
    private IEnumerator RestoreEverything()
    {
        // 🔥 esperar a que todo Awake/Start termine
        yield return new WaitForEndOfFrame();

        if (DialogueSystem.instance != null)
        {
            DialogueSystem.instance.RestoreVisualState();
            DialogueSystem.instance.RestoreDialoguePosition();

            // 🔥 desbloquear restauración
            DialogueSystem.instance.isRestoring = false;
        }

        if (DialogueHistoryManager.instance != null)
        {
            DialogueHistoryManager.instance.LoadHistory();
        }

        DialogueRunner runner =
            FindFirstObjectByType<DialogueRunner>();

        if (runner != null)
        {
            runner.RestoreDialogueUI();
        }

        // 🔥 liberar bloqueo de escena
        if (SceneController.instance != null)
            SceneController.instance.isLoadingGame = false;

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