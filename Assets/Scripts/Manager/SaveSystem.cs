using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private void Awake()
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

        // =========================
        // Guardar escena actual
        // =========================
        PlayerPrefs.SetString(
            "SavedScene",
            SceneManager.GetActiveScene().name
        );

        // =========================
        // Guardar progreso narrativo
        // =========================
        PlayerPrefs.SetInt(
            "CurrentChapter",
            SceneController.instance.currentChapter
        );

        PlayerPrefs.SetInt(
            "CurrentDialogueIndex",
            DialogueSystem.instance.index
        );

        // =========================
        // Guardar stats principales
        // =========================
        PlayerPrefs.SetInt(
            "amor",
            GameManager.instance.amor
        );

        PlayerPrefs.SetInt(
            "reputacion",
            GameManager.instance.reputacion
        );

        PlayerPrefs.SetInt(
            "dinero",
            GameManager.instance.dinero
        );

        PlayerPrefs.SetInt(
            "ambicion",
            GameManager.instance.ambicion
        );

        // =========================
        // Guardar rutas románticas
        // =========================
        PlayerPrefs.SetInt(
            "theoPoints",
            GameManager.instance.theoPoints
        );

        PlayerPrefs.SetInt(
            "sebastianPoints",
            GameManager.instance.sebastianPoints
        );

        PlayerPrefs.SetInt(
            "routeTheo",
            GameManager.instance.routeTheo
        );

        PlayerPrefs.SetInt(
            "routeSebastian",
            GameManager.instance.routeSebastian
        );

        // =========================
        // Guardar decisiones
        // =========================
        PlayerPrefs.SetInt(
            "honesty",
            GameManager.instance.honesty
        );

        PlayerPrefs.SetInt(
            "lie",
            GameManager.instance.lie
        );

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

        // =========================
        // Restaurar capítulo actual
        // =========================
        SceneController.instance.currentChapter =
            PlayerPrefs.GetInt("CurrentChapter", 0);

        // =========================
        // Restaurar stats principales
        // =========================
        GameManager.instance.amor =
            PlayerPrefs.GetInt("amor", 0);

        GameManager.instance.reputacion =
            PlayerPrefs.GetInt("reputacion", 0);

        GameManager.instance.dinero =
            PlayerPrefs.GetInt("dinero", 0);

        GameManager.instance.ambicion =
            PlayerPrefs.GetInt("ambicion", 0);

        // =========================
        // Restaurar rutas románticas
        // =========================
        GameManager.instance.theoPoints =
            PlayerPrefs.GetInt("theoPoints", 0);

        GameManager.instance.sebastianPoints =
            PlayerPrefs.GetInt("sebastianPoints", 0);

        GameManager.instance.routeTheo =
            PlayerPrefs.GetInt("routeTheo", 0);

        GameManager.instance.routeSebastian =
            PlayerPrefs.GetInt("routeSebastian", 0);

        // =========================
        // Restaurar decisiones
        // =========================
        GameManager.instance.honesty =
            PlayerPrefs.GetInt("honesty", 0);

        GameManager.instance.lie =
            PlayerPrefs.GetInt("lie", 0);

        // =========================
        // Cargar escena guardada
        // =========================
        string savedScene =
            PlayerPrefs.GetString("SavedScene");

        SceneManager.LoadScene(savedScene);

        // Esperar 1 frame para restaurar UI,
        // fondos, personajes, música y diálogo
        StartCoroutine(RestoreAfterLoad());

        Debug.Log("✅ Partida cargada correctamente");
    }

    // =========================
    // BORRAR PARTIDA
    // =========================
    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("🗑 Partida eliminada");
    }

    // =========================
    // RESTAURAR DESPUÉS DE CARGAR
    // =========================
    private IEnumerator RestoreAfterLoad()
    {
        yield return null;

        // Restaurar fondos, sprites y música
        if (DialogueSystem.instance != null)
        {
            DialogueSystem.instance.RestoreVisualState();

            // Luego restaurar posición exacta
            DialogueSystem.instance.RestoreDialoguePosition();
        }

        // Restaurar historial
        if (DialogueHistoryManager.instance != null)
        {
            DialogueHistoryManager.instance.LoadHistory();
        }

        Debug.Log("✅ Progreso completo restaurado");
    }
}