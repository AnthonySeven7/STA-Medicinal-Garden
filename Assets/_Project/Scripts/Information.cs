using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Vuforia;
using TMPro;

public class Information : MonoBehaviour
{
    public TextAsset fileToRead;
    public string dataSetName = "";
    public Canvas canvasUI;
    public List<string[]> plants;
    public GameObject prefab;
    public List<GameObject> plantObj;
    public TextMeshProUGUI testText;
    void Start()
    {
        // Get a list of all of the plants with all of their information
        plants = readFile();
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(LoadDataSet);
        Debug.Log("Instances Created : " + setInformation(plants,plantObj));
    }

    List<string[]> readFile()
    {
        // Initiate Variables
        List<string[]> plants = new List<string[]>(); // A list to place each plant into
        bool cont = true; // Whether or not to continue the process
        bool quote = false; // Whether the string input is in quotation marks or not
                            // Read the first line of the text document
#if UNITY_EDITOR
        StreamReader fileData = new StreamReader(AssetDatabase.GetAssetPath(fileToRead)); // File to be open which contains all of the plants
        Debug.Log(fileToRead.name);
        fileData.ReadLine();
        while (cont)
        {
            // Create a new, blank, plant entry to be filled in
            string[] entry = new string[13];
            // Read current line of the text file
            string str = fileData.ReadLine();
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
            string[] entry = new string[13];
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
        return plants; // Return the lit full of all the plants
    }

    bool setInformation(List<string[]> plants, List<GameObject> plantObj)
    {
        // Initiate Variables
        bool test1 = true;
        string comName;
        string sciName;
        string family;
        string moleAname;
        string moleAclass;
        string moleBname;
        string moleBclass;
        string moleCname;
        string moleCclass;
        string medicinal;
        int toxicity;
        string description;
        int hardiness;
        GameObject emptyGameObject;
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
                foreach(char ch in plants[x][0])
                {
                    if (ch != ' ') name += ch; // Get the name of the plant without spaces
                }
                if (plantObj[i].name.ToLower() == name.ToLower())
                {
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
                    if (!int.TryParse(plants[x][10], out toxicity)) Debug.Log("<color=red>ERROR : " + comName + "'s toxicity non-convertable : '" + plants[x][10] + "'</color>"); // If the toxicity is not in the form ex:"123", then display an error message
                    description = plants[x][11];
                    if (!int.TryParse(plants[x][12], out hardiness)) Debug.Log("<color=red>ERROR : " + comName + "'s hardiness non-convertable : '" + plants[x][12] + "'</color>"); // If the hardiness is not in the form ex:"123", then display an error message
                    emptyGameObject = plantObj[i];

                    // Create a cube for visualization purposes and set its transform
                    GameObject myprefab = getPrefab(comName);
                    model = Instantiate(myprefab, new Vector3(0.0f, 0.02f, 0.0f), Quaternion.identity); //Assets/Prefabs/NameTag.prefab
                    model.GetComponent<Plant>().rend = model.GetComponent<MeshRenderer>();
                    model.GetComponent<Plant>().canvasUI = canvasUI;
                    model.GetComponent<LockModel>().canvasUI = canvasUI;

                    // Transfer Information
                    model.GetComponent<Plant>().comName = comName;
                    model.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = comName;
                    model.GetComponent<Plant>().sciName = sciName;
                    model.GetComponent<Plant>().family = family;
                    model.GetComponent<Plant>().description = description;
                    model.GetComponent<Plant>().moleAname = moleAname;
                    model.GetComponent<Plant>().moleAclass = moleAclass;
                    model.GetComponent<Plant>().moleBname = moleBname;
                    model.GetComponent<Plant>().moleBclass = moleBclass;
                    model.GetComponent<Plant>().moleCname = moleCname;
                    model.GetComponent<Plant>().moleCclass = moleCclass;
                    model.GetComponent<Plant>().medicinal = medicinal;
                    model.GetComponent<Plant>().toxicity = toxicity;
                    model.GetComponent<Plant>().hardiness = hardiness;

                    model.transform.parent = emptyGameObject.transform;
                    model.name = "Model : " + comName;
                    // Set Empty Game Object Transform
                    emptyGameObject.transform.position = new Vector3((float)(0.25 * i), 0.0f, 0.0f);
                    emptyGameObject.transform.rotation = Quaternion.identity;
                    emptyGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                }
            }
            if (!match) Debug.Log("<color=red>ERROR : " + plantObj[i].name + " does not match any plant in database.</color>");
            plantObj[i].name = ("Track : " + plantObj[i].name);
        }
        return test1;
    }

    void LoadDataSet()
    {
        // Initialize Varaibles
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        DataSet dataSet = objectTracker.CreateDataSet();

        if (dataSet.Load(dataSetName))
        {
            objectTracker.Stop();  // stop tracker so that we can add new dataset
            if (!objectTracker.ActivateDataSet(dataSet))
            {
                // Note: ImageTracker cannot have more than 100 total targets activated
                Debug.Log("<color=yellow>Failed to Activate DataSet: " + dataSetName + "</color>");
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
            Debug.LogError("<color=yellow>Failed to load dataset: '" + dataSetName + "'</color>");
        }
    }

    GameObject getPrefab(string name)
    {
        if (name[name.Length - 1] == ' ') name = name.Substring(0, name.Length - 1);
        GameObject mprefab = Resources.Load<GameObject>("Prefabs/" + name);
        if (mprefab == null) mprefab = prefab;
        return mprefab;
    }
}