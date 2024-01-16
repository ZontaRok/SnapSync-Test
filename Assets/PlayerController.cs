using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text ScoreNumber;
    public int score = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GeneratedLine")) // Assuming the generated lines are tagged with "GeneratedLine"
        {
            score = score + 1;
            ScoreNumber.text = score.ToString();

        }
    }
}
