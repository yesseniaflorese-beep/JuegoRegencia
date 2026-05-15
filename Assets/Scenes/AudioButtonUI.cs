using UnityEngine;
using UnityEngine.UI;

public class AudioButtonUI : MonoBehaviour
{
    public Image iconoSonido;

    public Sprite iconoActivo;
    public Sprite iconoMute;

    Button btn;

    void Start()
{
    btn = GetComponent<Button>();

    if (btn != null)
    {
        btn.onClick.AddListener(ToggleAudio);
    }
    else
    {
        Debug.LogError("❌ No hay Button en este objeto");
    }

    Invoke(nameof(ActualizarIcono), 0.1f);
}

    void ToggleAudio()
    {
        if (AudioManager.instance == null)
            return;

        AudioManager.instance.ToggleSonido();

        ActualizarIcono();
    }

    void ActualizarIcono()
{
    if (AudioManager.instance == null)
        return;

    if (iconoSonido == null)
        return;

    if (AudioManager.instance.SonidoActivo())
    {
        iconoSonido.sprite = iconoActivo;
    }
    else
    {
        iconoSonido.sprite = iconoMute;
    }
}
}