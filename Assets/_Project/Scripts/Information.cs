using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Vuforia;
using TMPro;

/// <summary>
/// A system that imports information from a provided .csv document and creates game objects for each plant in said document
/// </summary>
public class Information : MonoBehaviour
{
    #region VARIABLES
    [Header("Data (Hover over variables for descriptions)")]
    [Tooltip("CSV file that contains all information on the plants.")]
    public TextAsset fileToRead;
    [Tooltip("Name of Vuforia Dataset to use.")]
    public string datasetName = "";
    [Tooltip("Default prefab that will be displayed on the plaque.")]
    public GameObject plaquePrefab;
    [Tooltip("Canvas where all information will be displayed.")]
    public Canvas canvasUI;

    [Tooltip("The default model to be displayed if no model can be found for the plant.")]
    public GameObject defaultModel;
    [Tooltip("The default prefab used when creating a hyperlink.")]
    public GameObject linkPrefab;

    [HideInInspector] public List<GameObject> plantObj;
    [HideInInspector] public List<string[]> plants;

    #endregion // VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    private void Start()
    {
        // Get a list of all of the plants with all of their information
        plants = ReadFile();
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(LoadDataSet);
        var result = SetInformation(plants, plantObj);
        if (result) Debug.Log("Instances Created : <color=green>true</color>");
        else Debug.Log("Instances Created: <color=red>false</color>");
    }
    #endregion //UNITY_MONOBEHAVIOUR_METHODS

    #region PRIVATE_METHODS
    // Open and read provided data file (fileToRead) and return a list of all plants in the file
    // Each plant in returned list will be, itself, a list containing all the information about itself
    private List<string[]> ReadFile()
    {
        // Initialize Variables
        List<string[]> plants = new List<string[]>(); // Ensure 'plants' is a blank list
        bool cont = true; // Whether to continue the process
        bool quote = false; // Whether the current information contains a quote (this is used to keep commas in some information)

        #if UNITY_EDITOR // If we're in the Unity Editor
        StreamReader fileData = new StreamReader(AssetDatabase.GetAssetPath(fileToRead)); // Open the CSV file provided and prepare to start reading
        Debug.Log(string.Format("Opening file: {0}.", fileToRead.name));
        fileData.ReadLine(); // Read the first line (this line contains the headers that we don't care about so we skip it here)
        while (cont) // While we are continuing (the entire document)
        {
            // Initialize Local Variables (reinitialized each loop)
            string[] entry = new string[14]; // A new, blank, plant entry to be filled in (14 = the number of columbs in the CSV document)
            string str = fileData.ReadLine(); // Current read line of the document
            int index = 0;
            string data = "";
            if (str != null) // Check the current line is not empty
            {
                foreach (char ch in str) // For each letter in the current line
                {
                    if (ch == '"') quote = !quote;
                    else if (!quote && ((ch == ',') || ("" + ch) == "\n"))
                    {
                        entry[index] = data; // Store the information
                        data = ""; // Reset the variable
                        index++; // Increment the index
                    }
                    //else if (!quote && ("" + ch) == "\n")
                    else data += ch;
                }
                plants.Add(entry); // Add the plant entry to the List
            }
            else
            {
                // If the rest of the text file is empty, then don't continue
                cont = false;
            }
        }
        fileData.Close(); // Close the text file
        #else
        string path = Application.dataPath;
        int mainindex = 0;
        TextAsset text = Resources.Load<TextAsset>("Data_files/" + fileToRead.name);
        string[] linesFromFile = text.text.Split("\n"[0]);
        for (int i = 0; i < linesFromFile.Length; i++)
        {
            string[] entry = new string[14];
            string str = linesFromFile[i];
            if (mainindex != 0)
            {
                // Set / Reset Variables
                int index = 0;
                string data = "";
                // Check if the current line of the text file is empty
                if (str != null)
                {
                    foreach (char ch in str) // For each letter in the current line
                    {
                        if (ch == '"') quote = !quote;
                        else if (!quote && ((ch == ',') || ("" + ch) == "\n"))
                        {
                            entry[index] = data; // Store the information
                            data = ""; // Reset the variable
                            index++; // Increment the index
                        }
                        //else if (!quote && ("" + ch) == "\n")
                        else data += ch;
                    }
                    if (entry[0] != "")
                    {
                        plants.Add(entry); // Add the plant entry to the List
                    }
                }
                else
                {
                    // If the rest of the text file is empty, then don't continue
                    cont = false;
                }
            }
            mainindex++;
        }
        #endif
        return plants; // Return the list full of all the plants
    }
    private bool SetInformation(List<string[]> plants, List<GameObject> plantObj)
    {
        // Initiate Variables
        bool test1 = true;
        string comName, sciName, family, moleAname, moleAclass, moleBname, moleBclass, moleCname, moleCclass, medicinal, description, toxicity;
        int[] hardiness;
        //string[] links;
        GameObject emptyGameObject, mainModel, moleAmodel, moleBmodel, moleCmodel;
        GameObject model;
        for (int i = 0; i < plantObj.Count; i++) // For each plant in the list
        {
            bool match = false;
            // Get information from the list and pair it with the appropriate
            #if UNITY_EDITOR
            for (int x = 0; x < plants.Count; x++)
            #else
            for (int x = 0; x < (plants.Count)-1; x++)
            #endif
            {
                string name = "";
                hardiness = new int[2];
                foreach(char ch in plants[x][0])
                {
                    if (ch != ' ') name += ch; // Get the name of the plant without spaces
                }
                if (plantObj[i].name.ToLower() == name.ToLower())
                {
                    // Reset variables for local use
                    mainModel = null;
                    moleAmodel = null;
                    moleBmodel = null;
                    moleCmodel = null;

                    match = true;
                    // Gather information
                    comName = plants[x][0];
                    sciName = plants[x][1];
                    family = plants[x][2];
                    moleAname = plants[x][3];
                    moleAclass = plants[x][4];
                    moleBname = plants[x][5];
                    moleBclass = plants[x][6];
                    moleCname = plants[x][7];
                    moleCclass = plants[x][8];
                    medicinal = plants[x][9];
                    toxicity = plants[x][10];
                    description = plants[x][11];
                    var hard = plants[x][12].Split(' ');
                    if (hard.Length != 3 && hard.Length != 1) Debug.Log(string.Format("<color=red>ERROR : </color> {0}'s hardiness not of form '# to #'.", comName));
                    else
                    {
                        if (hard.Length == 3) // If a range of hardiness zones was provided
                        {
                            if (!int.TryParse(hard[0], out hardiness[0])) Debug.Log(string.Format("<color=red>ERROR : </color> {0}'s hardiness non-convertable : '{1}'.", comName, hard[0])); // If the hardinessStart is not in the form ex:"123", then display an error message
                            if (!int.TryParse(hard[2], out hardiness[1])) Debug.Log(string.Format("<color=red>ERROR : </color> {0}'s hardiness non-convertable : '{1}'.", comName, hard[2])); // If the hardinessEnd is not in the form ex:"123", then display an error message
                        }
                        else // If only one hardiness zone was provided
                        {
                            if (!int.TryParse(hard[0], out hardiness[0])) Debug.Log(string.Format("<color=red>ERROR : </color> {0}'s hardiness non-convertable : '{1}'.", comName, hard[0])); // If the hardinessStart is not in the form ex:"123", then display an error message
                            if (!int.TryParse(hard[0], out hardiness[1])) Debug.Log(string.Format("<color=red>ERROR : </color> {0}'s hardiness non-convertable : '{1}'.", comName, hard[0])); // If the hardinessStart is not in the form ex:"123", then display an error message
                        }
                    }
                    // 'links' is equal to the string of links broken up by spaces 
                    string[] links = plants[x][13].Split(' ');
                    emptyGameObject = plantObj[i];

                    // Check Assets/Resources/Prefabs for models that match the names provided
                    if ((comName != null) && (comName != ""))
                    {
                        mainModel = getPrefab(comName, false);
                        //mainModel.transform.parent = emptyGameObject.transform;
                    }
                    if ((moleAname != null) && (moleAname != "")) moleAmodel = getPrefab(moleAname, true);
                    if ((moleBname != null) && (moleBname != "")) moleBmodel = getPrefab(moleBname, true);
                    if ((moleCname != null) && (moleCname != "")) moleCmodel = getPrefab(moleCname, true);
                    model = Instantiate(plaquePrefab, new Vector3(0.0f, 0.02f, 0.0f), Quaternion.identity);
                    Plant plantModel = model.GetComponent<Plant>();
                    plantModel.rend = model.GetComponent<MeshRenderer>();
                    plantModel.canvasUI = canvasUI;
                    if (model.GetComponent<LockModel>() != null) model.GetComponent<LockModel>().canvasUI = canvasUI;
                    if (model.GetComponent<Rotate>() != null) model.GetComponent<Rotate>().canvasUI = canvasUI;
                    // Transfer Information
                    plantModel.linkPrefab = linkPrefab;
                    plantModel.plantModel = mainModel;
                    plantModel.comName = comName;
                    //model.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = comName;
                    plantModel.sciName = sciName;
                    plantModel.family = family;
                    plantModel.description = description;
                    plantModel.moleAname = moleAname;
                    plantModel.moleAclass = moleAclass;
                    plantModel.moleAmodel = moleAmodel;
                    plantModel.moleBname = moleBname;
                    plantModel.moleBclass = moleBclass;
                    plantModel.moleBmodel = moleBmodel;
                    plantModel.moleCname = moleCname;
                    plantModel.moleCclass = moleCclass;
                    plantModel.moleCmodel = moleCmodel;
                    plantModel.medicinal = medicinal;
                    plantModel.toxicity = toxicity;
                    plantModel.hardiness = hardiness;
                    plantModel.links = links;

                    model.transform.parent = emptyGameObject.transform;
                    model.name = "Info : " + comName;
                    // Set Empty Game Object Transform
                    emptyGameObject.transform.position = new Vector3((float)(0.25 * i), 0.0f, 0.0f);
                    emptyGameObject.transform.rotation = Quaternion.identity;
                    emptyGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                }
            }
            if (!match) Debug.Log(string.Format("<color=red>ERROR :</color> '{0}' does not match any plant in database.", plantObj[i].name));
            plantObj[i].name = (plantObj[i].name);
        }
        return test1;
    }
    private void LoadDataSet()
    {
        // Initialize Varaibles
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        DataSet dataSet = objectTracker.CreateDataSet();

        if (dataSet.Load(datasetName))
        {
            objectTracker.Stop();  // stop tracker so that we can add new dataset
            if (!objectTracker.ActivateDataSet(dataSet))
            {
                // Note: ImageTracker cannot have more than 100 total targets activated
                Debug.Log(string.Format("<color=yellow>Failed to Activate DataSet: </color>'{0}'", datasetName));
            }
            if (!objectTracker.Start()) Debug.Log("<color=yellow>Tracker Failed to Start.</color>");

            IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
            foreach (TrackableBehaviour tb in tbs)
            {
                if (tb.name == "New Game Object")
                {
                    // change generic name to include trackable name
                    tb.gameObject.name = tb.TrackableName;

                    // add additional script components for trackable
                    tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
                    tb.gameObject.AddComponent<TurnOffBehaviour>();
                    plantObj.Add(tb.gameObject);
                }
            }
        }
        else
        {
            Debug.LogError(string.Format("<color=yellow>Failed to load dataset: </color>'{0}'", datasetName));
        }
    }
    GameObject getPrefab(string name, bool mole)
    {
        if ((name.Length > 0) && (name[name.Length - 1] == ' ')) name = name.Substring(0, name.Length - 1);
        GameObject mprefab;
        if (!mole) mprefab = Resources.Load<GameObject>("Prefabs/" + name);
        else mprefab = Resources.Load<GameObject>("Prefabs/Molecules/" + name);
        if (mprefab == null && !mole)
        {
            Debug.Log(string.Format("<color=yellow>Warning : </color>Assets/Resources/Prefabs/'{0}' not found", name));
            return defaultModel;
        }
        else if (mprefab == null && mole)
        {
            Debug.Log(string.Format("<color=yellow>Warning : </color>Assets/Resources/Prefabs/Molecules/'{0}' not found", name));
            return defaultModel;
        }
        return mprefab;
    }
    #endregion // PRIVATE_METHODS
}