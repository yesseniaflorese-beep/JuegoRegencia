using UnityEngine;
using UnityEngine.UI;

public class MenuBotones : MonoBehaviour
{
    public Button nuevoJuegoBtn;
    public Button continuarBtn;

    void Start()
    {
        // NUEVA HISTORIA
        nuevoJuegoBtn.onClick.AddListener(() =>
        {
            SaveSystem.instance.DeleteSave();

            SceneController.instance.LoadSceneByName("CapituloInicio");
        });

        // CONTINUAR
        continuarBtn.onClick.AddListener(() =>
        {
            SaveSystem.instance.LoadGame();
        });

        // Desactivar continuar si no existe save
        continuarBtn.interactable =
            PlayerPrefs.HasKey("SavedScene");
    }
}