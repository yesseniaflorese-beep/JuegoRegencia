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

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AdvanceDialogue();
        }
    }

    // ▶ AVANZAR (ESPACIO Y BOTÓN)
   public void AdvanceDialogue()
{
    // ⏩ Si está escribiendo
    if (architect.isBuilding)
    {
        if (!architect.rapido)
            architect.rapido = true;
        else
            architect.ForceComplete();

        return;
    }

    // 🟡 SI HAY DECISIÓN PENDIENTE → mostrar opciones
    if (ds.waitingForChoice)
    {
        ShowChoices();
        return;
    }

    // 📖 Pedir siguiente línea
    string line = ds.GetNextLine();


    if (line == null)
    {
            // 🟡 si es una decisión, NO terminar capítulo
        if (ds.waitingForChoice)
        {
            ShowChoices();
            return;
        }

        // 🏁 ahora sí, fin real del capítulo
        if (ds.dialogueFinished)
        {
            Debug.Log("📕 Fin del capítulo");
            enabled = false;

            SceneController sc = FindFirstObjectByType<SceneController>();
            if (sc != null)
                sc.LoadNextChapter();
        }

        return;
    }

    if (line == "")
    {
        // pausa narrativa
        return;
    }

    ProcessLine(line);
}


    // ◀ RETROCEDER (BOTÓN)
    public void BackDialogue()
    {
        if (architect.isBuilding)
        {
            architect.ForceComplete();
            return;
        }

        string line = ds.GetPreviousLine();

        if (line == null || line == "")
            return;

        ProcessLine(line);
    }

    // 🔎 PROCESA NOMBRE + TEXTO
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
        }
        else
        {
            if (nameText != null)
                nameText.text = "";

            architect.Build(line);
        }
    }

    void ShowChoices()
    {
        architect.ForceComplete(); // 👈 importante
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


