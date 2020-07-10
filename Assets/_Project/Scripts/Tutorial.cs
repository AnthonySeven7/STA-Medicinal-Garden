using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// Controller for the tutorial system
/// </summary>
public class Tutorial : MonoBehaviour
{
    #region VARIABLES
    public GameObject[] scenes; // list of all the scenes (in order)
    [Space]
    public GameObject notTracking; // scene to display when nothing is tracked

    private int mainPanelIndex = 0; // index of the 'MainPanel' section of the tutorial
    private int notTrackingIndex = 0; // index of the not tracking scene
    private int sceneNum = 0; // current tutorial scene
    private bool tutorial = true; // is the user using the tutorial?
    #endregion // VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    // Get index of tracking scene
    private void Start()
    {
        for(int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i] == notTracking) notTrackingIndex = i;
            if (scenes[i].name == "Introduction") mainPanelIndex = i;
        }
    }
    // Check tracking
    private void Update()
    {
        // Ensure user tracking something to use tutorial
        if ((sceneNum >= notTrackingIndex) && (sceneNum < scenes.Length - 1) && (tutorial)) // ensure user is far enough into tutorial but not on last scene
        {
            // Get the Vuforia StateManager
            StateManager sm = TrackerManager.Instance.GetStateManager();
            // Query the StateManager for a list of currently 'active' trackables (converted to ICollection to utilize the 'Count' method)
            // (i.e. the ones currently being tracked by Vuforia)
            ICollection<TrackableBehaviour> activeTrackables = (ICollection<TrackableBehaviour>)sm.GetActiveTrackableBehaviours();
            if (activeTrackables.Count == 0) // if nothing is currently being tracked then tell user to track something
            {
                Tracking(false);
            }
            else // if something is being tracked
            {
                if (tutorial)
                {
                    if (sceneNum == notTrackingIndex) sceneNum++;
                    Tracking(true);
                }
            }
        }
        else if (tutorial) Tracking(true);
    }
    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS
    // Continue to next tutorial scene
    public void Next()
    {
        if (tutorial)
        {
            scenes[sceneNum].SetActive(false); // disable current screen
            sceneNum++;
        }
    }
    // Skip the tutorial to a particular scene (used for the 'MainPanel' section)
    public void Skip(int scene)
    {
        if (tutorial)
        {
            scenes[sceneNum].SetActive(false); // disable current screen
            if (sceneNum == (mainPanelIndex + scene)) sceneNum = mainPanelIndex; // if panel is already opened, go back to 'mainPanel'
            else sceneNum = (mainPanelIndex + scene); // open a specific panel
        }
    }
    // Close tutorial
    public void Close()
    {
        tutorial = false;
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].SetActive(false);
        }
        sceneNum = 0;
    }
    // Method called when the options button is pressed in the tutorial
    public void OptionsButton()
    {
        if (tutorial)
        {
            scenes[sceneNum].SetActive(false);
            sceneNum++; // Check if tutorial is in use to advance
        }
    }

    public void Begin()
    {
        sceneNum = 0;
        tutorial = true;
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].SetActive(false);
        }
    }
    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS
    // Method to set tutorial scene based on it object is being tracked
    private void Tracking(bool state)
    {
        if (scenes[sceneNum].activeSelf != state) scenes[sceneNum].SetActive(state);
        if (notTracking.activeSelf == state) notTracking.SetActive(!state);
    }
    #endregion // PRIVATE_METHODS
}
