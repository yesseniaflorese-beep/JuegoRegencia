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
        if (!string.IsNullOrEmpty(data.statName))
        {
            GameManager.instance.AddStat(data.statName, data.statValue);
        }

        DialogueSystem ds = DialogueSystem.instance;
        ds.waitingForChoice = false;
        ds.currentChoices.Clear();

        transform.parent.gameObject.SetActive(false);

        DialogueRunner runner = FindFirstObjectByType<DialogueRunner>();
        if (runner != null)
            runner.AdvanceDialogue();
    }
}
