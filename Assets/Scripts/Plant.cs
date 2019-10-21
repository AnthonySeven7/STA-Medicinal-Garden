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

    // Information to be displayed
    public string comName;
    public string sciName;
    public string family;
    [Multiline]
    public string description;
    
    // Locations where information is being displayed
    public TextMeshProUGUI tmp_comName;
    public TextMeshPro tmp_sciName;
    public TextMeshPro tmp_fam;
    
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
        Debug.Log("Enabled"); // Output to console

        //Set all displays to show current plant's information
        canvasUI.transform.Find("Common Name").GetComponent<Text>().text = "Common Name: " + comName;
        tmp_comName.text = comName;
        canvasUI.transform.Find("Scientific Name").GetComponent<Text>().text = "Scientific Name: " + sciName;
        //tmp_sciName.text = sciName;  [Not yet implemented]
        canvasUI.transform.Find("Family Name").GetComponent<Text>().text = "Family Name: " + family;
        tmp_fam.text = family;
        canvasUI.transform.Find("Description").GetComponent<Text>().text = "Description: " + description;
        current = true; // Update current state
    }

    // If the model is not being tracked
    public void Isdisabled()
    {
        Debug.Log("Diabled"); // Output to console

        // Set all displays to their default messages
        canvasUI.transform.Find("Common Name").GetComponent<Text>().text = "Common Name: ";
        canvasUI.transform.Find("Scientific Name").GetComponent<Text>().text = "Scientific Name: ";
        canvasUI.transform.Find("Family Name").GetComponent<Text>().text = "Family Name: ";
        canvasUI.transform.Find("Description").GetComponent<Text>().text = "Description: ";
        tmp_fam.text = "";
        current = false; // Update current state
    }
}

//#if UNITY_EDITOR
// Edit the look of the code in the inspector
[CustomEditor (typeof(Plant))]
[CanEditMultipleObjects]
public class PlantEditor : Editor
{
    // Initialize Varialbes
    bool showDesc = false; // showing the description?
    bool showInfo = true; // showing any information?

    // In the inspector
    public override void OnInspectorGUI()
    {
        Plant model = (Plant)target;
        model.canvasUI = (Canvas)EditorGUILayout.ObjectField("Canvas", model.canvasUI, typeof(Canvas), true);
        showInfo = EditorGUILayout.Foldout(showInfo, "Input Information", EditorStyles.foldout);
        if (showInfo)
        {
            // Common Name
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 65;
            model.comName = EditorGUILayout.TextField("ComName", model.comName, GUILayout.MaxWidth(180));
            EditorGUIUtility.labelWidth = 45;
            model.tmp_comName = (TextMeshProUGUI)EditorGUILayout.ObjectField("Display", model.tmp_comName, typeof(TextMeshProUGUI), true);
            EditorGUILayout.EndHorizontal();

            // Scientific Name
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 65;
            model.sciName = EditorGUILayout.TextField("SciName", model.sciName, GUILayout.MaxWidth(180));
            EditorGUIUtility.labelWidth = 45;
            model.tmp_sciName = (TextMeshPro)EditorGUILayout.ObjectField("Display", model.tmp_sciName, typeof(TextMeshPro), true);
            EditorGUILayout.EndHorizontal();

            // Family
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 65;
            model.family = EditorGUILayout.TextField("Family", model.family, GUILayout.MaxWidth(180));
            EditorGUIUtility.labelWidth = 45;
            model.tmp_fam = (TextMeshPro)EditorGUILayout.ObjectField("Display", model.tmp_fam, typeof(TextMeshPro), true);
            EditorGUILayout.EndHorizontal();

            // Description
            showDesc = EditorGUILayout.Foldout(showDesc, "Description");
            EditorStyles.textArea.wordWrap = true;
            if (showDesc) model.description = EditorGUILayout.TextArea(model.description, EditorStyles.textArea, GUILayout.Width(350), GUILayout.ExpandWidth(true));
        }
    }
}
//#endif