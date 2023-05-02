using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSGraphDisplay : MonoBehaviour
{
    public LineRenderer FPSLineHistory;

    public float frequency = 1;
    public float amplitude = 1;
    public Vector2 offset = new Vector2(0,1);
    private float _yOffset;
    private float _maxPeek;


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
        _yOffset = (amplitude * -1) + offset.y;

        FPSLineHistory.positionCount = graph.Count;
        for (int currentPoint = 0; currentPoint < graph.Count; currentPoint++)
        {
            if (graph[currentPoint] > _maxPeek)
            {
                _maxPeek = graph[currentPoint];
            }
            FPSLineHistory.SetPosition(currentPoint, new Vector3((currentPoint + offset.x) * frequency, ((Mathf.InverseLerp(0, _maxPeek, graph[currentPoint]) * amplitude) + _yOffset), 0));
        }
    }
}
