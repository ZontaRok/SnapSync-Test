using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public float loudness = 0;
    public float loudnessThreshold = 0.5f;
    public float sensitivity = 100;
    public float moveSpeed = 0.1f;

    private AudioSource audioSource;

    void Start()
    {

        string[] devices = Microphone.devices;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(devices[0], true, 10, 44100);
        audioSource.loop = true;

        // Debugging: Check if Microphone is recording
        Debug.Log("Microphone is recording: " + Microphone.IsRecording(null));

        while (!(Microphone.GetPosition(null) > 0)) { } // Debugging: Check if the microphone position is greater than 0
        Debug.Log("Microphone position: " + Microphone.GetPosition(null));

        audioSource.Play(); // Debugging: Check if AudioSource is playing
        Debug.Log("AudioSource is playing: " + audioSource.isPlaying);
    }

    void Update()
    {
        loudness = GetMicrophoneLoudness() * sensitivity;

        Debug.Log("Current Loudness: " + loudness);


        if (loudness < loudnessThreshold)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
    }

    float GetMicrophoneLoudness()
    {
        float[] samples = new float[256];
        float sum = 0;

        audioSource.GetOutputData(samples, 0);

        // Debugging: Print individual sample values
        for (int i = 0; i < samples.Length; i++)
        {
            Debug.Log("Sample " + i + ": " + samples[i]);
        }

        // Calculate the sum of absolute values of samples
        foreach (float sample in samples)
        {
            sum += Mathf.Abs(sample);
        }

        // Debugging: Print the sum value
        Debug.Log("Sum of Samples: " + sum);

        return sum / samples.Length;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GeneratedLine")) // Assuming the generated lines are tagged with "GeneratedLine"
        {
            // Handle collision with generated lines (e.g., decrease health, trigger animation)
            Debug.Log("Player collided with a generated line!");
        }
    }
}
