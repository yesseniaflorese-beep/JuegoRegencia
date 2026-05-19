using UnityEngine;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    public TextAsset dialogueFile;

    public bool waitingForChoice = false;
    public List<ChoiceData> currentChoices = new List<ChoiceData>();

    public List<string> lines;
    private Dictionary<string, int> labels = new Dictionary<string, int>();

    public int index = 0;
    public int lastDisplayedIndex = 0; // índice de la última línea mostrada al jugador

    public bool isRestoring = false;

    public bool dialogueFinished => lines != null && index >= lines.Count;

    // 🔥 BLOQUEO GLOBAL (minijuego, cutscene, etc.)
    public bool isBlocked =>
        MinigameManager.instance != null &&
        MinigameManager.instance.isMinigameActive;

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
            Debug.LogError("Dialogue file not assigned!");
            return;
        }

        lines = new List<string>(dialogueFile.text.Split('\n'));

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].StartsWith("@LABEL"))
            {
                string label = lines[i].Replace("@LABEL", "").Trim();
                labels[label] = i;
            }
        }
    }

    void Update()
    {
        if (isRestoring) return;
        if (isBlocked) return;
        if (waitingForChoice) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetNextLine();
        }
    }

    // =============================
    // NEXT LINE
    // =============================
    public string GetNextLine()
    {
        if (isRestoring) return null;

        if (lines == null || index >= lines.Count)
            return null;

        string line = lines[index].Trim();

        while (
            string.IsNullOrEmpty(line) ||
            line.StartsWith("//") ||
            line.StartsWith("/*") ||
            line.StartsWith("*") ||
            line.StartsWith("*/")
        )
        {
            index++;

            if (index >= lines.Count)
                return null;

            line = lines[index].Trim();
        }

        index++;

        if (string.IsNullOrWhiteSpace(line))
            return "";

        if (line.StartsWith("@LABEL"))
            return GetNextLine();

        if (line.StartsWith("@ENDIF"))
            return GetNextLine();

        if (line.StartsWith("@SPRITE"))
        {
            string[] parts = line.Split(' ');
            if (parts.Length >= 4)
                SpriteManager.instance.ShowSprite(parts[1], parts[2], parts[3]);

            return GetNextLine();
        }

        if (line.StartsWith("@BG"))
        {
            BackgroundManager.instance.ChangeBackground(line.Replace("@BG", "").Trim());
            return GetNextLine();
        }

        if (line.StartsWith("@MUSIC"))
        {
            AudioManager.instance.PlayMusic(line.Replace("@MUSIC", "").Trim());
            return GetNextLine();
        }

        if (line.StartsWith("@SFX"))
        {
            AudioManager.instance.PlaySFX(line.Replace("@SFX", "").Trim());
            return GetNextLine();
        }

        if (line.StartsWith("@MINIGAME"))
        {
            MinigameManager.instance.StartMinigame(line.Replace("@MINIGAME", "").Trim());
            return null;
        }

        if (line.StartsWith("@GOTO"))
        {
            string label = line.Replace("@GOTO", "").Trim();

            if (labels.ContainsKey(label))
                index = labels[label] + 1;

            return GetNextLine();
        }

        if (line.StartsWith("@IF"))
        {
            if (!EvaluateCondition(line.Replace("@IF", "").Trim()))
                SkipUntilEndIf();

            return GetNextLine();
        }

        if (line == "@CHOICE")
        {
            ParseChoices();
            waitingForChoice = true;
            return null;
        }

        // Guardar el índice de esta línea como la última mostrada al jugador
        lastDisplayedIndex = index - 1;

        return line;
    }

    // =============================
    // RESTORE POSITION
    // =============================
    public void RestoreDialoguePosition()
    {
        if (lines == null || lines.Count == 0)
            return;

        isRestoring = true;

        index = PlayerPrefs.GetInt("CurrentDialogueIndex", 0);
        index = Mathf.Clamp(index, 0, lines.Count - 1);

        isRestoring = false;
    }

    // =============================
    // RESTORE VISUAL STATE
    // =============================
    public void RestoreVisualState()
    {
        if (lines == null || lines.Count == 0)
            return;

        int savedIndex = PlayerPrefs.GetInt("CurrentDialogueIndex", 0);
        savedIndex = Mathf.Clamp(savedIndex, 0, lines.Count);

        for (int i = 0; i < savedIndex; i++)
        {
            string line = lines[i].Trim();

            if (line.StartsWith("@BG"))
                BackgroundManager.instance.ChangeBackground(line.Replace("@BG", "").Trim());

            else if (line.StartsWith("@SPRITE"))
            {
                string[] parts = line.Split(' ');
                if (parts.Length >= 4)
                    SpriteManager.instance.ShowSprite(parts[1], parts[2], parts[3]);
            }

            else if (line.StartsWith("@MUSIC"))
                AudioManager.instance.PlayMusic(line.Replace("@MUSIC", "").Trim());

            else if (line.StartsWith("@SFX"))
                AudioManager.instance.PlaySFX(line.Replace("@SFX", "").Trim());
        }
    }

    // =============================
    // CHOICES
    // =============================
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

            if (parts.Length < 3)
                continue;

            ChoiceData choice = new ChoiceData();

            choice.id = parts[0].Trim();
            choice.text = parts[1].Trim();

            string statBlock = parts[2].Trim();
            string[] stats = statBlock.Split(' ');

            foreach (string s in stats)
            {
                if (s.Contains("+"))
                {
                    string[] statParts = s.Split('+');
                    choice.stats[statParts[0]] = int.Parse(statParts[1]);
                }
                else if (s.Contains("-"))
                {
                    string[] statParts = s.Split('-');
                    choice.stats[statParts[0]] = -int.Parse(statParts[1]);
                }
            }

            if (parts.Length >= 4)
                choice.gotoLabel = parts[3].Trim();

            currentChoices.Add(choice);
        }
    }

    public void SelectChoice(ChoiceData choice)
    {
        foreach (var stat in choice.stats)
        {
            GameManager.instance.AddStat(stat.Key, stat.Value);
        }

        if (!string.IsNullOrEmpty(choice.gotoLabel) &&
            labels.ContainsKey(choice.gotoLabel))
        {
            index = labels[choice.gotoLabel] + 1;
        }

        waitingForChoice = false;
    }

    // =============================
    // CONDITIONS
    // =============================
    bool EvaluateCondition(string condition)
    {
        string[] parts = condition.Split(' ');

        if (parts.Length < 3)
            return false;

        string leftStat = parts[0];
        string op = parts[1];
        string rightValue = parts[2];

        int left = GameManager.instance.GetStat(leftStat);
        int right = int.TryParse(rightValue, out int n)
            ? n
            : GameManager.instance.GetStat(rightValue);

        switch (op)
        {
            case ">=": return left >= right;
            case "<=": return left <= right;
            case ">": return left > right;
            case "<": return left < right;
            case "==": return left == right;
        }

        return false;
    }
public string GetPreviousLine()
{
    if (lines == null || lines.Count == 0)
        return null;

    if (isRestoring) return null;

    index = Mathf.Clamp(index - 2, 0, lines.Count - 1);

    string line = lines[index].Trim();
    index++;

    if (string.IsNullOrWhiteSpace(line))
        return "";

    return line;
}

    void SkipUntilEndIf()
    {
        int depth = 1; // ya estamos dentro de un @IF que falló

        while (index < lines.Count)
        {
            string l = lines[index].Trim();

            if (l.StartsWith("@IF"))
                depth++;           // @IF anidado: sube un nivel
            else if (l == "@ENDIF")
            {
                depth--;           // cerramos un nivel
                if (depth == 0)
                {
                    index++;       // consumir el @ENDIF y salir
                    break;
                }
            }

            index++;
        }
    }
}