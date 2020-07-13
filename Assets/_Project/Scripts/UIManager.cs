using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Script that controls the panels of information
/// </summary>
public class UIManager : MonoBehaviour
{
    #region VARIABLES
    public GameObject  gen, molA, molB, molC, more, selected;
    [Space]
    public string activePanel;
    [Space]
    [Header("Text With Resizeable Component")]
    public TextMeshProUGUI[] tmpComponents;
    #endregion // VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    // Start is called before the first frame update
    void Start()
    {
        activePanel = "";
    }
    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS
    /// <summary> 
    /// Methods control which panel is being displayed when a button is pressed
    /// </summary>
    public void GeneralInfoButton()
    {
        CheckPanel(gen, "gen_info");
    }
    public void MoleAButton()
    {
        CheckPanel(molA, "moleA");
    }
    public void MoleBButton()
    {
        CheckPanel(molB, "moleB");
    }
    public void MoleCButton()
    {
        CheckPanel(molC, "moleC");
    }
    public void MoreButton()
    {
        CheckPanel(more, "more");
    }
    public void Options()
    {
        CheckPanel(null,"opt");
    }
    public void CleanUp()
    {
        if (activePanel != "") // if there's a panel open
        {
            if (selected != null)
            {
                // Close the panel
                StartCoroutine(PanelAnimate(selected, 0.0f, "down"));
            }
        }
        selected = null;
        activePanel = "";
    }

    public void CheckPanel(GameObject pointer, string name)
    {
        if (activePanel != "") // if there's not a panel open
        {
            if (selected != null)
            {
                StartCoroutine(PanelAnimate(selected, 0.0f, "down"));
            }
        }

        if (activePanel != name) // if active panel is not what we just clicked
        {
            selected = pointer;
            if (name != "opt")
            {
                StartCoroutine(PanelAnimate(selected, 0.3f, "up"));
            }
            activePanel = name;
        }

        else // if we closed the open panel
        {
            selected = null;
            activePanel = "";
        }
    }
    // Change the font size of all tmpComponents
    public void SetFontSize(float size)
    {
        for(int i = 0; i < tmpComponents.Length; i++)
        {
            tmpComponents[i].fontSize = size;
        }
    }

    IEnumerator PanelAnimate(GameObject pointer, float delay, string trigger)
    {
        yield return new WaitForSeconds(delay);
        pointer.GetComponent<Animator>().ResetTrigger(trigger);
        pointer.GetComponent<Animator>().SetTrigger(trigger);
    }
    #endregion // PUBLIC_METHODS
}
