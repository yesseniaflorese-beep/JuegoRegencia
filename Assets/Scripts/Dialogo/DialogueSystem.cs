using UnityEngine;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    public TextAsset dialogueFile;

    public bool waitingForChoice = false;
    public List<ChoiceData> currentChoices = new List<ChoiceData>();

    private List<string> lines;
    private Dictionary<string, int> labels = new Dictionary<string, int>();
    private int index = 0;

    public bool dialogueFinished => lines != null && index >= lines.Count;

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

        // Registrar labels
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].StartsWith("@LABEL"))
            {
                string label = lines[i].Replace("@LABEL", "").Trim();
                labels[label] = i;
            }
        }
    }

    // =============================
    // ▶ SIGUIENTE LÍNEA
    // =============================
    public string GetNextLine()
    {
        if (lines == null || index >= lines.Count)
            return null;

        string line = lines[index].Trim();
        index++;

        if (string.IsNullOrWhiteSpace(line))
            return "";

        // SPRITE
        if (line.StartsWith("@SPRITE"))
        {
            string[] parts = line.Split(' ');
            if (parts.Length >= 4)
                SpriteManager.instance.ShowSprite(parts[1], parts[2], parts[3]);

            return GetNextLine();
        }

        // BACKGROUND
        if (line.StartsWith("@BG"))
        {
            string bgName = line.Replace("@BG", "").Trim();
            BackgroundManager.instance.ChangeBackground(bgName);
            return GetNextLine();
        }

        // MUSIC
        if (line.StartsWith("@MUSIC"))
        {
            string musicName = line.Replace("@MUSIC", "").Trim();
            AudioManager.instance.PlayMusic(musicName);
            return GetNextLine();
        }

        // SFX
        if (line.StartsWith("@SFX"))
        {
            string sfxName = line.Replace("@SFX", "").Trim();
            AudioManager.instance.PlaySFX(sfxName);
            return GetNextLine();
        }

        // MINIGAME
        if (line.StartsWith("@MINIGAME"))
        {
            string minigameName = line.Replace("@MINIGAME", "").Trim();

            MinigameManager.instance.StartMinigame(minigameName);

            return null;
        }

        // GOTO
        if (line.StartsWith("@GOTO"))
        {
            string label = line.Replace("@GOTO", "").Trim();

            if (labels.ContainsKey(label))
                index = labels[label] + 1;

            return GetNextLine();
        }

        // IF
        if (line.StartsWith("@IF"))
        {
            if (!EvaluateCondition(line.Replace("@IF", "").Trim()))
            {
                SkipUntilEndIf();
            }

            return GetNextLine();
        }

        // CHOICE
        if (line == "@CHOICE")
        {
            ParseChoices();
            waitingForChoice = true;
            return null;
        }

        return line;
    }

    // =============================
    // ◀ RETROCEDER
    // =============================
    public string GetPreviousLine()
    {
        if (lines == null || lines.Count == 0)
            return null;

        index = Mathf.Clamp(index - 2, 0, lines.Count - 1);

        string line = lines[index].Trim();
        index++;

        if (string.IsNullOrWhiteSpace(line))
            return "";

        return line;
    }

    // =============================
    // PARSEAR DECISIONES
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

            string stat = parts[2].Trim();
            choice.statName = stat.Split('+')[0];
            choice.statValue = int.Parse(stat.Split('+')[1]);

            if (parts.Length >= 4)
                choice.gotoLabel = parts[3].Trim();

            currentChoices.Add(choice);
        }
    }

    // =============================
    // CUANDO EL JUGADOR ELIGE
    // =============================
    public void SelectChoice(ChoiceData choice)
    {
        GameManager.instance.AddStat(choice.statName, choice.statValue);

        if (!string.IsNullOrEmpty(choice.gotoLabel))
        {
            if (labels.ContainsKey(choice.gotoLabel))
            {
                index = labels[choice.gotoLabel] + 1;
            }
        }

        waitingForChoice = false;
    }

    // =============================
    // EVALUAR CONDICIONES
    // =============================
    bool EvaluateCondition(string condition)
    {
        string[] parts = condition.Split(' ');

        if (parts.Length < 3)
            return false;

        string stat = parts[0];
        string op = parts[1];
        int value = int.Parse(parts[2]);

        int currentValue = GameManager.instance.GetStat(stat);

        switch (op)
        {
            case ">=": return currentValue >= value;
            case "<=": return currentValue <= value;
            case ">": return currentValue > value;
            case "<": return currentValue < value;
            case "==": return currentValue == value;
        }

        return false;
    }

    // =============================
    // SALTAR IF
    // =============================
    void SkipUntilEndIf()
    {
        while (index < lines.Count)
        {
            if (lines[index].Trim() == "@ENDIF")
            {
                index++;
                break;
            }
            index++;
        }
    }
}