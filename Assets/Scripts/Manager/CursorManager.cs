using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    public Texture2D normalCursor;
    public Texture2D clickCursor;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("CursorManager activo");
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorClickable(bool clickable)
    {
        if (clickable)
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }
}