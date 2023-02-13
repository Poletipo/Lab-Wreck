using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoggedInfo : MonoBehaviour
{
    public TextMeshProUGUI ControllerText;
    public TextMeshProUGUI PlayerIndexText;

    public void Setup(string device, int index)
    {
        ControllerText.text = device;
        PlayerIndexText.text = index.ToString();
    }

}
