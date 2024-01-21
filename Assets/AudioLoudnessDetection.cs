using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
        
    //public float loudness = 0;
    //public float loudnessThreshold = 0.5f;
    //public float sensitivity = 100;
    //public float moveSpeed = 0.1f;

    //public float minY = -2.0f;
    //public float maxY = 2.0f

    public int samapleWindows = 64;
    private AudioClip microphoneClip;


    //AudioSource audioSource;

    //void Start()
    //{
    //    audioSource = GetComponent<AudioSource>();
    //    audioSource.clip = Microphone.Start(null, true, 1, 44100);
    //    audioSource.loop = true;
    //    while (!(Microphone.GetPosition(null) > 0)) { }
    //    audioSource.Play();
    //}



    //void Update()
    //{
    //    loudness = GetAverageVolume() * sensitivity;

    //    float translationSpeed = Mathf.Lerp(0, moveSpeed, loudness / loudnessThreshold);

    //    if (loudness > loudnessThreshold)
    //    {
    //        transform.Translate(Vector3.up * translationSpeed * Time.deltaTime);
    //    }
    //    else
    //    {
    //        transform.Translate(Vector3.down * translationSpeed * Time.deltaTime);
    //    }

    //    float newY = Mathf.Clamp(transform.position.y, minY, maxY);
    //    transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    //}


    //float GetAverageVolume()
    //{
    //    float[] data = new float[64];
    //    float a = 0;
    //    audioSource.GetOutputData(data, 0);
    //    foreach (float s in data)
    //    {
    //        a += Mathf.Abs(s);
    //    }

    //    return (a / data.Length);
    //}

    void Start()
    {
        MicrophoneToAudioClip();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetLoudnessfromMicrophone()
    {
        return GetLoudnessfromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public  void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessfromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - samapleWindows;

        float[] waveDate = new float[samapleWindows];
        clip.GetData(waveDate, startPosition);

        float totalLoudness = 0;

        for(int i = 0; i < samapleWindows; i++)
        {
            totalLoudness += Mathf.Abs(waveDate[i]);
        }

        return totalLoudness / samapleWindows;  
    }
}
