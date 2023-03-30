using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAnalyser : MonoBehaviour
{
    public static event Action<int> FrequencyHit;

    AudioSource _audioSource;

    static float[] samples = new float[512];
    public static float[] frequencyBands = new float[8];
    public static float[] frequencyBandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];
    float[] _highestBandHeight = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    [Range(0, 1)]
    public float[] beatThreashold = new float[8] { .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f };


    [Range(0.00001f, .5f)]
    public float defaultDecrease = 0.005f;
    [Range(1f, 2f)]
    public float decreaseAmount = 1.05f;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        IsFrequencyHigher();
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (frequencyBands[i] > _highestBandHeight[i])
            {
                _highestBandHeight[i] = frequencyBands[i];
            }
            audioBand[i] = (frequencyBands[i] / _highestBandHeight[i]);
            audioBandBuffer[i] = (frequencyBandBuffer[i] / _highestBandHeight[i]);
        }
    }

    private void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void IsFrequencyHigher()
    {
        for (int i = 0; i < frequencyBands.Length; i++)
        {
            if (frequencyBands[i] >= beatThreashold[i])
            {
                FrequencyHit?.Invoke(i);
            }
        }
    }

    private void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (frequencyBands[i] > frequencyBandBuffer[i])
            {
                frequencyBandBuffer[i] = frequencyBands[i];
                _bufferDecrease[i] = defaultDecrease;
            }

            if (frequencyBands[i] < frequencyBandBuffer[i])
            {
                frequencyBandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= decreaseAmount;
            }
        }
    }

    private void MakeFrequencyBands()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;
            frequencyBands[i] = average;
        }
    }
}
