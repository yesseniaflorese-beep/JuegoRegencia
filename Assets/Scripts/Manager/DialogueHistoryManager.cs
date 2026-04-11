using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHistoryManager : MonoBehaviour
{
    public static DialogueHistoryManager instance;

    public GameObject historyPanel;
    public Transform contentParent;
    public GameObject historyTextPrefab;

    private List<string> history = new List<string>();

    void Awake()
    {
        instance = this;
        historyPanel.SetActive(false);
    }

    public void AddDialogue(string speaker, string dialogue)
    {
        string line = speaker + ": " + dialogue;
        history.Add(line);
    }

    public void OpenHistory()
    {
        historyPanel.SetActive(true);
        ShowHistory();
    }

    public void CloseHistory()
    {
        historyPanel.SetActive(false);
    }

    void ShowHistory()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (string line in history)
        {
            GameObject text = Instantiate(historyTextPrefab, contentParent);
            text.GetComponent<TextMeshProUGUI>().text = line;
        }
    }
}