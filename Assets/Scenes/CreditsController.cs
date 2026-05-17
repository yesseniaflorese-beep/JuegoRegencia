using UnityEngine;
 
public class CreditsController : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneController.instance.LoadMenu();
    }
}
 