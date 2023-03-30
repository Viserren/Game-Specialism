using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSGraphDisplay : MonoBehaviour
{
    public LineRenderer FPSLineHistory;

    public float frequency = 1;
    public float amplitude = 1;
    public Vector2 offset = new Vector2(0,1);
    private float yOffset;
    public float yOffsetFraction = 2.5f;
    public float maxPeek;


    private void Awake()
    {
        FPSLineHistory = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        FPSCounter.FPSGraphChanged += Draw;
    }

    private void OnDisable()
    {
        FPSCounter.FPSGraphChanged -= Draw;
    }

    void Draw(List<float> graph)
    {
        yOffset = ((amplitude / yOffsetFraction) * -1) + offset.y;

        FPSLineHistory.positionCount = graph.Count;
        for (int currentPoint = 0; currentPoint < graph.Count; currentPoint++)
        {
            if (graph[currentPoint] > maxPeek)
            {
                maxPeek = graph[currentPoint];
            }
            FPSLineHistory.SetPosition(currentPoint, new Vector3((currentPoint + offset.x) * frequency, ((Mathf.InverseLerp(0, maxPeek, graph[currentPoint]) * amplitude) + yOffset), 0));
        }
    }
}
