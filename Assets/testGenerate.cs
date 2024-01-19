using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using System.Net.Http;
using System.Threading.Tasks;

public class testGenerate : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject testPrefab;
    public Text Score;
    public Text SongName;
    public RectTransform canvasRect;
    public float LineSpacing = 0f;
    public Text BPMSpeed;
    public Text MIDIdata;
    public Text Lyricsdata;
    public Text SongLength;
    private float moveSpeed;

    private List<List<float>> midiData = new List<List<float>>();
    private List<string> lyricsData = new List<string>();
    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    private List<GameObject> spawnedLines = new List<GameObject>();
    private LineRenderer[] lineRenderers;
    private float space = 0f;
    private float sizeofLine = 0f;

    //https://snapsync-two.vercel.app/api/leaderboardpost?username=John&score=100&songName=ExampleSong
    private const string ApiUrl = "https://snapsync-two.vercel.app/api/leaderboardpost?";

    void Start()
    {
        moveSpeed = float.Parse(BPMSpeed.text);

        LoadMidiData();
        ParseLyricsData();


        RightDurationLine(float.Parse(SongLength.text));

        for (int i = 0; i < lyricsData.Count - 1; i++)
        {
            float duration = CalculateDuration(lyricsData[i], lyricsData[i + 1]);

            // Spawn prefa
            SpawnPrefab(lyricsData[i], lyricsData[i + 1]);
        }

        //SpawnAllLines();

        StartCoroutine(AnimateLines());
        StartCoroutine(AnimatePrefabs());

        FetchAndDisplayMusic();
    }


    async Task FetchAndDisplayMusic()
    {
        string username = "neki";
        string score = Score.text;
        string Songname = SongName.text;
        string YesApiUrl = ApiUrl+ "username=" + username + "&score=" + score + "&songName=" + SongName.text;
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(YesApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Dela api");
                }
                else
                {
                    Debug.Log($"Failed to fetch data. Status code: {response.StatusCode}");
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log($"An error occurred: {ex.Message}");
        }
    }

    //Midi
    void LoadMidiData()
    {
        if (MIDIdata != null)
        {
            string[] lines = MIDIdata.text.Split(';');
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
        if (Lyricsdata != null)
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

    void RightDurationLine(float totalDuration)
    {
        sizeofLine = (totalDuration * float.Parse(BPMSpeed.text));

        Debug.Log(sizeofLine);

        Vector3 spawnPosition = new Vector3((sizeofLine / 2), 700f, 0f);

        GameObject spawnedPrefab = Instantiate(linePrefab, spawnPosition, Quaternion.identity);

        RectTransform bigassline = spawnedPrefab.GetComponent<RectTransform>();

        bigassline.sizeDelta = new Vector2(sizeofLine, 10f);

        bigassline.SetParent(canvasRect, false);

        spawnedPrefabs.Add(spawnedPrefab);

    }

    void SpawnPrefab(string timestamp1, string timestamp2)
    {
        float time1 = (float)ExtractTimeSpan(timestamp1).TotalSeconds;
        Debug.Log(time1);
        float time2 = (float)ExtractTimeSpan(timestamp2).TotalSeconds;
        Debug.Log(time2);

        Vector2 startPos = new Vector2((time1 * float.Parse(BPMSpeed.text)) + (canvasRect.sizeDelta.x / 2), (canvasRect.sizeDelta.y / 2));
        Vector2 endPos = new Vector2((time2 * float.Parse(BPMSpeed.text)) + (canvasRect.sizeDelta.x / 2), (canvasRect.sizeDelta.y / 2));

        GameObject spawnedPrefab = Instantiate(testPrefab, canvasRect);
        LineRenderer lineRenderer = spawnedPrefab.GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        spawnedLines.Add(spawnedPrefab);
        lineRenderers = spawnedLines.ConvertAll(line => line.GetComponent<LineRenderer>()).ToArray();

        Vector3[] linePositions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(linePositions);

        int a = 0;
        for (int i = 0; i <= 4; i++)
        {
            float t = i / 5f;
            Vector3 position = Vector3.Lerp(linePositions[0], linePositions[1], t);
            position.y = ((midiData[a][i] * (canvasRect.sizeDelta.y / 2)) / 55);
            GameObject spawnedPref = Instantiate(linePrefab, position, Quaternion.identity, canvasRect);
            spawnedPrefabs.Add(spawnedPref);

            if (i == 0)
            {
                SetImageColor(spawnedPrefab, Color.green);
            }
            else if (i == 4)
            {
                SetImageColor(spawnedPrefab, Color.red);
            }


        }
        a++;
    }

    void SetImageColor(GameObject prefab, Color color)
    {
        Image image = prefab.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
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

                spawnedPrefabs.Add(line);

                space += LineSpacing;
            }
        }
    }

    IEnumerator AnimatePrefabs()
    {
        while (true)
        {
            foreach (GameObject line in spawnedPrefabs)
            {
                RectTransform lineRect = line.GetComponent<RectTransform>();

                float newX = lineRect.anchoredPosition.x - (moveSpeed * Time.deltaTime);
                lineRect.anchoredPosition = new Vector2(newX, lineRect.anchoredPosition.y);

            }

            yield return null;
        }
    }

    IEnumerator AnimateLines()
    {
        lineRenderers = spawnedLines.ConvertAll(line => line.GetComponent<LineRenderer>()).ToArray();

        while (true)
        {
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                LineRenderer lineRenderer = lineRenderers[i];

                Vector3 pos1 = lineRenderer.GetPosition(0);
                Vector3 pos2 = lineRenderer.GetPosition(1);

                float moveDistance = moveSpeed * Time.deltaTime;
                pos1.x -= moveDistance;
                pos2.x -= moveDistance;

                lineRenderer.SetPosition(0, pos1);
                lineRenderer.SetPosition(1, pos2);

                if (pos2.x < -canvasRect.rect.width)
                {
                    float lastLineX = lineRenderers[lineRenderers.Length - 1].GetPosition(1).x;
                    pos1.x = lastLineX;
                    pos2.x = lastLineX + LineSpacing;

                    lineRenderer.SetPosition(0, pos1);
                    lineRenderer.SetPosition(1, pos2);
                }
            }

            yield return null;
        }
    }
}