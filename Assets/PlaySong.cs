using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class PlaySong : MonoBehaviour
{
    public GameObject MusicBox;
    public AudioSource audioSource;

    public string localFilePath = "Assets/DownloadedFiles/file.mp3";
    public string fileURL = "https://storage.googleapis.com/karaoke-cfa02.appspot.com/80s/a-ha%20-%20Take%20On%20Me%20(Lyrics).mp3";


    private void Start()
    {
        if (MusicBox.TryGetComponent<AudioSource>(out AudioSource existingSource))
        {
            audioSource = existingSource;
        }
        else
        {
            audioSource = MusicBox.AddComponent<AudioSource>();
        }
    }

    public void OnClickButton()
    {
        Transform musicBoxTransform = MusicBox.transform;

        Transform[] allChildren = musicBoxTransform.GetComponentsInChildren<Transform>(true);

        if (allChildren.Length > 0)
        {
            // Get the last child Transfor
            Transform lastChild = allChildren[allChildren.Length - 1];

            // Try to get the Text component from the last child Transfor
            Text textComponent = lastChild.GetComponent<Text>();

            // If the Text component exists on the last child, log its text valu
            if (textComponent != null)
            {
                string textValue = textComponent.text;
                Debug.Log(textValue);
                //StartCoroutine(CretaFile(textValue));
                StartCoroutine(PlayAudioFromURL(fileURL));
                //StartCoroutine(LoadAudioClip());
            }
            else
            {
                Debug.Log("The last child Transform does not have a Text component.");
            }
        }
        else
        {
            Debug.Log("There are no child Transforms in the array.");
        }
    }


    IEnumerator CretaFile(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Save the downloaded file to the local path
                System.IO.File.WriteAllBytes(localFilePath, www.downloadHandler.data);

                Debug.Log("File downloaded to: " + localFilePath);
            }
            else
            {
                Debug.LogError("Download failed: " + www.error);
            }
        }
    }
    IEnumerator PlayAudioFromURL(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            // Set the download handler to AudioDownloadHandler to stream the audio
            www.downloadHandler = new DownloadHandlerAudioClip(url, AudioType.MPEG);

            // Begin the download
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Create an AudioSource component and play the streamed audio clip
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.Play();

                Debug.Log("Audio streamed and played!");
            }
            else
            {
                Debug.LogError("Streaming failed: " + www.error);
            }
        }
    }

    private System.Collections.IEnumerator LoadAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(localFilePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error loading audio data: " + www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null && audioSource != null)
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                }
            }
        }
    }
}