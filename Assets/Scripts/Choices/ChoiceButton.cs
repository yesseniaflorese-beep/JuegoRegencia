using UnityEngine;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    public TMP_Text label;
    private ChoiceData data;

    public void Setup(ChoiceData choice)
    {
        data = choice;
        label.text = choice.text;
    }

public void Select()
{
    // Aplicar consecuencias
    GameManager.instance.amor += data.amor;
    GameManager.instance.reputacion += data.reputacion;
    GameManager.instance.dinero += data.dinero;

    DialogueSystem ds = DialogueSystem.instance;

    // Salir del modo elección
    ds.waitingForChoice = false;
    ds.currentChoices.Clear();

    // Ocultar panel
    transform.parent.gameObject.SetActive(false);

    // 🔥 FORZAR CONTINUACIÓN DEL DIÁLOGO
    DialogueRunner runner = Object.FindFirstObjectByType<DialogueRunner>();
    if (runner != null)
    {
        runner.AdvanceDialogue();
    }
}

}

