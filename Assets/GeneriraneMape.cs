using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GeneriraneMape : MonoBehaviour
{
    public List<float> noteDurations = new List<float>();

    // Prefab with a UI Image component to act as a line
    public GameObject linePrefab;

    public RectTransform canvasTransform; // Drag your Canvas RectTransform here

    public string midiFilePath = "MIDIdata.txt";

    void Start()
    {
        LoadMidiData();
        GenerateLinesOnCanvas();
    }

    void LoadMidiData()
    {
        string[] lines = File.ReadAllLines(Path.Combine(Application.dataPath, midiFilePath));

        foreach (string line in lines)
        {
            string[] values = line.Split(',');

            foreach (string value in values)
            {
                if (float.TryParse(value.Trim(), out float duration))
                {
                    noteDurations.Add(duration);
                }
                else
                {
                    Debug.LogWarning("Failed to parse value: " + value);
                }
            }
        }
    }

    void GenerateLinesOnCanvas()
    {
        float maxY = Mathf.Max(noteDurations.ToArray());
        float minY = Mathf.Min(noteDurations.ToArray());

        float canvasHeight = Mathf.Abs(maxY - minY);
        canvasTransform.sizeDelta = new Vector2(canvasTransform.sizeDelta.x, canvasHeight);

        Vector3 startPosition = Vector3.zero;

        foreach (float duration in noteDurations)
        {
            GameObject lineInstance = Instantiate(linePrefab, canvasTransform);

            // Check if RectTransform exists on the instantiated prefab
            RectTransform lineRectTransform = lineInstance.GetComponent<RectTransform>();
            if (lineRectTransform == null)
            {
                Debug.LogError("RectTransform not found on LinePrefab! Please attach a RectTransform to the LinePrefab.");
                return;
            }

            // Set line width based on duration (adjust this as needed)
            lineRectTransform.sizeDelta = new Vector2(duration, 2f);

            // Set line position
            lineRectTransform.localPosition = startPosition + new Vector3(duration / 2, 0, 0);

            // Move the starting position for the next line
            startPosition = lineRectTransform.localPosition + new Vector3(duration / 2, 0, 0);
        }
    }
}
