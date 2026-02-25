using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    public Image backgroundImage;

    public List<string> backgroundNames;
    public List<Sprite> backgrounds;

    void Awake()
    {
        instance = this;
    }

    public void ChangeBackground(string name)
{
    // 🔥 LIMPIAR PERSONAJES ANTES DE CAMBIAR ESCENA
    if (SpriteManager.instance != null)
    {
        SpriteManager.instance.ClearAllSlots();
    }

    for (int i = 0; i < backgroundNames.Count; i++)
    {
        if (backgroundNames[i].ToLower() == name.ToLower())
        {
            backgroundImage.sprite = backgrounds[i];
            return;
        }
    }

    Debug.LogWarning("⚠ Background no encontrado: " + name);
}
}