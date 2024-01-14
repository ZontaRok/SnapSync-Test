using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{

    public float loudness = 0;
    public float loudnessThreshold = 0.5f;
    public float sensitivity = 100;
    public float moveSpeed = 0.1f;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
    }



    void Update()
    {
        loudness = GetAvragevolume() * sensitivity;

        if (loudness < loudnessThreshold)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }   
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
    }

    float GetAvragevolume()
    {        
        float[] data = new float[64];
        float a = 0;
        audioSource.GetOutputData(data, 0);
        foreach(float s in data)
        {
            a += Mathf.Abs(s);
        }

        return (a / data.Length);
    }
}
