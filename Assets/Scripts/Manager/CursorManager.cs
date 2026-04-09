using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    public Texture2D normalCursor;
    public Texture2D clickCursor;

    public Vector2 hotspot = Vector2.zero;

    private bool isHoveringClickable = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetNormalCursor();
    }

    void Update()
    {
        DetectClickableUI();
    }

    // =========================
    // DETECTAR SOLO BOTONES
    // =========================
    void DetectClickableUI()
    {
        if (EventSystem.current == null || Mouse.current == null)
            return;

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Mouse.current.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);

        bool foundClickable = false;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                foundClickable = true;
                break;
            }
        }

        if (foundClickable && !isHoveringClickable)
        {
            SetClickCursor();
            isHoveringClickable = true;
        }
        else if (!foundClickable && isHoveringClickable)
        {
            SetNormalCursor();
            isHoveringClickable = false;
        }
    }

    // =========================
    // CURSORES
    // =========================
    public void SetClickCursor()
    {
        Cursor.SetCursor(clickCursor, hotspot, CursorMode.Auto);
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
    }

    // =========================
    // OBJETOS DEL MUNDO
    // =========================
    public void SetCursorClickable(bool clickable)
    {
        if (clickable)
            SetClickCursor();
        else
            SetNormalCursor();
    }
}