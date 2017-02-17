using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour
{
    public float sensitivity = 100;
    public float loudness = 0;
    public AudioSource audioSource;
    bool started;
    float counter;

    // Use this for initialization
    void Start()
    {

    }

    public void StartInput()
    {
        counter = 0;
        started = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 1, 44100);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (counter >= 60)
            {
                loudness = GetAveragedVolume() * sensitivity;
                counter = 0;
            }

            counter++;
        }

    }

    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        audioSource.GetOutputData(data, 0);
        foreach (float num in data)
        {
            a += Mathf.Abs(num);
        }
        return a / 256;
    }

    public void StopInput()
    {
        if(started)
        {
            audioSource.Stop();
            started = false;
            loudness = 0;
        }

    }
}
