using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public Image amorBar;
    public Image reputacionBar;
    public Image dineroBar;

    [Range(0f, 1f)]
    public float amor = 0.5f;

    [Range(0f, 1f)]
    public float reputacion = 0.8f;

    [Range(0f, 1f)]
    public float dinero = 0.7f;

    void Start()
    {
        ActualizarBarras();
    }

    public void CambiarAmor(float cantidad)
    {
        amor += cantidad;
        amor = Mathf.Clamp01(amor);
        amorBar.fillAmount = amor;
    }

    public void CambiarReputacion(float cantidad)
    {
        reputacion += cantidad;
        reputacion = Mathf.Clamp01(reputacion);
        reputacionBar.fillAmount = reputacion;
    }

    public void CambiarDinero(float cantidad)
    {
        dinero += cantidad;
        dinero = Mathf.Clamp01(dinero);
        dineroBar.fillAmount = dinero;
    }

    void ActualizarBarras()
    {
        amorBar.fillAmount = amor;
        reputacionBar.fillAmount = reputacion;
        dineroBar.fillAmount = dinero;
    }
}