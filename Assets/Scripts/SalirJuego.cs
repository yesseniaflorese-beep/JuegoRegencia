using UnityEngine;

public class SalirJuego : MonoBehaviour
{
    public void Salir()
    {
        Application.Quit();

        Debug.Log("Juego cerrado");
    }
}