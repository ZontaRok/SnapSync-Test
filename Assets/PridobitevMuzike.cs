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
    private const string ApiUrl = "https://snapsync-two.vercel.app/api/songs";

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

        StartCoroutine(CreateMusicBox(music.CoverURL, music.Song_name, music.Artist, nameSt, music.Link, music.BPM, music.Lyrics, music.MIDI, music.lenght));
    }

    IEnumerator CreateMusicBox(string url, string Song_name, string Artist, int nameSt, string Link, string BPM, string Lyrics, string MIDI, string SongLength)
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


                double originalNumber = double.Parse(SongLength);

                // Convert to minutes and seconds
                int minutes = (int)originalNumber;  // Extract the whole part
                int seconds = (int)((originalNumber - minutes) * 100);  // Extract the decimal part as seconds

                // Use the formula to combine minutes and seconds
                int result = minutes * 60 + seconds;

                newMusicBox.transform.Find("SongNameText").GetComponent<Text>().text = Song_name;
                newMusicBox.transform.Find("ArtistText").GetComponent<Text>().text = Artist;
                newMusicBox.transform.Find("CoverURLImage").GetComponent<Image>().sprite = sprite;
                newMusicBox.transform.Find("UrlSongPlayhidden").GetComponent<Text>().text = Link;
                newMusicBox.transform.Find("BPMhidden").GetComponent<Text>().text = BPM;
                newMusicBox.transform.Find("Lyricshidden").GetComponent<Text>().text = Lyrics;
                newMusicBox.transform.Find("MIDIhidden").GetComponent<Text>().text = MIDI;
                newMusicBox.transform.Find("Lengthhidden").GetComponent<Text>().text = result.ToString();
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
    public string BPM { get; set; }    
    public string MIDI { get; set; }    
    public string CoverURL { get; set; }
    public string Link { get; set; }
    public string Lyrics { get; set; }
    public string Song_name { get; set; }
    public string lenght { get; set; }
}

public class ApiResponse
{
    public List<Music> songs { get; set; }
}