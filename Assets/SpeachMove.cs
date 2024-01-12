using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAudio.Wave;

public class SpeachMove : MonoBehaviour
{
    private AudioClip recordedClip;
    private const int sampleRate = 44100; // This can vary
    private const int recordTime = 5; // Recording time in seconds

    void Start()
    {
        recordedClip = Microphone.Start(null, true, recordTime, sampleRate);
    }

    void Update()
    {
    }
}
