using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class Plant : MonoBehaviour
{
    // Initialize Variables
    public Canvas canvasUI;
    public MeshRenderer rend;

    // Information from the plant
    public string comName = null;
    public string sciName = null;
    public string family = null;
    [Multiline]
    public string description = null;
    public string moleAname = null;
    public string moleAclass = null;
    public string moleBname = null;
    public string moleBclass = null;
    public string moleCname = null;
    public string moleCclass = null;
    public string medicinal = null;
    public int toxicity = -1;
    public int hardiness = 1;

    // Locations where information is being displayed
    public TextMeshProUGUI tmp_comName = null;
    public TextMeshProUGUI tmp_sciName = null;
    public TextMeshProUGUI tmp_fam = null;
    public TextMeshProUGUI tmp_description = null;

    // A list of all the current buttons on the GUI
    public List<string> buttons;
    
    // Test variable to prevent overlooping
    private bool current = true;   

    // Start is called before the first frame update
    void Start()
    {

        rend = this.GetComponent<MeshRenderer>();
        tmp_comName = canvasUI.transform.Find("Screens").Find("GeneralInfo").GetChild(0).GetChild(0).Find("Common Name").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_sciName = canvasUI.transform.Find("Screens").Find("GeneralInfo").GetChild(0).GetChild(0).Find("Scientific Name").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_fam = canvasUI.transform.Find("Screens").Find("GeneralInfo").GetChild(0).GetChild(0).Find("Family Name").gameObject.GetComponent<TextMeshProUGUI>();
        tmp_description = canvasUI.transform.Find("Screens").Find("GeneralInfo").GetChild(0).GetChild(0).Find("Description").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if current state does not equal desired state
        if ((current != rend.enabled) && rend.enabled) Isenabled();
        else if ((current != rend.enabled) && !rend.enabled) Isdisabled();
    }
    
    // If the model is being tracked
    public void Isenabled()
    {
        //Set all displays to show current plant's information
        tmp_comName.text = "<b>Common Name: </b>" + comName;
        tmp_sciName.text = "<b>Scientific Name: </b><i>" + sciName + "</i>";
        tmp_fam.text = "<b>Family Name: </b>" + family;
        tmp_description.text = "<b>Description: </b>" + description;
        GUIEnabled();
        current = true; // Update current state
    }

    // If the model is not being tracked
    public void Isdisabled()
    {
        // Set all displays to their default messages
        tmp_comName.text = "<b>Common Name: </b>";
        tmp_sciName.text = "<b>Scientific Name: </b>";
        tmp_fam.text = "<b>Family Name: </b>";
        tmp_description.text = "<b>Description: </b>";
        GUIDisabled();
        current = false; // Update current state
    }

    public void GUIEnabled()
    {
        int count = 0;
        int totalButtons = 0;
        buttons.Add("Options_Button");
        buttons = new List<string>();
        if ((comName != null && comName != "") || (sciName != null && sciName != "") || (family != null && family != "") || (description != null && description != ""))
        {
            buttons.Add("GeneralInfo_Button");
            totalButtons++;
        }
        if (moleAname != null && moleAclass != null && moleAname != "" && moleAclass != "")
        {
            buttons.Add("MoleA_Button");
            totalButtons++;
        }
        if (moleBname != null && moleBclass != null && moleBname != "" && moleBclass != "")
        {
            buttons.Add("MoleB_Button");
            totalButtons++;
        }
        if (moleCname != null && moleCclass != null && moleCname != "" && moleCclass != "")
        {
            buttons.Add("MoleC_Button");
            totalButtons++;
        }
        if (toxicity != 0 || hardiness != 0)
        {
            buttons.Add("MoreInfo_Button");
            totalButtons++;
        }
        foreach(string button in buttons) // [Change later for animations]
        {
            GameObject currButton = canvasUI.transform.Find("MainPanel").Find(button).gameObject;
            currButton.SetActive(true);
            currButton.transform.position = new Vector3((float)(540-(106*(totalButtons-1))+(206*count)), 0.0f, 0.0f);
            count++;
        }
    }

    public void GUIDisabled()
    {
        foreach (Transform button in canvasUI.transform.Find("MainPanel").transform)
        {
            if (button.name != "Options_Button")
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = button.GetComponent<GUIDisplay>().Buttontext;
                button.GetComponent<Image>().color = button.GetComponent<GUIDisplay>().Button_color_normal;
                button.gameObject.SetActive(false);
            }
        }
    }
}

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
            }

            if (model.moleCname != null && model.moleCclass != null && model.moleCname != "" && model.moleCclass != "")
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 65;
                model.moleCname = EditorGUILayout.TextField("C: Name", model.moleCname, GUILayout.MaxWidth(200));
                EditorGUIUtility.labelWidth = 45;
                model.moleCclass = EditorGUILayout.TextField("Class", model.moleCclass, GUILayout.MaxWidth(180));
                EditorGUILayout.EndHorizontal();
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
            GUILayout.Label("Hardiness: " + model.hardiness);
        }
    }
}
#endif