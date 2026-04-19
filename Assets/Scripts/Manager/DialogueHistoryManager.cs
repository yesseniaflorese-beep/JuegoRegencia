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

    // ✅ Guardado correcto (NO string)
    private List<DialogueEntry> history = new List<DialogueEntry>();

    void Awake()
    {
        instance = this;
        historyPanel.SetActive(false);
    }

    // ============================
    // AGREGAR DIÁLOGO
    // ============================
    public void AddDialogue(string speaker, string dialogue)
    {
        history.Add(new DialogueEntry(speaker, dialogue));

        // 🔥 Si el panel está abierto → agregar en tiempo real
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

        DialogueHistoryItem historyItem = item.GetComponent<DialogueHistoryItem>();

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