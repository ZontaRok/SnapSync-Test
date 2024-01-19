using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using System.Net.Http;
using System.Threading.Tasks;

public class leaderBoardPush : MonoBehaviour
{

    public Text SongName;
    public Text Score;

    //https://snapsync-two.vercel.app/api/leaderboardpost?username=John&score=100&songName=ExampleSong
    private const string ApiUrl = "https://snapsync-two.vercel.app/api/leaderboardpost?";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Pritistni()
    {
        FetchAndDisplayMusic();
    }

    async Task FetchAndDisplayMusic()
    {
        string username = "neki";
        string score = Score.text;
        string Songname = SongName.text;
        string YesApiUrl = ApiUrl + "username=" + username + "&score=" + score + "&songName=" + SongName.text;
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
}
