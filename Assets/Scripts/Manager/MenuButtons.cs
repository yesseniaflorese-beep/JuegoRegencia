using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void IrASeleccion()
    {
        if (SceneController.instance != null)
            SceneController.instance.LoadSeleccionPersonaje();
    }
}