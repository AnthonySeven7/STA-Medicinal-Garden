using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Vuforia;

public class Information : MonoBehaviour
{
    public TextAsset fileToRead;
    public string dataSetName = "";
    public GameObject augmentationObject = null;
    public Canvas canvasUI;
    public List<string[]> plants;
    public List<GameObject> plantObj;
    
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
        StreamReader fileData = new StreamReader(AssetDatabase.GetAssetPath(fileToRead)); // File to be open which contains all of the plants
        bool cont = true; // Whether or not to continue the process
        bool quote = false; // Whether the string input is in quotation marks or not
        // Read the first line of the text document
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
        return plants; // Return the list full of all the plants
    }

    bool setInformation(List<string[]> plants, List<GameObject> plantObj)
    {
        // Initiate Variables
        bool test = true;
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
            // Get information from the list and pair it with the appropriate
            comName = plants[i][0];
            sciName = plants[i][1];
            family = plants[i][2];
            moleAname = plants[i][3];
            moleAclass = plants[i][4];
            moleBname = plants[i][5];
            moleBclass = plants[i][6];
            moleCname = plants[i][7];
            moleCclass = plants[i][8];
            medicinal = plants[i][9];
            if (!int.TryParse(plants[i][10], out toxicity)) Debug.Log("<color=red>ERROR : " + comName + "'s toxicity non-convertable : '" + plants[i][10] + "'</color>"); // If the toxicity is not in the form ex:"123", then display an error message
            description = plants[i][11];
            if (!int.TryParse(plants[i][12], out hardiness)) Debug.Log("<color=red>ERROR : " + comName + "'s hardiness non-convertable : '" + plants[i][12] + "'</color>"); // If the hardiness is not in the form ex:"123", then display an error message
            
            emptyGameObject = plantObj[i];
            // Create a cube for visualization purposes and set its transform
            //model = Instantiate((Resources.Load<GameObject>("Assets/temp/models/" + comName)),new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity); [Intigrate models later]
            model = GameObject.CreatePrimitive(PrimitiveType.Cube);
            model.AddComponent<Plant>();
            model.GetComponent<Plant>().rend = model.GetComponent<MeshRenderer>();
            model.GetComponent<Plant>().canvasUI = canvasUI;
            // Set variables
            model.GetComponent<Plant>().comName = comName;
            model.GetComponent<Plant>().sciName = sciName;
            model.GetComponent<Plant>().family = family;
            model.GetComponent<Plant>().description = description;
            model.transform.parent = emptyGameObject.transform;
            model.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            model.name = "Model : " + comName;
            


            // Set Empty Game Object Transform
            emptyGameObject.transform.position = new Vector3((float)(0.25 * i), 0.0f, 0.0f);
            emptyGameObject.transform.rotation = Quaternion.identity;
            emptyGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        return test;
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
                    tb.gameObject.name = "Track: " + tb.TrackableName;

                    // add additional script components for trackable
                    tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
                    tb.gameObject.AddComponent<TurnOffBehaviour>();

                    if (augmentationObject != null)
                    {
                        // instantiate augmentation object and parent to trackable
                        GameObject augmentation = (GameObject)GameObject.Instantiate(augmentationObject);
                        augmentation.transform.parent = tb.gameObject.transform;
                        augmentation.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("<color=yellow>Warning: No augmentation object specified for: " + tb.TrackableName + "</color>");
                    }
                    plantObj.Add(tb.gameObject);
                }
            }
        }
        else
        {
            Debug.LogError("<color=yellow>Failed to load dataset: '" + dataSetName + "'</color>");
        }
    }
}
