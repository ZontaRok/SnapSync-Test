    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Networking;
    using System.Threading.Tasks;
    using System.Net.Http;
    using UnityEngine.EventSystems;
    using Newtonsoft.Json;

public class ScoreUI : MonoBehaviour
{

    public GameObject rowUi;
    public Transform contentParent;
    private Dictionary<string, Score> ScoreDictionary = new Dictionary<string, Score>();
    ScoreData apiResponse = new ScoreData();
    private const string ApiUrl = "https://snapsync-two.vercel.app/api/leaderboard?songName=Take%20on%20Me";   


    async void Start()
    {
        await GetAndDisplayHighScores();
    }

    async Task GetAndDisplayHighScores()
    {
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(ApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    LoadDisplayData(jsonContent);
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
    void LoadDisplayData(string jsonData)
    {
        apiResponse = JsonConvert.DeserializeObject<ScoreData>(jsonData);
        int nameSt = 0;


        if (apiResponse != null && apiResponse.leaderboard != null)
        {
            foreach (var score in apiResponse.leaderboard)
            {
                LoadRow(score, nameSt);
                nameSt++;
            }
        }
    }

    public void LoadRow(Score score, int nameSt)
    {
        string prefabName = "Row" + score.songName;

        ScoreDictionary[prefabName] = score;

        StartCoroutine(CreateRow(score.songName, score.score, score._id, score.username, nameSt));
    }

    IEnumerator CreateRow(string Name, float ScoreNumber, string Id, string Username, int nameSt)
    {
        string uniqueName = "Row" + nameSt.ToString();

        GameObject newMusicBox = Instantiate(rowUi, contentParent);
        newMusicBox.name = uniqueName;

        ScoreDictionary[uniqueName] = new Score { songName = Name, score = ScoreNumber, _id = Id, username = Username };

        newMusicBox.transform.Find("Rank").GetComponent<Text>().text = (nameSt+1).ToString(); ;
        newMusicBox.transform.Find("User").GetComponent<Text>().text = Username;
        newMusicBox.transform.Find("Score").GetComponent<Text>().text = ScoreNumber.ToString();

        yield return null;
    }

    public class Score
    {
        public string _id { get; set; }
        public float score { get; set; }
        public string songName { get; set; }
        public string username { get; set; }
    }

    public class ScoreData
    {
        public List<Score> leaderboard { get; set; }
    }

}
