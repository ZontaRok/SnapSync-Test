using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GeneriraneMape : MonoBehaviour
{
    public string filePath = "Assets/pitch_data_lyrics_Song.txt";
    public float minY = 56.0f;
    public float maxY = 106.0f;
    public float CanvesScaleer = 0.5f;
    public float lineWidth = 10.0f;
    public float lineLength = 100.0f;

    public GameObject linePrefab;
    public RectTransform canvasRect;

    void Start()
    {
        List<float> heights = ReadHeightsFromFile(filePath);

        // Initialize the starting X position to the rightmost position of the Canvas
        float currentXPosition = canvasRect.rect.width;

        foreach (float height in heights)
        {
            if (height >= minY && height <= maxY)
            {
                DrawLine(new Vector2(currentXPosition, height));
            }
            // Move to the left for the next line
            currentXPosition -= lineLength;
        }
    }

    List<float> ReadHeightsFromFile(string path)
    {
        List<float> heights = new List<float>();

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                foreach (string value in values)
                {
                    float height;
                    if (float.TryParse(value, out height))
                    {
                        heights.Add(height);
                    }
                    else
                    {
                        Debug.LogWarning("Failed to parse height value: " + value);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("File not found at path: " + path);
        }

        return heights;
    }

    void DrawLine(Vector2 position)
    {
        GameObject line = Instantiate(linePrefab, canvasRect);
        line.GetComponent<RectTransform>().anchoredPosition = position;

        // Ensure the line is within the Canvas height boundaries
        if (position.y < canvasRect.rect.height && position.y > 0)
        {
            line.GetComponent<RectTransform>().sizeDelta = new Vector2(lineLength, 2); // 2 is the thickness, adjust as needed
        }
        else
        {
            // Optionally, you can adjust or ignore lines that are outside the Canvas height boundaries
            Destroy(line);
        }
    }
}
