using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneriranjeMape : MonoBehaviour
{
    public GameObject linePrefab;
    public RectTransform canvasRect;
    public float LineSpacing = 0f;
    public Text BPMSpeed;
    public Text MIDIdata;
    private float moveSpeed;

    private List<List<float>> midiData = new List<List<float>>();
    private List<GameObject> spawnedLines = new List<GameObject>();
    private float space = 0f;

    void Start()
    {
        moveSpeed = float.Parse(BPMSpeed.text);

        LoadMidiData();

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