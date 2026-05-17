using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
 
/// <summary>
/// Agrega este componente al mismo GameObject que tiene el EventSystem.
/// Bloquea que Enter/Submit del teclado active botones de UI,
/// dejando solo el click del mouse o el botón designado en el juego.
/// </summary>
public class NoKeyboardSubmit : MonoBehaviour
{
    InputSystemUIInputModule inputModule;
 
    void Awake()
    {
        inputModule = GetComponent<InputSystemUIInputModule>();
    }
 
    void Update()
    {
        // Deseleccionar siempre para que Enter no dispare el botón activo
        if (EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
 