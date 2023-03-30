using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackButton : MonoBehaviour
{
    public TrackInfo trackInfo { get; private set; }

    public void SetTrackInfo(TrackInfo trackInfo)
    {
        this.trackInfo = trackInfo;
    }

    public void PlayTrack()
    {
        if (trackInfo != null)
        {
            trackInfo.PlayTrack();
        }
    }
}
