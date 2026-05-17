using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Agrega este componente al Canvas o cualquier GameObject del MenuInicial.
/// Todos los botones del menú deben apuntar aquí — nunca a objetos locales
/// de la escena ni llamar SceneManager directamente.
/// </summary>
public class MenuBotones : MonoBehaviour
{
    [Header("Botón Continuar (opcional: desactivar si no hay save)")]
    public Button botonContinuar;

    void Start()
    {
        // Desactivar el botón de continuar si no hay partida guardada
        if (botonContinuar != null)
            botonContinuar.interactable = PlayerPrefs.HasKey("SavedScene");
    }

    // ▶ Nueva partida
    public void NuevaPartida()
    {
        if (SceneController.instance != null)
            SceneController.instance.StartGame();
    }

    // ▶ Continuar partida guardada
    public void ContinuarPartida()
    {
        if (SaveSystem.instance != null)
            SaveSystem.instance.LoadGame();
        else
            Debug.LogWarning("⚠ SaveSystem no encontrado");
    }

    // ▶ Ir a créditos
    public void IrACreditos()
    {
        if (SceneController.instance != null)
            SceneController.instance.LoadCredits();
    }

    // ▶ Ir a instrucciones
    public void IrAInstrucciones()
    {
        if (SceneController.instance != null)
            SceneController.instance.LoadInstrucciones();
    }

    // ▶ Ir a selección de personaje
    public void IrASeleccion()
    {
        if (SceneController.instance != null)
            SceneController.instance.LoadSeleccionPersonaje();
    }

    // ▶ Salir del juego
    public void SalirJuego()
    {
        Application.Quit();
    }
}