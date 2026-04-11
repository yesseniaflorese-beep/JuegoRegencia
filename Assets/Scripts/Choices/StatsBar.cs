using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public Image amorBar;
    public Image reputacionBar;
    public Image dineroBar;

    void OnEnable()
    {
        GameManager.onStatChanged += UpdateStatUI;
    }

    void OnDisable()
    {
        GameManager.onStatChanged -= UpdateStatUI;
    }

    void Start()
    {
        ActualizarBarras();
    }

    void UpdateStatUI(string statName)
    {
        switch (statName)
        {
            case "amor":
                amorBar.fillAmount = GameManager.instance.amor / 100f;
                break;

            case "reputacion":
                reputacionBar.fillAmount = GameManager.instance.reputacion / 100f;
                break;

            case "dinero":
                dineroBar.fillAmount = GameManager.instance.dinero / 100f;
                break;
        }
    }

    void ActualizarBarras()
    {
        amorBar.fillAmount = GameManager.instance.amor / 100f;
        reputacionBar.fillAmount = GameManager.instance.reputacion / 100f;
        dineroBar.fillAmount = GameManager.instance.dinero / 100f;
    }
}