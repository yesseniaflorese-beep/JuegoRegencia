using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public void SaveGame()
    {
        if (SaveSystem.instance == null)
        {
            Debug.LogError("❌ No se encontró SaveSystem");
            return;
        }

        SaveSystem.instance.SaveGame();
        Debug.Log("✅ Botón Guardar ejecutado");
    }

    public void LoadGame()
    {
        if (SaveSystem.instance == null)
        {
            Debug.LogError("❌ No se encontró SaveSystem");
            return;
        }

        SaveSystem.instance.LoadGame();
        Debug.Log("✅ Botón Cargar ejecutado");
    }
}