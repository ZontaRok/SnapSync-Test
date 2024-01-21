using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchaleFromMicrophone : MonoBehaviour
{
    public AudioSource source;
    public AudioLoudnessDetection detector;

    public float loudnessSensiteivety = 100;
    public float loudness = 0;
    public float loudnessThreshold = 0.1f;
    public float moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessfromMicrophone() * loudnessSensiteivety;

        if (loudness > loudnessThreshold)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
    }
}
