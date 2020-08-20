using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

/// <summary>
/// A system to store and display information about a specific plant
/// 
/// [Information stored in this script is provided by the script 'Information.cs']
/// This script stores information about a specific plant and is called upon when that specific plant is recognized by the Vuforia system.
/// Displaying the stored information to the designated location on the GUI when the specific object is being tracked and resets the display back
/// to default values, once the object's tracking has been lost, for future tracking.
/// </summary>
public class Plant : MonoBehaviour
{
    #region VARIABLES
    // Initialize Variables
    public Canvas canvasUI;
    public MeshRenderer rend;
    public GameObject linkPrefab;

    // Information for the plant
    public string comName, sciName, family, moleAname, moleAclass, moleAlink, moleBname, moleBclass, moleBlink, moleCname, moleCclass, moleClink;
    public GameObject plantModel, moleAmodel, moleBmodel, moleCmodel;
    [Multiline]
    public string description, medicinal;
    public int toxicity = -1;
    public int[] hardiness = new int[2] { -1, -1 };
    public string[] links;

    // Locations where the information is displayed
    public TextMeshProUGUI tmp_comName, tmp_sciName, tmp_fam, tmp_description, tmp_moleAname, tmp_moleAclass, tmp_moleBname, tmp_moleBclass, tmp_moleCname, tmp_moleCclass, tmp_medicinal, tmp_hardiness;
    public GameObject[] hardinessImages = new GameObject[14];

    // List of all current buttons in GUI
    public List<string> buttons;
    
    // Test variable to prevent overlooping
    private bool current = true;

    public List<GameObject> _links;

    #endregion // VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    // Start is called before the first frame update
    void Start()
    {
        // Assign Variables On Start
        rend = GetComponent<MeshRenderer>();
        // Assign Text Mesh Pro Components
        tmp_comName = canvasUI.transform.Find("Screens").Find("GeneralInfo").Find("Viewport").Find("Content").Find("Common Name").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_sciName = canvasUI.transform.Find("Screens").Find("GeneralInfo").Find("Viewport").Find("Content").Find("Scientific Name").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_fam = canvasUI.transform.Find("Screens").Find("GeneralInfo").Find("Viewport").Find("Content").Find("Family Name").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_description = canvasUI.transform.Find("Screens").Find("GeneralInfo").Find("Viewport").Find("Content").Find("Description").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_medicinal = canvasUI.transform.Find("Screens").Find("GeneralInfo").Find("Viewport").Find("Content").Find("Medicinal").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_moleAname = canvasUI.transform.Find("Screens").Find("MoleA").Find("Viewport").Find("Content").Find("Mole A Name").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_moleAclass = canvasUI.transform.Find("Screens").Find("MoleA").Find("Viewport").Find("Content").Find("Mole A Class").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_moleBname = canvasUI.transform.Find("Screens").Find("MoleB").Find("Viewport").Find("Content").Find("Mole B Name").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_moleBclass = canvasUI.transform.Find("Screens").Find("MoleB").Find("Viewport").Find("Content").Find("Mole B Class").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_moleCname = canvasUI.transform.Find("Screens").Find("MoleC").Find("Viewport").Find("Content").Find("Mole C Name").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_moleCclass = canvasUI.transform.Find("Screens").Find("MoleC").Find("Viewport").Find("Content").Find("Mole C Class").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_hardiness = canvasUI.transform.Find("Screens").Find("MoreInfo").Find("Viewport").Find("Content").Find("HardinessZones").gameObject.GetComponent<TextMeshProUGUI>();
        // Assign Hardiness Maps
        var x = 0;
        foreach (Transform child in canvasUI.transform.Find("Screens").Find("MoreInfo").Find("Viewport").Find("Content").Find("HardinessMaps"))
        {
            if (x != 0) hardinessImages[x - 1] = child.gameObject;
            x++;
        }
        CreateModels();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name != "FontSize")
        {
            // Check if current state does not equal desired state
            if (rend.enabled) DisplayModel(canvasUI.transform.Find("UIManager").GetComponent<UIManager>().activePanel);
            if ((current != rend.enabled) && rend.enabled) Isenabled();
            else if ((current != rend.enabled) && !rend.enabled) Isdisabled();
        }
    }
    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS
    // If Model Is Being Tracked
    public void Isenabled()
    {
        // Set all displays to show current plant's information
        tmp_comName.text = "<b>Common Name: </b>" + comName;
        tmp_sciName.text = "<b>Scientific Name: </b><i>" + sciName + "</i>";
        tmp_fam.text = "<b>Family Name: </b>" + family;
        tmp_description.text = "<b>Description: </b>" + description;
        tmp_medicinal.text = "<b>Medicinal: </b>" + medicinal;
        tmp_hardiness.text = string.Format("<b>Hardiness Zones:</b> {0}-{1}",hardiness[0], hardiness[1]);
        
        string[] test = Link(moleAclass); // check to see if moleAclass has a link attached to it
        tmp_moleAname.text = "<b>Molecule Name: </b>" + moleAname;
        if (test[1] != "") tmp_moleAclass.text = "<b>Class Name: </b><color=blue><i><u>" + test[0] + "</u></i></color>";// if a link is found then indicate hyperlink by making text blue and underlined
        else tmp_moleAclass.text = "<b>Class Name: </b>" + test[0]; // if no link is provided, display text normally
        tmp_moleAclass.transform.GetChild(0).GetComponent<ClickableText>().link = test[1];
        
        test = Link(moleBclass); // check to see if moleBclass has a link attached to it
        tmp_moleBname.text = "<b>Molecule Name: </b>" + moleBname; 
        if (test[1] != "") tmp_moleBclass.text = "<b>Class Name: </b><color=blue><i><u>" + test[0] + "</u></i></color>";// if a link is found then indicate hyperlink by making text blue and underlined
        else tmp_moleBclass.text = "<b>Class Name: </b>" + test[0]; // if no link is provided, display text normally
        tmp_moleBclass.transform.GetChild(0).GetComponent<ClickableText>().link = test[1];
        
        test = Link(moleCclass); // check to see if moleCclass has a link attached to it
        tmp_moleCname.text = "<b>Molecule Name: </b>" + moleCname;
        if (test[1] != "") tmp_moleCclass.text = "<b>Class Name: </b><color=blue><i><u>" + test[0] + "</u></i></color>"; // if a link is found then indicate hyperlink by making text blue and underlined
        else tmp_moleCclass.text = "<b>Class Name: </b>" + test[0]; // if no link is provided, display text normally
        tmp_moleCclass.transform.GetChild(0).GetComponent<ClickableText>().link = test[1];
        
        // Only display hardiness maps related to current plant
        var temp = hardiness[0];
        if (temp > 0)
        {
            while (temp <= hardiness[1])
            {
                hardinessImages[temp - 1].SetActive(true);
                temp++;
            }
        }
            hardinessImages[13].SetActive(true);
        GUIEnabled();
        current = true; // update current state

        // Create Extra Links
        var textSize = canvasUI.transform.Find("UIManager").GetComponent<UIManager>().textSize;
        foreach (string link in links)
        {
            var linkModel = Instantiate(linkPrefab, Vector3.zero, Quaternion.identity);
            _links.Add(linkModel);
            linkModel.GetComponent<TextMeshProUGUI>().text = string.Format("<color=blue><i><u>{0}</u></i></color>", link);
            linkModel.transform.SetParent(canvasUI.transform.Find("Screens").Find("MoreInfo").Find("Viewport").Find("Content"));
            linkModel.transform.GetChild(0).GetComponent<ClickableText>().link = link;
            linkModel.name = link;
            if (textSize != 0) linkModel.GetComponent<TextMeshProUGUI>().fontSize = textSize;
            else linkModel.GetComponent<TextMeshProUGUI>().fontSize = 50.0f;
            linkModel.transform.localScale = Vector3.one;
            canvasUI.transform.Find("UIManager").GetComponent<UIManager>().tmpComponents.Add(linkModel.GetComponent<TextMeshProUGUI>());
        }
    }

    // Display a particular model from the plant
    public void DisplayModel(string str)
    {
        if (plantModel != null && str == "gen_info") plantModel.GetComponent<MeshRenderer>().enabled = true;
        else if (plantModel != null) plantModel.GetComponent<MeshRenderer>().enabled = false;
        if (moleAmodel != null && str == "moleA") moleAmodel.GetComponent<MeshRenderer>().enabled = true;
        else if (moleAmodel != null) moleAmodel.GetComponent<MeshRenderer>().enabled = false;
        if (moleBmodel != null && str == "moleB") moleBmodel.GetComponent<MeshRenderer>().enabled = true;
        else if (moleBmodel != null) moleBmodel.GetComponent<MeshRenderer>().enabled = false;
        if (moleCmodel != null && str == "moleC") moleCmodel.GetComponent<MeshRenderer>().enabled = true;
        else if (moleCmodel != null) moleCmodel.GetComponent<MeshRenderer>().enabled = false;
    }
    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS
    private GameObject CreateModel(GameObject obj, string str)
    {
        obj = Instantiate(obj, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.name = str;
        obj.GetComponent<LockModel>().canvasUI = canvasUI;
        return obj;
    }
    // If Model Is Not Begin Tracked
    private void Isdisabled()
    {
        // set all displays to default messages
        tmp_comName.text = "<b>Common Name: </b>";
        tmp_sciName.text = "<b>Scientific Name: </b>";
        tmp_fam.text = "<b>Family Name: </b>";
        tmp_description.text = "<b>Description: </b>";
        tmp_medicinal.text = "<b>Medicinal: </b>";
        tmp_moleAname.text = "<b>Molecule Name: </b>";
        tmp_moleAclass.text = "<b>Class Name: </b>";
        tmp_moleBname.text = "<b>Molecule Name: </b>";
        tmp_moleBclass.text = "<b>Class Name: </b>";
        tmp_moleCname.text = "<b>Molecule Name: </b>";
        tmp_moleCclass.text = "<b>Class Name: </b>";
        tmp_hardiness.text = "<b>Hardiness Zones: </b>";
        foreach (GameObject obj in hardinessImages)
        {
            obj.SetActive(false);
        }
        if (_links.Count != 0)
        {
            foreach (GameObject link in _links)
            {
                Destroy(link);
            }
        }
        DisplayModel("");
        GUIDisabled();
        current = false; // update current state
    }

    // When Model Is Being Tracked
    private void GUIEnabled()
    {
        int count = 0;
        int totalButtons = 0;
        buttons = new List<string>();
        // Check for general information
        if ((comName != null && comName != "") || (sciName != null && sciName != "") || (family != null && family != "") || (description != null && description != "") || (medicinal != null && medicinal != ""))
        {
            buttons.Add("GeneralInfo_Button");
            totalButtons++;
        }
        // Check for molecule A
        if (moleAname != null && moleAclass != null && moleAname != "" && moleAclass != "")
        {
            buttons.Add("MoleA_Button");
            totalButtons++;
        }
        // Check for molecule B
        if (moleBname != null && moleBclass != null && moleBname != "" && moleBclass != "")
        {
            buttons.Add("MoleB_Button");
            totalButtons++;
        }
        // Check for molecule C
        if (moleCname != null && moleCclass != null && moleCname != "" && moleCclass != "")
        {
            buttons.Add("MoleC_Button");
            totalButtons++;
        }
        // Check for more information
        if (toxicity != 0 || hardiness[0] != -1)
        {
            buttons.Add("MoreInfo_Button");
            totalButtons++;
        }
        foreach(string button in buttons)
        {
            GameObject currButton = canvasUI.transform.Find("MainPanel").Find(button).gameObject;
            currButton.transform.localPosition = new Vector3((0 + ((-80) * (totalButtons-1)) + (160 * count)), 0.0f, 0.0f);
            count++;
            currButton.GetComponent<Animator>().Play("ButtonUp_anim");
        }
        canvasUI.transform.Find("ToggleLock").gameObject.SetActive(true);
        canvasUI.transform.Find("ToggleLock").gameObject.GetComponent<SwitchSprite>().cleanUp();
    }

    private void GUIDisabled()
    {
        foreach (Transform button in canvasUI.transform.Find("MainPanel").transform)
        {
            if (button.name != "Options_Button" && button.transform.position.y > -100)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = button.GetComponent<GUIDisplay>().Buttontext;
                button.GetComponent<Image>().color = button.GetComponent<GUIDisplay>().Button_color_normal;
                button.GetComponent<Animator>().Play("ButtonDown_anim");
            }
        }
        canvasUI.transform.Find("ToggleLock").GetComponent<SwitchSprite>().cleanUp();
        canvasUI.transform.Find("ToggleLock").gameObject.SetActive(false);
        canvasUI.transform.Find("UIManager").GetComponent<UIManager>().CleanUp();
        canvasUI.transform.Find("MainPanel").GetChild(0).GetComponent<GUIDisplay>().CleanUp();
    }

    private string[] Link(string name)
    {
        // break a string into two components the text to be shown and the website URL to be navigated to once clicked
        bool check = false;
        string[] title = new string[2];
        title[0] = ""; // text to be displayed for the hyperlink
        title[1] = ""; // URL destination once clicked
        foreach (char ch in name)
        {
            if (ch == '[' || ch == ']') check = !check;
            else title[check ? 1 : 0] += ch;
        }
        return title;
    }

    // If A Model Is Provided Then Create It
    private void CreateModels()
    {
        if (plantModel != null) // create plant model if one is provided
        {
            plantModel = CreateModel(plantModel, "Plant Model : " + comName);
            plantModel.transform.parent = plantModel.transform.parent.parent;
            plantModel.transform.localScale = Vector3.one;

        }
        if (moleAmodel != null) // create moleA model if one is provided
        {
            moleAmodel = CreateModel(moleAmodel, "Mole A Model : " + moleAname);
            moleAmodel.transform.parent = moleAmodel.transform.parent.parent;
            moleAmodel.transform.localScale = Vector3.one;
        }
        if (moleBmodel != null) // create moleB model if one is provided
        {
            moleBmodel = CreateModel(moleBmodel, "Mole B Model : " + moleBname);
            moleBmodel.transform.parent = moleBmodel.transform.parent.parent;
            moleBmodel.transform.localScale = Vector3.one;
        }
        if (moleCmodel != null) // create moleC model if one is provided
        {
            moleCmodel = CreateModel(moleCmodel, "Mole C Model : " + moleCname);
            moleCmodel.transform.parent = moleCmodel.transform.parent.parent;
            moleCmodel.transform.localScale = Vector3.one;
        }
    }
    #endregion // PRIVATE_METHODS
}

#region DISPLAY_EDITOR
#if UNITY_EDITOR
// Edit the look of the code in the inspector
[CustomEditor (typeof(Plant))]
[CanEditMultipleObjects]
public class PlantEditor : Editor
{
    // Initialize Varialbes
    bool showDesc = false; // showing the description?
    bool showInfo = true; // showing any information?
    bool showMed = false; // showing the medicinal section?

    // In the inspector
    public override void OnInspectorGUI()
    {
        Plant model = (Plant)target;
        model.canvasUI = (Canvas)EditorGUILayout.ObjectField("Canvas", model.canvasUI, typeof(Canvas), true);
        showInfo = EditorGUILayout.Foldout(showInfo, "Input Information", EditorStyles.foldout);
        if (showInfo)
        {
            // Common Name
            if (model.comName != null && model.comName != "")
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 65;
                model.comName = EditorGUILayout.TextField("ComName", model.comName, GUILayout.MaxWidth(180));
                EditorGUIUtility.labelWidth = 45;
                if (model.tmp_comName != null) model.tmp_comName = (TextMeshProUGUI)EditorGUILayout.ObjectField("Display", model.tmp_comName, typeof(TextMeshProUGUI), true);
                else GUILayout.Label("     Display Not Provided");
                EditorGUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = 70;
                if (model.plantModel != null) model.plantModel = (GameObject)EditorGUILayout.ObjectField("Plant Model", model.plantModel, typeof(GameObject), true);
                EditorGUILayout.Space();
            }

            // Scientific Name
            if (model.sciName != null && model.sciName != "")
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 65;
                model.sciName = EditorGUILayout.TextField("SciName", model.sciName, GUILayout.MaxWidth(180));
                EditorGUIUtility.labelWidth = 45;
                if (model.tmp_sciName != null) model.tmp_sciName = (TextMeshProUGUI)EditorGUILayout.ObjectField("Display", model.tmp_sciName, typeof(TextMeshProUGUI), true);
                else GUILayout.Label("     Display Not Provided");
                EditorGUILayout.EndHorizontal();
            }

            // Family
            if (model.family != null && model.family != "")
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 65;
                model.family = EditorGUILayout.TextField("Family", model.family, GUILayout.MaxWidth(180));
                EditorGUIUtility.labelWidth = 45;
                if (model.tmp_fam != null) model.tmp_fam = (TextMeshProUGUI)EditorGUILayout.ObjectField("Display", model.tmp_fam, typeof(TextMeshProUGUI), true);
                else GUILayout.Label("     Display Not Provided");
                EditorGUILayout.EndHorizontal();
            }

            // Description
            if (model.description != null && model.description != "")
            {
                showDesc = EditorGUILayout.Foldout(showDesc, "Description");
                EditorStyles.textArea.wordWrap = true;
                if (showDesc) model.description = EditorGUILayout.TextArea(model.description, EditorStyles.textArea, GUILayout.Width(350), GUILayout.ExpandWidth(true));
            }

            // Molecules
            if (model.moleAname != null && model.moleAclass != null && model.moleAname != "" && model.moleAclass != "")
            {
                GUILayout.Label("                                              Molecules");
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 65;
                model.moleAname = EditorGUILayout.TextField("A: Name", model.moleAname, GUILayout.MaxWidth(200));
                EditorGUIUtility.labelWidth = 45;
                model.moleAclass = EditorGUILayout.TextField("Class", model.moleAclass, GUILayout.MaxWidth(180));
                EditorGUILayout.EndHorizontal();
                if (model.moleAmodel != null) model.moleAmodel = (GameObject)EditorGUILayout.ObjectField("Model", model.moleAmodel, typeof(GameObject), true);
                EditorGUILayout.Space();
            }
            else GUILayout.Label("Molecules: Not Provided");

            if (model.moleBname != null && model.moleBclass != null && model.moleBname != "" && model.moleBclass != "")
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 65;
                model.moleBname = EditorGUILayout.TextField("B: Name", model.moleBname, GUILayout.MaxWidth(200));
                EditorGUIUtility.labelWidth = 45;
                model.moleBclass = EditorGUILayout.TextField("Class", model.moleBclass, GUILayout.MaxWidth(180));
                EditorGUILayout.EndHorizontal();
                if (model.moleBmodel != null) model.moleBmodel = (GameObject)EditorGUILayout.ObjectField("Model", model.moleBmodel, typeof(GameObject), true);
                EditorGUILayout.Space();
            }

            if (model.moleCname != null && model.moleCclass != null && model.moleCname != "" && model.moleCclass != "")
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 65;
                model.moleCname = EditorGUILayout.TextField("C: Name", model.moleCname, GUILayout.MaxWidth(200));
                EditorGUIUtility.labelWidth = 45;
                model.moleCclass = EditorGUILayout.TextField("Class", model.moleCclass, GUILayout.MaxWidth(180));
                EditorGUILayout.EndHorizontal();
                if (model.moleCmodel != null) model.moleCmodel = (GameObject)EditorGUILayout.ObjectField("Model", model.moleCmodel, typeof(GameObject), true);
                EditorGUILayout.Space();
            }

            // Medicinal
            if (model.medicinal != null && model.medicinal != "")
            {
                showDesc = EditorGUILayout.Foldout(showDesc, "Medicinal");
                EditorStyles.textArea.wordWrap = true;
                if (showMed) model.medicinal = EditorGUILayout.TextArea(model.medicinal, EditorStyles.textArea, GUILayout.Width(350), GUILayout.ExpandWidth(true));
            }

            // Toxicity
            GUILayout.Label("Toxicity: " + model.toxicity);

            // Hardiness
            // if ((model.hardiness[0] >= 0) && (model.hardiness[1] >= 0) && model.hardiness != null) GUILayout.Label("Hardiness: " + model.hardiness[0] + "-" + model.hardiness[1]);

            // Extra Links
            GUILayout.Label("Extra Links:");
            for (int i = 0; i < model.links.Length; i++)
            {
                GUILayout.Label(model.links[i]);
            }
        }
    }
}
#endif
#endregion // DISPLAY_EDITOR