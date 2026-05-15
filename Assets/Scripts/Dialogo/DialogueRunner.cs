using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueRunner : MonoBehaviour
{
    public GameObject choicesPanel;
    public ChoiceButton[] choiceButtons;
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    DialogueSystem ds;
    TextArchitect architect;

    void Start()
    {
        ds = DialogueSystem.instance;
        architect = new TextArchitect(dialogueText, this);
    }
    void Awake()
{
    if (FindObjectsByType<DialogueRunner>(FindObjectsSortMode.None).Length > 1)
    {
        Destroy(gameObject);
        return;
    }
}

    public void RestoreDialogueUI()
    {
        if (ds == null)
            ds = DialogueSystem.instance;

        if (ds == null || ds.dialogueFile == null)
            return;

        string[] lines = ds.dialogueFile.text.Split('\n');

        int savedIndex = ds.index;

        if (savedIndex < 0 || savedIndex >= lines.Length)
            return;

        string line = lines[savedIndex].Trim();

        // Saltar comandos inválidos
        while (savedIndex < lines.Length &&
               (string.IsNullOrWhiteSpace(line) || line.StartsWith("@")))
        {
            savedIndex++;

            if (savedIndex >= lines.Length)
                return;

            line = lines[savedIndex].Trim();
        }

        ProcessLine(line);

        // 🔥 IMPORTANTE: sincronización correcta
        ds.index = savedIndex + 1;
    }

    void Update()
{
    if (SceneController.instance != null &&
        SceneController.instance.isLoadingGame)
        return;

    if (Keyboard.current == null)
        return;

    if (Keyboard.current.spaceKey.wasPressedThisFrame)
    {
        AdvanceDialogue();
    }
}
    

    // ▶ AVANZAR
    public void AdvanceDialogue()
    {
        // 🔥 BLOQUEO DURANTE RESTORE
        if (SceneController.instance != null &&
            SceneController.instance.isLoadingGame)
            return;

        // ⏩ texto en construcción
        if (architect.isBuilding)
        {
            if (!architect.rapido)
                architect.rapido = true;
            else
                architect.ForceComplete();

            return;
        }

        // 🟡 choices
        if (ds.waitingForChoice)
        {
            ShowChoices();
            return;
        }

        string line = ds.GetNextLine();

        if (line == null)
        {
            // 🟡 si hay choice pendiente, no terminar
            if (ds.waitingForChoice)
            {
                ShowChoices();
                return;
            }

            // 🏁 FIN DE CAPÍTULO (CONTROLADO)
            if (ds.dialogueFinished &&
                SceneController.instance != null &&
                !SceneController.instance.isLoadingGame &&
                !ds.isRestoring)
            {
                Debug.Log("📕 Fin del capítulo");

                enabled = false;

                SceneController sc =
                    FindFirstObjectByType<SceneController>();

                if (sc != null)
                    sc.LoadNextChapter();
            }

            return;
        }

        if (line == "")
            return;

        ProcessLine(line);
    }

    // ◀ RETROCEDER
    public void BackDialogue()
    {
        if (SceneController.instance != null &&
            SceneController.instance.isLoadingGame)
            return;

        if (architect.isBuilding)
        {
            architect.ForceComplete();
            return;
        }

        string line = ds.GetPreviousLine();

        if (string.IsNullOrEmpty(line))
            return;

        ProcessLine(line);
    }

    // 🔎 PROCESA TEXTO
    void ProcessLine(string line)
    {
        if (line.Contains(":"))
        {
            string[] split = line.Split(new char[] { ':' }, 2);

            string speaker = split[0].Trim();
            string dialogue = split[1].Trim();

            if (nameText != null)
                nameText.text = speaker;

            architect.Build(dialogue);

            if (DialogueHistoryManager.instance != null)
                DialogueHistoryManager.instance.AddDialogue(speaker, dialogue);
        }
        else
        {
            if (nameText != null)
                nameText.text = "";

            architect.Build(line);

            if (DialogueHistoryManager.instance != null)
                DialogueHistoryManager.instance.AddDialogue("Narrador", line);
        }
    }

    // 🎯 CHOICES
    void ShowChoices()
    {
        architect.ForceComplete();
        choicesPanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < ds.currentChoices.Count)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].Setup(ds.currentChoices[i]);
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }
}