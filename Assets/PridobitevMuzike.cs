using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;



public class PridobitevMuzike : MonoBehaviour
{

    public GameObject musicBoxPrefab; 
    public Transform contentParent;
    public Music currentSong;

    private Dictionary<string, Music> songDictionary = new Dictionary<string, Music>();
    ApiResponse apiResponse = new ApiResponse();
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
        apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonData);
        int nameSt = 0;

        if (apiResponse != null && apiResponse.songs != null)
        {
            foreach (var music in apiResponse.songs)
            {
                LoadMusicBox(music, nameSt);
                nameSt++;
            }
        }
    }

    public void LoadMusicBox(Music music, int nameSt)
    {
        string prefabName = "SongDisplayContent" + music.Song_name;

        songDictionary[prefabName] = music;

        StartCoroutine(CreateMusicBox(music.CoverURL, music.Song_name, music.Artist, nameSt, music.Link));
    }

    IEnumerator CreateMusicBox(string url, string Song_name, string Artist, int nameSt, string Link)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Generate a unique name for the prefab instance using a timestamp
                string uniqueName = "SongDisplayContent" + nameSt.ToString();

                // Instantiate the prefab with the unique name
                GameObject newMusicBox = Instantiate(musicBoxPrefab, contentParent);
                newMusicBox.name = uniqueName; // Set the unique name to the instantiated prefab

                // Add the Music data to the dictionary using the unique name as the key
                songDictionary[uniqueName] = new Music { Artist = Artist, CoverURL = url, Song_name = Song_name };

                // Set the text and image components of the instantiated prefab
                newMusicBox.transform.Find("SongNameText").GetComponent<Text>().text = Song_name;
                newMusicBox.transform.Find("ArtistText").GetComponent<Text>().text = Artist;
                newMusicBox.transform.Find("CoverURLImage").GetComponent<Image>().sprite = sprite;
                newMusicBox.transform.Find("UrlSongPlayhidden").GetComponent<Text>().text = Link;
            }
            else
            {
                Debug.Log("Failed to load image: " + www.error);
            }
        }
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