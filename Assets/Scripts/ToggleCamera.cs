using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleCamera : MonoBehaviour
{
    public GameObject ARCamera;
    public GameObject SecondCamera;
    public GameObject button;
    public bool arEnabled = true;
    // Start is called before the first frame update

    void Toggle()
    {
        arEnabled = !arEnabled;
        ARCamera.SetActive(arEnabled);
        SecondCamera.SetActive(!arEnabled);
    }

    void ButtonCastOff()
    {
        button.GetComponent<Button>().interactable = false;
    }

    void ButtonCastOn()
    {
        button.GetComponent<Button>().interactable = true;
    }

    void ButtonText()
    {
        button.GetComponent<GUIDisplay>().onClick();
    }
}
