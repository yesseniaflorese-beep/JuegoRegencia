using UnityEngine;
using TMPro;

public class UIStatsDisplay : MonoBehaviour
{
    public TMP_Text amorValue;
    public TMP_Text reputacionValue;
    public TMP_Text dineroValue;

    void Update()
    {
        if (GameManager.instance == null) return;

        amorValue.text = GameManager.instance.amor.ToString();
        reputacionValue.text = GameManager.instance.reputacion.ToString();
        dineroValue.text = GameManager.instance.dinero.ToString();
    }
}
