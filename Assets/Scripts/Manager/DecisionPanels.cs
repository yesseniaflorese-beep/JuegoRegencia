using UnityEngine;
using System.Collections;

public class DecisionPanels : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelRosa;
    public GameObject panelAmarillo;
    public GameObject panelAzul;

    [Header("Duración")]
    public float duracion = 1f;

    void Start()
    {
        panelRosa.SetActive(false);
        panelAmarillo.SetActive(false);
        panelAzul.SetActive(false);
    }

    // =========================
    // ROSA
    // =========================
    public void MostrarRosa()
    {
        StartCoroutine(MostrarPanel(panelRosa));
    }

    // =========================
    // MORADO
    // =========================
    public void MostrarAmarillo()
    {
        StartCoroutine(MostrarPanel(panelAmarillo));
    }

    // =========================
    // AZUL
    // =========================
    public void MostrarAzul()
    {
        StartCoroutine(MostrarPanel(panelAzul));
    }

    // =========================
    // COROUTINE
    // =========================
    IEnumerator MostrarPanel(GameObject panel)
    {
        panel.SetActive(true);

        yield return new WaitForSeconds(duracion);

        panel.SetActive(false);
    }
}