using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI FPSCounterText;

    private void OnEnable()
    {
        FPSCounter.FPSTextChanged += UpdateText;
    }

    private void OnDisable()
    {
        FPSCounter.FPSTextChanged -= UpdateText;
    }

    void UpdateText(float newFPS)
    {

        FPSCounterText.text = $"FPS: {Mathf.RoundToInt(newFPS)}";
    }
}
