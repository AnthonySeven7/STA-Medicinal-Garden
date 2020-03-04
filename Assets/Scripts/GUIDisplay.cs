using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIDisplay : MonoBehaviour
{
    public Camera ARCamera;
    public Camera SecondCamera;
    public GameObject background;
    public GameObject connectedDisplay = null;
    public string Buttontext;
    public bool displayActive = false;
    public Color Button_color_normal = Color.white;
    public Color Button_color_active = Color.white;
    
    void Start()
    {
        ARCamera = Camera.main;
        Buttontext = this.GetComponentInChildren<TextMeshProUGUI>().text;
        this.GetComponent<Image>().color = Button_color_normal;
        if (Buttontext.Length >= 10) this.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Baseline;
        else this.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Midline;
    }

    public void onClick() // [TO BE EDITED]
    {
        if (displayActive) // If button has already been clicked and display is active
        {
            this.GetComponentInChildren<TextMeshProUGUI>().text = Buttontext; // Reset button text
            if (Buttontext.Length >= 10) this.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Baseline;
            else this.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Midline;
            //if (connectedDisplay != null) connectedDisplay.SetActive(false); // Close the affiliated display [TO BE CHANGED LATER W/ ANIMATIONS]
            this.GetComponent<Image>().color = Button_color_normal; // Change the button color back to normal
            displayActive = false;
            //if (connectedDisplay != null) background.SetActive(false);
            if (ARCamera.enabled == false)
            {
                ARCamera.enabled = true;
                SecondCamera.enabled = false;
            }
        }
        else
        {
            cleanUp(); // Ensure all other displays are off
            this.GetComponentInChildren<TextMeshProUGUI>().text = "Close" + '\n';
            this.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Midline;
            this.GetComponent<Image>().color = Button_color_active;
            if (background.activeSelf == false) background.SetActive(true);
            if (connectedDisplay != null)
            {
                connectedDisplay.SetActive(true);
                if (connectedDisplay.name == "OptionsPanel")
                {
                    SecondCamera.enabled = true;
                    ARCamera.enabled = false;
                }
                displayActive = true;
            }
        }
    }

    public void close()
    {
        this.GetComponentInChildren<TextMeshProUGUI>().text = Buttontext; // Reset button text
        //if (connectedDisplay != null && connectedDisplay.name != "OptionsPanel") connectedDisplay.SetActive(false); // Close the affiliated display [TO BE CHANGED LATER W/ ANIMATIONS]
        this.GetComponent<Image>().color = Button_color_normal; // Change the button color back to normal
        displayActive = false;
    }

    void cleanUp()
    {
        Debug.Log(this.transform.parent.transform);
        foreach(Transform child in this.transform.parent.transform)
        {
            if (child.GetComponent<GUIDisplay>().displayActive) child.GetComponent<GUIDisplay>().close();
        }
    }
}

