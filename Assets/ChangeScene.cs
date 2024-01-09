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
        ActivatePanel(GameCPanel);
        DeactivatePanel(playGamePanel);
    }

    public void NavigateToPlayGame(string SongName)
    {
        Text textComponent = playGamePanel.transform.Find("PlaySongName")?.GetComponent<Text>();

        if (textComponent != null)
        {
            textComponent.text = SongName;
        }

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
