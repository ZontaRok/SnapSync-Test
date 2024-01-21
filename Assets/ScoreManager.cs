using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;

    //https://snapsync-two.vercel.app/api/leaderboard?songName=ExampleSong
    private string apiUrl = "https://snapsync-two.vercel.app/api/leaderboard?songName=ExampleSong";


    void Awake()
    {
        sd = new ScoreData();
    }

    public void AddScore(Score score) {
        sd.scores.Add(score);
    }

    public IEnumerable<Score> GetHighScores()
    {
        string jsonResponse = FetchDataFromAPI(apiUrl);

        if (!string.IsNullOrEmpty(jsonResponse))
        {

            ScoreData scoreData = JsonUtility.FromJson<ScoreData>(jsonResponse);

            // Vrnitev urejenega seznama rezultatov po padajoèem vrstnem redu
            return scoreData.scores;
        }

        return null;
    }

    private string FetchDataFromAPI(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SendWebRequest();

        while (!request.isDone)
        {
            
        }

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Napaka pri pridobivanju podatkov iz API-ja: " + request.error);
            return null;
        }
        else
        {
            return request.downloadHandler.text;
        }
    }
}
