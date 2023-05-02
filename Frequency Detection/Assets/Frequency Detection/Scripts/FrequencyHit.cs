using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyHit : MonoBehaviour
{
    #region Listen to event
    private void OnEnable()
    {
        AudioAnalyser.FrequencyHit += WhatFrequencyHit;
    }

    private void OnDisable()
    {
        AudioAnalyser.FrequencyHit -= WhatFrequencyHit;
    }
    #endregion

    private void WhatFrequencyHit(int band)
    {
        Debug.Log(band);
    }

}
