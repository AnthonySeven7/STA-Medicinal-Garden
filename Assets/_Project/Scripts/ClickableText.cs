using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ClickableText : MonoBehaviour
{
    public string link = "";
    public void onClick()
    {
        if (link != "") Application.OpenURL(link);
    }
}
