using UnityEngine;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public bool dialogueFinished => index >= lines.Count;
    public bool waitingForChoice = false;
    public List<ChoiceData> currentChoices = new List<ChoiceData>();
    public static DialogueSystem instance;

    [Header("Dialogue File")]
    public TextAsset dialogueFile; // Arrastra aquí el TXT

    private List<string> lines;
    private int index = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (dialogueFile == null)
        {
            Debug.LogError("❌ Dialogue file NOT assigned");
            return;
        }

        lines = new List<string>(dialogueFile.text.Split('\n'));
        index = 0;
    }

    // ▶ AVANZAR
    public string GetNextLine()
    {
        if (lines == null || index >= lines.Count)
            return null;

        string line = lines[index].TrimEnd();
        index++;

        if (string.IsNullOrWhiteSpace(line))
            return "";
            
        if (line == "@CHOICE")
        {
            ParseChoices();
            waitingForChoice = true;
            return null;
        }

        return line;
    }

    // ◀ RETROCEDER
    public string GetPreviousLine()
    {
        if (lines == null || lines.Count == 0)
            return null;

        // Retroceder 2 posiciones porque ya se incrementó antes
        index = Mathf.Clamp(index - 2, 0, lines.Count - 1);

        string line = lines[index].TrimEnd();
        index++;

        if (string.IsNullOrWhiteSpace(line))
            return "";

        return line;
    }
    void ParseChoices()
{
    currentChoices.Clear();

    while (index < lines.Count)
    {
        string line = lines[index].Trim();
        index++;

        if (line == "@END")
            break;

        string[] parts = line.Split('|');

        ChoiceData choice = new ChoiceData
        {
            id = parts[0].Trim(),
            text = parts[1].Trim(),
        };

        string stat = parts[2].Trim();

        if (stat.Contains("amor"))
            choice.amor = int.Parse(stat.Split('+')[1]);
        if (stat.Contains("reputacion"))
            choice.reputacion = int.Parse(stat.Split('+')[1]);
        if (stat.Contains("dinero"))
            choice.dinero = int.Parse(stat.Split('+')[1]);

        currentChoices.Add(choice);
    }
}
public void SkipChoiceBlock()
{
    while (index < lines.Count)
    {
        string line = lines[index].Trim();
        index++;

        if (line == "@END")
            break;
    }
}

}


