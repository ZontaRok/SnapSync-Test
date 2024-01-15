using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class SongPlay : MonoBehaviour
{
    private AudioSource audioSource = null;
    public Text SongName;

    public string path = "C:/SnapSync/Assets/DownloadedFiles/";

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        StartCoroutine(LoadSongCoroutine());
    }
    private IEnumerator LoadSongCoroutine()
    {        
        string trimSongName = SongName.text.Replace(" ", "");
        Debug.Log("Song playing: " + trimSongName);

        string url = string.Format("file://{0}", path + trimSongName + ".mp3");
        WWW www = new WWW(url);
        yield return www;

        audioSource.clip = NAudioPlayer.FromMp3Data(www.bytes);
        audioSource.Play();
    }
}
