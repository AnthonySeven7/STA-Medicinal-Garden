using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Vuforia;

public class Information : MonoBehaviour
{
    void Start()
    {
        // Get a list of all of the plants with all of their information
        List<string[]> plants = readFile();
        Debug.Log("File Read : True");
        Debug.Log("Instances Created : " + setInformation(plants));


    }

    List<string[]> readFile()
    {
        // Initiate Variables
        List<string[]> plants = new List<string[]>(); // A list to place each plant into
        StreamReader fileData = new StreamReader("Assets/Text/plant_test.csv"); // File to be open which contains all of the plants
        bool cont = true; // Whether or not to continue the process
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
                    if (ch != ',' && ("" + ch) != "\n") data += ch; // If the current character is either neither a comma or new line then add it to the string
                    else
                    {
                        entry[index] = data; // Store the information
                        data = ""; // Reset the variable
                        index++; // Increment the index
                    }
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

    bool setInformation(List<string[]> plants)
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

        for (int i = 0; i < plants.Count; i++) // For each plant in the list
        {
            if (!test) return test; // If there's an error then end the task with a false
            else
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
                if (!int.TryParse(plants[i][10], out toxicity)) Debug.LogError("ERROR : " + comName + "'s toxicity non-convertable : '" + plants[i][10] + "'"); // If the toxicity is not in the form ex:"123", then display an error message
                description = plants[i][11];
                if (!int.TryParse(plants[i][12], out hardiness)) Debug.LogError("ERROR : " + comName + "'s hardiness non-convertable : '" + plants[i][12] + "'"); // If the hardiness is not in the form ex:"123", then display an error message
            }
        }
        return test;
    }
}
