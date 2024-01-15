using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class testGenerate : MonoBehaviour
{
    public GameObject linePrefab;
    public RectTransform canvasRect;
    public float LineSpacing = 0f;
    public Text BPMSpeed;
    public Text MIDIdata;
    public Text Lyricsdata;
    public float SongLength;
    private float moveSpeed;

    private List<List<float>> midiData = new List<List<float>>();
    private List<string> lyricsData = new List<string>();
    private List<GameObject> spawnedLines = new List<GameObject>();
    private float space = 0f;

    void Start()
    {
        moveSpeed = float.Parse(BPMSpeed.text);

        LoadMidiData();
        ParseLyricsData();

        SpawnAllLines();

        StartCoroutine(AnimateLines());
    }

    //Midi
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
                }
                midiData.Add(data);
            }
        }
    }

    void ParseLyricsData()
    {
        if(Lyricsdata != null)
        {
            List<string> timestamps = ExtractTimestamps(Lyricsdata.text);

            for (int i = 0; i < timestamps.Count - 1; i++)
            {
                float duration = CalculateDuration(timestamps[i], timestamps[i + 1]);
                Debug.Log($"{timestamps[i]}, {duration:F2}, {timestamps[i + 1]}");
            }

            lyricsData = timestamps;
        }
    }

    List<string> ExtractTimestamps(string lyrics)
    {
        List<string> timestamps = new List<string>();
        string pattern = @"\[\d{2}:\d{2}\.\d{2}\]";
        MatchCollection matches = Regex.Matches(lyrics, pattern);

        foreach (Match match in matches)
        {
            timestamps.Add(match.Value);
        }

        return timestamps;
    }

    float CalculateDuration(string timestamp1, string timestamp2)
    {
        TimeSpan time1 = ExtractTimeSpan(timestamp1);
        TimeSpan time2 = ExtractTimeSpan(timestamp2);
        float duration = (float)(time2.TotalSeconds - time1.TotalSeconds);
        return duration;
    }

    TimeSpan ExtractTimeSpan(string timestamp)
    {
        string[] parts = timestamp.Trim('[', ']').Split(':');
        int minutes = int.Parse(parts[0]);
        float seconds = float.Parse(parts[1]);
        return new TimeSpan(0, 0, minutes, (int)seconds, (int)((seconds % 1) * 1000));
    }


    void SpawnAllLines()
    {
        foreach (List<float> lineData in midiData)
        {
            foreach (float value in lineData)
            {
                GameObject line = Instantiate(linePrefab, canvasRect);

                RectTransform lineRect = line.GetComponent<RectTransform>();
                lineRect.anchoredPosition = new Vector2((canvasRect.sizeDelta.x / 2) + space, (((value * (canvasRect.sizeDelta.y / 2)) / 55) - 2000));

                spawnedLines.Add(line);

                space += LineSpacing;
            }
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

                if (lineRect.anchoredPosition.x <= -space)
                {
                    Destroy(line);
                }
            }

            yield return null;
        }
    }
}