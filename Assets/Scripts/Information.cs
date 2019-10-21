using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Information : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string[]> plants = readFile();
    }

    public List<string[]> readFile()
    {
        List<string[]> plants = new List<string[]>();
        StreamReader fileData = new StreamReader("Assets/Text/plant_test.csv");
        fileData.ReadLine();
        bool cont = true;
        while (cont)
        {
            string[] entry = new string[13];
            string str = fileData.ReadLine();
            int index = 0;
            string data = "";
            if (str != null)
            {
                foreach (char ch in str)
                {
                    if (ch != ',' && ("" + ch) != "\n") data += ch;
                    else
                    {
                        entry[index] = data;
                        data = "";
                        index++;
                    }
                }
                Debug.Log("Entry: " + entry[0]);
                plants.Add(entry);
            }
            else
            {
                cont = false;
            }
        }
        fileData.Close();
        return plants;
    }
}
