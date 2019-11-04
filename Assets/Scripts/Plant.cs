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
    public int toxicity;
    public int hardiness;

    // Locations where information is being displayed
    public TextMeshProUGUI tmp_comName = null;
    public TextMeshPro tmp_sciName = null;
    public TextMeshPro tmp_fam = null;
    
    // Test variable to prevent overlooping
    private bool current = true;   

    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<MeshRenderer>();
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
        canvasUI.transform.Find("Common Name").GetComponent<Text>().text = "Common Name: " + comName;
        if (tmp_comName != null) tmp_comName.text = comName;
        canvasUI.transform.Find("Scientific Name").GetComponent<Text>().text = "Scientific Name: <i>" + sciName + "</i>";
        if (tmp_sciName != null) tmp_sciName.text = sciName;
        canvasUI.transform.Find("Family Name").GetComponent<Text>().text = "Family Name: " + family;
        if (tmp_fam != null) tmp_fam.text = family;
        canvasUI.transform.Find("Description").GetComponent<Text>().text = "Description: " + description;
        current = true; // Update current state
    }

    // If the model is not being tracked
    public void Isdisabled()
    {
        // Set all displays to their default messages
        canvasUI.transform.Find("Common Name").GetComponent<Text>().text = "Common Name: ";
        canvasUI.transform.Find("Scientific Name").GetComponent<Text>().text = "Scientific Name: ";
        canvasUI.transform.Find("Family Name").GetComponent<Text>().text = "Family Name: ";
        canvasUI.transform.Find("Description").GetComponent<Text>().text = "Description: ";
        current = false; // Update current state
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
                if (model.tmp_sciName != null) model.tmp_sciName = (TextMeshPro)EditorGUILayout.ObjectField("Display", model.tmp_sciName, typeof(TextMeshPro), true);
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
                if (model.tmp_fam != null) model.tmp_fam = (TextMeshPro)EditorGUILayout.ObjectField("Display", model.tmp_fam, typeof(TextMeshPro), true);
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