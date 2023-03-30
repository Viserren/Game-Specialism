using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackInfo : ScriptableObject
{
    public static event Action<TrackInfo,bool> OnSongChanged;
    public string trackTitle { get; private set; }
    public string trackArtist { get; private set; }
    public AudioClip trackClip { get; private set; }
    public int trackNumber { get; private set; }

    public void SetTracKTitle(string newTitle)
    {
        trackTitle = newTitle;
    }

    public void SetTrackArtist(string newArtist)
    {
        trackArtist = newArtist;
    }

    public void SetTrackClip(AudioClip newAudioClip)
    {
        trackClip = newAudioClip;
    }

    public void PlayTrack()
    {
        OnSongChanged?.Invoke(this,true);
    }

    public void SetTrackNumber(int newTrackNumber)
    {
        trackNumber = newTrackNumber;
    }
}
