using UnityEngine;

public class BuscarObjetos : MonoBehaviour
{
    public GameObject minigameUI;

    public void StartGame()
    {
        minigameUI.SetActive(true);
    }

    public void EndGame()
    {
        minigameUI.SetActive(false);

        MinigameManager.instance.EndMinigame();
    }
}