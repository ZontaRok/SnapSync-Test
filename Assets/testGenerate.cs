using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testGenerate : MonoBehaviour
{
    public GameObject linePrefab;
    public RectTransform canvasRect;
    public float LineSpacing = 0f;
    public Text LyricsData;
    public Text MIDIdata;
    public Text BPMSpeed;

    private float moveSpeed;
    private float songDuration = 227.85f;
    private List<float> lyricsTimestamps = new List<float>();
    private List<List<float>> midiData = new List<List<float>>();

    private List<GameObject> spawnedLines = new List<GameObject>();
    private float space = 0f;

    void Start()
    {
        moveSpeed = float.Parse(BPMSpeed.text);

        LoadLyricsTimestamps();
        LoadMidiData();
        SpawnAllLines();

        StartCoroutine(AnimateLines());
    }

    void LoadLyricsTimestamps()
    {
        if (LyricsData != null)
        {
            string[] lines = LyricsData.text.Split('\n');
            foreach (string line in lines)
            {
                // Extract the timestamp from the line
                string timestampStr = line.Substring(1, 8);
                if (TimeSpan.TryParseExact(timestampStr, "mm':'ss'.'ff", null, out TimeSpan timestamp))
                {
                    float timestampInSeconds = (float)timestamp.TotalSeconds;
                    lyricsTimestamps.Add(timestampInSeconds);
                }
                else
                {
                    Debug.LogError("Failed to parse timestamp: " + timestampStr);
                }
            }

            Debug.Log("Loaded " + lyricsTimestamps.Count + " lyrics timestamps.");
        }
        else
        {
            Debug.LogError("LyricsData is null.");
        }
    }

    void LoadMidiData()
    {
        if (MIDIdata != null)
        {
            string[] lines = MIDIdata.text.Split('\n');
            foreach (string line in lines)
            {
                string[] values = line.Trim().Split(',');
                List<float> data = new List<float>();
                foreach (string value in values)
                {
                    if (float.TryParse(value, out float floatValue))
                    {
                        data.Add(floatValue);
                    }
                    else
                    {
                        Debug.LogError("Failed to parse MIDI value: " + value);
                    }
                }
                midiData.Add(data);
            }

            Debug.Log("Loaded " + midiData.Count + " sets of MIDI data.");
        }
        else
        {
            Debug.LogError("MIDIdata is null.");
        }
    }

    void SpawnAllLines()
    {
        for (int i = 0; i < Mathf.Min(lyricsTimestamps.Count, midiData.Count); i++)
        {
            GameObject line = Instantiate(linePrefab, canvasRect);

            RectTransform lineRect = line.GetComponent<RectTransform>();

            // Calculate the normalized position based on the song duration
            float normalizedPosition = lyricsTimestamps[i] / songDuration;
            lineRect.anchoredPosition = new Vector2((canvasRect.sizeDelta.x * normalizedPosition) + space, 0f);

            // Use MIDI data for Y position
            if (midiData[i].Count > 0)
            {
                float midiY = midiData[i][0];
                lineRect.anchoredPosition += new Vector2(0f, midiY);

                Debug.Log("Spawned line at timestamp " + lyricsTimestamps[i] + " with Y position " + midiY);
            }
            else
            {
                Debug.LogError("MIDI data is empty for line at timestamp " + lyricsTimestamps[i]);
            }

            spawnedLines.Add(line);

            space += LineSpacing;
        }
    }

    IEnumerator AnimateLines()
    {
        while (true)
        {
            foreach (GameObject line in spawnedLines)
            {
                RectTransform lineRect = line.GetComponent<RectTransform>();

                float newX = lineRect.anchoredPosition.x - moveSpeed * Time.deltaTime;
                lineRect.anchoredPosition = new Vector2(newX, lineRect.anchoredPosition.y);

                // You can add logic to destroy lines when they are no longer visible if needed

                yield return null;
            }
        }
    }
}
