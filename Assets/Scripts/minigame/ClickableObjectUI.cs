using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickableObjectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool tieneObjetoDetras = false;
    public GameObject objetoOculto;

    private bool yaClick = false;
    private bool isHovering = false;
    private bool isAnimating = false;

    private Coroutine hoverCoroutine;
    private Quaternion rotacionInicial;
    private Vector3 posicionInicial;

    // Hover
    public float hoverRotSpeed = 2f;
    public float hoverRotAmount = 2f;

    // Idle saltitos
    public float tiempoMin = 2f;
    public float tiempoMax = 4f;
    public float saltoAltura = 10f;
    public float saltoDuracion = 0.15f;

    void Start()
    {
        rotacionInicial = transform.localRotation;
        posicionInicial = transform.localPosition;

        StartCoroutine(IdleLoop());
    }

    // ------------------ HOVER ------------------

    IEnumerator HoverRotate()
    {
        while (true)
        {
            float rot = Mathf.Sin(Time.time * hoverRotSpeed) * hoverRotAmount;
            transform.localRotation = Quaternion.Euler(0, 0, rot);
            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (yaClick) return;

        isHovering = true;

        CursorManager.instance.SetCursorClickable(true);

        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine);

        transform.localPosition = posicionInicial; // reset por si estaba brincando
        hoverCoroutine = StartCoroutine(HoverRotate());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;

        CursorManager.instance.SetCursorClickable(false);

        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine);

        transform.localRotation = rotacionInicial;
    }

    // ------------------ IDLE SALTITOS ------------------

    IEnumerator IdleLoop()
    {
        while (!yaClick)
        {
            float espera = Random.Range(tiempoMin, tiempoMax);
            yield return new WaitForSeconds(espera);

            if (!isHovering && !isAnimating)
            {
                yield return StartCoroutine(Salto());
            }
        }
    }

    IEnumerator Salto()
    {
        isAnimating = true;

        Vector3 arriba = posicionInicial + new Vector3(0, saltoAltura, 0);

        float tiempo = 0f;

        // subir
        while (tiempo < saltoDuracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / saltoDuracion;

            transform.localPosition = Vector3.Lerp(posicionInicial, arriba, t);
            yield return null;
        }

        tiempo = 0f;

        // bajar
        while (tiempo < saltoDuracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / saltoDuracion;

            transform.localPosition = Vector3.Lerp(arriba, posicionInicial, t);
            yield return null;
        }

        transform.localPosition = posicionInicial;

        isAnimating = false;
    }

    // ------------------ CLICK ------------------

    public void OnPointerClick(PointerEventData eventData)
    {
        if (yaClick) return;

        yaClick = true;

        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine);

        StopAllCoroutines();

        gameObject.SetActive(false);

        if (objetoOculto != null)
            objetoOculto.SetActive(true);

        if (tieneObjetoDetras)
        {
            MinigameManager.instance.EndMinigame();
        }
    }
}