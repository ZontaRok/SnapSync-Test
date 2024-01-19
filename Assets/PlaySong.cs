using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaySong : MonoBehaviour
{
    public GameObject MusicBox;

    private string SongName;
    private string BPMName;
    private string LyricsName;
    private string MIDIName;
    private string Songlength;

    public void OnClickButton()
    {
        Transform musicBoxTransform = MusicBox.transform;

        Transform[] allChildren = musicBoxTransform.GetComponentsInChildren<Transform>(true);

        if (allChildren.Length > 0)
        {
            Transform TextSong_Name = allChildren[4];
            Transform TextBPM = allChildren[7];
            Transform TextLyrics = allChildren[9];
            Transform TextMIDI = allChildren[10];
            Transform TextLength = allChildren[11];

            Text textComponentForSong_Name = TextSong_Name.GetComponent<Text>();
            Text textComponentForBPM = TextBPM.GetComponent<Text>();
            Text textComponentForLyrics = TextLyrics.GetComponent<Text>();
            Text textComponentForMIDI = TextMIDI.GetComponent<Text>();
            Text textComponentForLength = TextLength.GetComponent<Text>();

            string Song_Name = textComponentForSong_Name.text;
            string BPM = textComponentForBPM.text;
            string Lyrics = textComponentForLyrics.text;
            string MIDI = textComponentForMIDI.text;
            string songlength = textComponentForLength.text;


            Debug.Log(Song_Name);
            Debug.Log(BPM);
            Debug.Log(Lyrics);
            Debug.Log(MIDI);
            Debug.Log(songlength);

            SongName = Song_Name;
            BPMName = BPM;
            LyricsName = Lyrics;
            MIDIName = MIDI;
            Songlength = songlength;
        }
        else
        {
            Debug.Log("There are no child Transforms in the array.");
        }

        // Find the UIManager in the scene.
        ChangeScene uiManager = FindObjectOfType<ChangeScene>();

        // Check if UIManager is found.
        if (uiManager != null)
        {
            uiManager.NavigateToPlayGame(SongName, BPMName, LyricsName, MIDIName, Songlength);
        }
        else
        {
            Debug.LogWarning("UIManager not found in the scene.");
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
