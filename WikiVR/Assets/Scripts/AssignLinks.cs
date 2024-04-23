using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AssignLinks : MonoBehaviour
{
    public TMP_Text[] bookTexts;
    // Start is called before the first frame update
    private void Start()
    {
        NewLinks();
    }
    public void NewLinks()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/links.txt");
        string fileContent = File.ReadAllText(filePath);
        TextAsset textAsset = new TextAsset(fileContent);

        if (textAsset != null)
        {
            // Split the text into lines
            string[] lines = textAsset.text.Split('\n');

            // Loop through each TMP_Text object
            for (int i = 0; i < bookTexts.Length; i++)
            {
                // Check if the line index is within the range of available lines
                if (i < lines.Length)
                {
                    // Trim to remove any leading or trailing whitespace
                    string line = lines[i].Trim();

                    // Assign the text to the TMP_Text object
                    bookTexts[i].text = line;
                }
                else
                {
                    Debug.LogWarning("Not enough lines in the text file to assign to all TMP_Text objects.");
                    break; // Exit the loop if there are no more lines in the text file
                }
            }
            Debug.Log("New Links added");
        }
        else
        {
            Debug.LogError("Failed to load text file from Resources folder.");
        }
    }


}
