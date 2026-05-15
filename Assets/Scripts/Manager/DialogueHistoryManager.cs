using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHistoryManager : MonoBehaviour
{
    public static DialogueHistoryManager instance;

    [Header("UI")]
    public GameObject historyPanel;
    public Transform contentParent;
    public GameObject historyTextPrefab;
    public ScrollRect scrollRect;

    public bool isRestoring = false;

    // Guardado del historial
    private List<DialogueEntry> history = new List<DialogueEntry>();

 void Awake()
{
    if (instance != null && instance != this)
    {
        Destroy(gameObject);
        return;
    }

    instance = this;

    DontDestroyOnLoad(gameObject);

    historyPanel.SetActive(false);

    LoadHistory();
}

    // ============================
    // AGREGAR DIÁLOGO
    // ============================
    public void AddDialogue(string speaker, string dialogue)
{
    history.Add(new DialogueEntry(speaker, dialogue));

    if (!isRestoring)
    {
        SaveHistory();
    }

    if (historyPanel.activeSelf)
    {
        CreateItem(speaker, dialogue);
        UpdateScroll();
    }
}

    // ============================
    // ABRIR HISTORIAL
    // ============================
    public void OpenHistory()
    {
        historyPanel.SetActive(true);
        ShowHistory();
    }

    public void CloseHistory()
    {
        historyPanel.SetActive(false);
    }

    // ============================
    // MOSTRAR TODO
    // ============================
    void ShowHistory()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (DialogueEntry entry in history)
        {
            CreateItem(entry.speaker, entry.dialogue);
        }

        UpdateScroll();
    }

    // ============================
    // CREAR ITEM VISUAL
    // ============================
    void CreateItem(string speaker, string dialogue)
    {
        GameObject item = Instantiate(historyTextPrefab, contentParent);

        DialogueHistoryItem historyItem =
            item.GetComponent<DialogueHistoryItem>();

        if (historyItem != null)
        {
            historyItem.Setup(speaker, dialogue);
        }
        else
        {
            Debug.LogError("❌ El prefab no tiene DialogueHistoryItem");
        }
    }

    // ============================
    // SCROLL AUTOMÁTICO
    // ============================
    void UpdateScroll()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    // ============================
    // GUARDAR HISTORIAL
    // ============================
    private void SaveHistory()
    {
        List<string> saveLines = new List<string>();

        foreach (DialogueEntry entry in history)
        {
            string line = entry.speaker + "||" + entry.dialogue;
            saveLines.Add(line);
        }

        string finalSave = string.Join("##", saveLines);

        PlayerPrefs.SetString("DialogueHistory", finalSave);
        PlayerPrefs.Save();

        Debug.Log("✅ Historial guardado");
    }

    // ============================
    // CARGAR HISTORIAL
    // ============================
    public void LoadHistory()
{
    isRestoring = true;

    history.Clear();

    if (!PlayerPrefs.HasKey("DialogueHistory"))
    {
        isRestoring = false;
        return;
    }

    string saved = PlayerPrefs.GetString("DialogueHistory");

    if (string.IsNullOrEmpty(saved))
    {
        isRestoring = false;
        return;
    }

    string[] entries = saved.Split(
        new string[] { "##" },
        System.StringSplitOptions.None
    );

    foreach (string entry in entries)
    {
        if (string.IsNullOrWhiteSpace(entry))
            continue;

        string[] parts = entry.Split(
            new string[] { "||" },
            System.StringSplitOptions.None
        );

        if (parts.Length < 2)
            continue;

        history.Add(new DialogueEntry(parts[0], parts[1]));
    }

    isRestoring = false;

    Debug.Log("✅ Historial cargado");
}

    // ============================
    // BORRAR HISTORIAL
    // ============================
    public void ClearHistory()
    {
        history.Clear();

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        PlayerPrefs.DeleteKey("DialogueHistory");
        PlayerPrefs.Save();

        Debug.Log("🗑 Historial eliminado");
    }
}

// ============================
// DATA STRUCT
// ============================
[System.Serializable]
public class DialogueEntry
{
    public string speaker;
    public string dialogue;

    public DialogueEntry(string s, string d)
    {
        speaker = s;
        dialogue = d;
    }
}