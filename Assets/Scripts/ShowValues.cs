using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowValues : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Plant plant = this.transform.parent.parent.parent.parent.parent.GetChild(0).GetComponent<Plant>();
        if (this.name == "Common Name")
        {
            this.GetComponent<TextMeshProUGUI>().text = "Common Name: " + plant.comName;
            plant.tmp_comName = this.transform.gameObject.GetComponent<TextMeshProUGUI>();
        }
        else if (this.name == "Scientific Name")
        {
            this.GetComponent<TextMeshProUGUI>().text = "Scientific Name: <i>" + plant.sciName + "</i>";
            plant.tmp_sciName = this.transform.gameObject.GetComponent<TextMeshProUGUI>();
        }
        else if (this.name == "Family")
        {
            this.GetComponent<TextMeshProUGUI>().text = "Family: " + plant.family;
            plant.tmp_fam = this.transform.gameObject.GetComponent<TextMeshProUGUI>();
        }
        else if (this.name == "Description")
        {
            this.GetComponent<TextMeshProUGUI>().text = "Description: " + plant.description;
            plant.tmp_description = this.transform.gameObject.GetComponent<TextMeshProUGUI>();
        }
    }
}
