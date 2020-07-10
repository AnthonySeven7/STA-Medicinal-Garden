using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIDisplay : MonoBehaviour
{
    #region VARIABLES
    //public GameObject background;
    public GameObject connectedDisplay = null;
    public string Buttontext;
    public bool displayActive = false;
    public Color Button_color_normal = Color.white;
    public Color Button_color_active = Color.white;
    #endregion // VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    void Start()
    {
        if (GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            Buttontext = GetComponentInChildren<TextMeshProUGUI>().text;
            if (Buttontext.Length >= 10) GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Baseline;
            else GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Midline;
        }
            if (GetComponent<Image>() != null) GetComponent<Image>().color = Button_color_normal;
    }
    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS
    public void OnClick()
    {
        if (displayActive) // If button has already been clicked and display is active
        {
            GetComponentInChildren<TextMeshProUGUI>().text = Buttontext; // Reset button text
            if (Buttontext.Length >= 10) GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Baseline;
            else GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Midline;
            GetComponent<Image>().color = Button_color_normal; // Change the button color back to normal
            displayActive = false;
        }
        else
        {
            CleanUp(); // Ensure all other displays are off
            GetComponentInChildren<TextMeshProUGUI>().text = "Close" + '\n';
            GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Midline;
            GetComponent<Image>().color = Button_color_active;
            //if (background.activeSelf == false) background.SetActive(true);
            if (connectedDisplay != null)
            {
                connectedDisplay.SetActive(true);
                displayActive = true;
            }
        }
    }
    public void Close()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = Buttontext; // Reset button text
        GetComponent<Image>().color = Button_color_normal; // Change the button color back to normal
        displayActive = false;
    }
    public void CleanUp()
    {
        foreach(Transform child in transform.parent.parent.Find("MainPanel"))
        {
            if (child.GetComponent<GUIDisplay>() != null && child.GetComponent<GUIDisplay>().displayActive) child.GetComponent<GUIDisplay>().Close();
        }
    }
    #endregion // PUBLIC_METHODS
}