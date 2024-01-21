using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject GameCPanel;
    public GameObject LeaderBoard;

    public void NazajNaGame()
    {

        DeactivatePanel(LeaderBoard);
        ActivatePanel(GameCPanel);
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
