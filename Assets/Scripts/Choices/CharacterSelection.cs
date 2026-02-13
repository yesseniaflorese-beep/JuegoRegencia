using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public void ElegirHombre()
    {
        GameManager.instance.selectedRoute = GameManager.PlayerRoute.Hombre;

        SceneController sceneController = FindFirstObjectByType<SceneController>();
        if (sceneController != null)
            sceneController.LoadCurrentChapter();
        else
            Debug.LogError("❌ No hay SceneController en la escena");
    }

    public void ElegirMujer()
    {
        GameManager.instance.selectedRoute = GameManager.PlayerRoute.Mujer;

        SceneController sceneController = FindFirstObjectByType<SceneController>();
        if (sceneController != null)
            sceneController.LoadCurrentChapter();
        else
            Debug.LogError("❌ No hay SceneController en la escena");
    }
}

