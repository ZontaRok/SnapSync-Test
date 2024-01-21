using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject GameCPanel;
    public GameObject playGamePanel;

    private void Start()
    {

    }

    public void NavigateToPlayGame(string SongName, string BPM, string Lyrics, string MIDI, string SongLength)
    {
        Text textComponentSongName = playGamePanel.transform.Find("PlaySongName")?.GetComponent<Text>();
        Text textComponentLyrics = playGamePanel.transform.Find("Lyricshidden")?.GetComponent<Text>();
        Text textComponentBPM = playGamePanel.transform.Find("BPMhidden")?.GetComponent<Text>();
        Text textComponentMIDI = playGamePanel.transform.Find("MIDIhidden")?.GetComponent<Text>();
        Text textComponentLength = playGamePanel.transform.Find("Lengthhidden")?.GetComponent<Text>();

        textComponentSongName.text = SongName;
        textComponentLyrics.text = Lyrics;
        textComponentBPM.text = BPM;
        textComponentMIDI.text = MIDI;
        textComponentLength.text = SongLength;

        DeactivatePanel(GameCPanel);
        ActivatePanel(playGamePanel);

    }

    private void ActivatePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    private void DeactivatePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
