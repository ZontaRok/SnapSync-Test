using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoicePickUp : MonoBehaviour
{
    public float sensitivity = 1f;
    public float moveSpeed = 5f;
    public float maxY = 5f;
    public float minY = 0f;


    private bool isMovingUp = false;
    private bool isMovingDown = false;

    private AudioSource audioSource;
    private float[] spectrum = new float[256];
        
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
        // Capture spectrum data
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        // Calculate average pitch
        float avgPitch = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            avgPitch += spectrum[i];
        }
        avgPitch /= spectrum.Length;

        // Move object based on average pitch and sensitivity
        if (avgPitch > sensitivity && !isMovingUp && transform.position.y < maxY)
        {
            isMovingUp = true;
            isMovingDown = false;
        }
        else if (avgPitch <= sensitivity && !isMovingDown && transform.position.y > minY)
        {
            isMovingDown = true;
            isMovingUp = false;
        }

        // Update object position
        if (isMovingUp)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            if (transform.position.y >= maxY)
            {
                isMovingUp = false;
            }
        }
        else if (isMovingDown)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            if (transform.position.y <= minY)
            {
                isMovingDown = false;
            }
        }
    }

    void OnDisable() 
    { 
        Microphone.End(null);
    }
}
