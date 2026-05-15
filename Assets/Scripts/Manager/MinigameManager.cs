using UnityEngine;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    public GameObject buscarObjetosUI;

    public bool isMinigameActive = false;

    void Awake()
    {
        instance = this;
    }

    public void StartMinigame(string name)
    {
        if (name == "buscar_objetos")
        {
            isMinigameActive = true;
            buscarObjetosUI.SetActive(true);
        }
    }

    public void EndMinigame()
    {
        StartCoroutine(EndMinigameDelay());
    }

    IEnumerator EndMinigameDelay()
    {
        yield return new WaitForSeconds(1f);

        buscarObjetosUI.SetActive(false);

        isMinigameActive = false;

        DialogueSystem.instance.waitingForChoice = false;
    }
}