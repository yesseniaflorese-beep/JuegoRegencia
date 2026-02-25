using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;

    [System.Serializable]
    public class SceneSlot
    {
        public string slotName;      // izquierda, derecha, centro, extra1...
        public RawImage image;
    }

    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public List<string> expressions;
        public List<Texture> textures;
    }

    public List<SceneSlot> sceneSlots;
    public List<CharacterData> characters;

    private Dictionary<string, RawImage> slotDictionary;

    void Awake()
    {
        instance = this;

        slotDictionary = new Dictionary<string, RawImage>();

        foreach (var slot in sceneSlots)
        {
            if (slot.image != null)
            {
                string key = slot.slotName.Trim().ToLower();
                slotDictionary[key] = slot.image;

                // 🔥 IMPORTANTE: iniciar ocultos para que no salgan blancos
                slot.image.texture = null;
                slot.image.color = new Color(1, 1, 1, 0);
                slot.image.gameObject.SetActive(false);
            }
        }
    }

    // ================================
    // MOSTRAR SPRITE
    // ================================
    public void ShowSprite(string character, string expression, string position)
    {
        foreach (var c in characters)
        {
            if (c.characterName.Trim().ToLower() == character.Trim().ToLower())
            {
                for (int i = 0; i < c.expressions.Count; i++)
                {
                    if (c.expressions[i].Trim().ToLower() == expression.Trim().ToLower())
                    {
                        string pos = position.Trim().ToLower();

                        if (slotDictionary.ContainsKey(pos))
                        {
                            RawImage target = slotDictionary[pos];

                            target.texture = c.textures[i];
                            target.color = new Color(1, 1, 1, 1); // visible
                            target.gameObject.SetActive(true);
                        }
                        else
                        {
                            Debug.LogWarning("⚠ Slot no existe: " + position);
                        }

                        return;
                    }
                }
            }
        }

        Debug.LogWarning("⚠ No se encontró personaje o expresión.");
    }

    // ================================
    // OCULTAR SLOT
    // ================================
    public void HideSlot(string position)
    {
        string pos = position.Trim().ToLower();

        if (slotDictionary.ContainsKey(pos))
        {
            RawImage target = slotDictionary[pos];
            target.texture = null;
            target.color = new Color(1, 1, 1, 0);
            target.gameObject.SetActive(false);
        }
    }

    // ================================
    // LIMPIAR TODOS LOS SLOTS
    // ================================
    public void ClearAllSlots()
    {
        foreach (var slot in slotDictionary.Values)
        {
            slot.texture = null;
            slot.color = new Color(1, 1, 1, 0);
            slot.gameObject.SetActive(false);
        }
    }
}