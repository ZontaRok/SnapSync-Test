using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GeneriraneMape : MonoBehaviour
{
    public GameObject linePrefab; // Reference to a GameObject prefab that will serve as a vertical line
    public Transform lineParent; // Parent transform to hold the lines

    private List<float> dataPoints = new List<float>();

    private void Start()
    {
        LoadDataFromFile("Assets/pitch_data_lyrics_Song.txt");

        StartCoroutine(DrawLines());
    }

    void LoadDataFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath); // Read all lines from the file

        if (lines.Length == 0)
        {
            Debug.LogError("No data found in the file!");
            return;
        }

        foreach (string line in lines)
        {
            string[] dataStrings = line.Split(','); // Split each line by commas to get individual data points

            foreach (string dataString in dataStrings)
            {
                if (float.TryParse(dataString, out float dataPoint))
                {
                    dataPoints.Add(dataPoint);
                }
                else
                {
                    Debug.LogError("Failed to parse a data point: " + dataString);
                }
            }
        }
    }

    IEnumerator DrawLines()
    {
        for (int i = 0; i < dataPoints.Count; i++)
        {
            // Calculate the y position based on data point and game area
            float yPos = 56 + (dataPoints[i] - 50); // Adjust as necessary based on your requirements

            // Instantiate a new vertical line prefab
            GameObject lineInstance = Instantiate(linePrefab, new Vector3(i, yPos, 0), Quaternion.identity);

            // Set the parent of the instantiated line to lineParent (if you want to organize them under a parent GameObject)
            if (lineParent != null)
            {
                lineInstance.transform.SetParent(lineParent);
            }

            // Wait for 1 second before drawing the next line
            yield return new WaitForSeconds(1.0f);
        }
    }
}
