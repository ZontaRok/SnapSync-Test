using UnityEngine;
using UnityEngine.UI;

public class lyricsShow : MonoBehaviour
{
    public Text lyricsText;
    public Text LyricsInput;

    private string[] lyricsArray;
    private float[] timings;

    private float currentTime = 0f;

    void Start()
    {
        string lyrics = LyricsInput.text;
        lyricsArray = lyrics.Split('[', ']');
        ParseTimings();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        ShowLyricsAtTime(currentTime);
    }

    void ParseTimings()
    {
        timings = new float[lyricsArray.Length / 2];

        for (int i = 1, j = 0; i < lyricsArray.Length; i += 2, j++)
        {
            string[] timeParts = lyricsArray[i].Split(':');
            float minutes = float.Parse(timeParts[0]);
            float seconds = float.Parse(timeParts[1]);
            timings[j] = minutes * 60 + seconds;
        }
    }

    public void ShowLyricsAtTime(float time)
    {
        for (int i = 0; i < timings.Length; i++)
        {
            if (time >= timings[i])
            {
                UpdateLyricsDisplay(i);
            }
        }
    }

    void UpdateLyricsDisplay(int lyricIndex)
    {
        lyricsText.text = lyricsArray[lyricIndex * 2 + 2];
    }
}
