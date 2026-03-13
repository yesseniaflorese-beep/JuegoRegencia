using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObjectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool tieneObjetoDetras = false;
    public GameObject objetoOculto;
    public Vector3 moverOffset = new Vector3(100f, 0, 0);

    private bool yaClick = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse encima");
        CursorManager.instance.SetCursorClickable(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.instance.SetCursorClickable(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK DETECTADO");

        if (yaClick) return;

        yaClick = true;

        transform.localPosition += moverOffset;

        if (objetoOculto != null)
            objetoOculto.SetActive(true);

        if (tieneObjetoDetras)
        {
            MinigameManager.instance.EndMinigame();
        }
    }
}