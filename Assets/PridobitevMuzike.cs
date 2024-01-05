using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PridobitevMuzike : MonoBehaviour
{
    public GameObject musicBoxPrefab; // Drag your prefab in the inspecto
    public Transform contentParent; // Drag a UI panel or content holder in the inspector

    private const string ApiUrl = "https://snapsyncapi-puce.vercel.app/api/songs";

    async void Start()
    {
        await FetchAndDisplayMusic();
    }

    async Task FetchAndDisplayMusic()
    {
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(ApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    DisplayData(jsonContent);
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

    void DisplayData(string jsonData)
    {
        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonData);

        int i = 1;

        if (apiResponse != null && apiResponse.songs != null)
        {
            foreach (var music in apiResponse.songs)
            {
                CreateMusicBox(music, i);
                i++;
            }
        }
    }
    
    void CreateMusicBox(Music music, int intcopy)
    {
        GameObject NewMusicBox = Instantiate(musicBoxPrefab, contentParent);

        NewMusicBox.transform.Find("SongNameText").GetComponent<Text>().text = music.Song_name;
        NewMusicBox.transform.Find("ArtistText").GetComponent<Text>().text = music.Artist;
        //NewMusicBox.transform.Find("CoverURLImage").GetComponent<Image>.

    }
}

public class Music
{
    public string Artist { get; set; }
    public string CoverURL { get; set; }
    public string Link { get; set; }
    public string Lyrics { get; set; }
    public string Song_name { get; set; }
}

public class ApiResponse
{
    public List<Music> songs { get; set; }
}