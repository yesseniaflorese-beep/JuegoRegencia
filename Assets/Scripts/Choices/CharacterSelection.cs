using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public void ElegirHombre()
    {
        SeleccionarRuta(GameManager.PlayerRoute.Hombre);
    }

    public void ElegirMujer()
    {
        SeleccionarRuta(GameManager.PlayerRoute.Mujer);
    }

    private void SeleccionarRuta(GameManager.PlayerRoute ruta)
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("❌ GameManager no encontrado");
            return;
        }

        GameManager.instance.selectedRoute = ruta;

        if (SceneController.instance != null)
            SceneController.instance.StartGame();
        else
            Debug.LogError("❌ SceneController no encontrado");
    }
}
