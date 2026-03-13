using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    public GameObject buscarObjetosUI;

    void Awake()
    {
        instance = this;
    }

    public void StartMinigame(string name)
    {
        if (name == "buscar_objetos")
        {
            buscarObjetosUI.SetActive(true);
        }
    }

    public void EndMinigame()
    {
        buscarObjetosUI.SetActive(false);

        DialogueSystem.instance.waitingForChoice = false;

    }
}