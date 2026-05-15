using UnityEngine;
using UnityEngine.UI;

public class UIButtonSFX : MonoBehaviour
{
    [Header("Nombre del SFX")]
    public string sonido;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ReproducirSonido);
    }

    void ReproducirSonido()
    {
        AudioManager.instance.PlaySFX(sonido);
    }
}