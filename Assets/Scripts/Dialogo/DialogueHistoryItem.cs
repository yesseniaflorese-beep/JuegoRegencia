using TMPro;
using UnityEngine;

public class DialogueHistoryItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public void Setup(string speaker, string dialogue)
    {
        nameText.text = speaker;
        dialogueText.text = dialogue;
    }
}