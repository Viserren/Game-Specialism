using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float pollingTime = 1f;
    private float _time;
    private int _frameCount;

    public static event Action<List<float>> FPSGraphChanged;
    public static event Action<float> FPSTextChanged;

    private List<float> fpsHistory = new List<float>();

    private int lastFrameIndex;
    public int graphHistory = 60;
    private float[] frameDeltaTimeArray;
    private void Awake()
    {
        frameDeltaTimeArray = new float[graphHistory];
    }
    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        _frameCount++;

        if (_time >= pollingTime)
        {
            frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
            lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;
            if (fpsHistory.Count >= graphHistory)
            {
                fpsHistory.RemoveAt(0);
            }
            fpsHistory.Add(Mathf.RoundToInt(CalculateFPS()));
            _time -= pollingTime;
            _frameCount = 0;

            FPSGraphChanged?.Invoke(fpsHistory);
            FPSTextChanged?.Invoke(CalculateFPS());
        }

    }

    private float CalculateFPS()
    {
        float total = 0;
        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += Time.deltaTime;
        }
        return frameDeltaTimeArray.Length / total;
    }
}
