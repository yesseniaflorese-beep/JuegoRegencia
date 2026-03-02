using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject panelOpciones; // El canvas o panel que quieres abrir

    public void AbrirPanel()
    {
        panelOpciones.SetActive(true);
    }

    public void CerrarPanel()
    {
        panelOpciones.SetActive(false);
    }
}