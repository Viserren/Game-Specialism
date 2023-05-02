using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamBand : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    public bool useBuffer = false;
    public bool useUnscaled = false;

    // Update is called once per frame
    void Update()
    {
        var temp = gameObject.GetComponent<RectTransform>();
        if (useBuffer && !useUnscaled)
        {
            temp.sizeDelta = new Vector2(.08f, (AudioAnalyser.audioBandBuffer[band] * scaleMultiplier) + startScale);
        }
        else if (useBuffer && useUnscaled)
        {
            temp.sizeDelta = new Vector2(.08f, (AudioAnalyser.frequencyBandBuffer[band] * scaleMultiplier) + startScale);
        }
        else if (!useBuffer && useUnscaled)
        {
            temp.sizeDelta = new Vector2(.08f, (AudioAnalyser.audioBand[band] * scaleMultiplier) + startScale);
        }
        else
        {
            temp.sizeDelta = new Vector2(.08f, (AudioAnalyser.frequencyBands[band] * scaleMultiplier) + startScale);
        }
    }
}
