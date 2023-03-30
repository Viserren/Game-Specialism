using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(AudioSource))]
public class LoadAudioFromFiles : MonoBehaviour
{
    //string _settingsPath = "Music/";
    string _settingsPathWAV = "Music/WAV/";
#if UNITY_EDITOR
    public bool useWAV = true;
    string _settingsPathMP3 = "Music/MP3/";
#endif
    public GameObject trackSelectButtonUI;
    List<TrackButton> _orderedTrackList = new List<TrackButton>();
    List<TrackButton> _shuffledTrackList = new List<TrackButton>();
    TrackInfo _currentPlayingSong;

    public Transform contentDisplayList;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Artist;
    public Image playPauseButtonImage;
    public Image shuffleButtonImage;
    public Color shuffleOnColour, shuffleOffColour;
    public Image loopButtonImage;
    public Color loopOnColour, loopOffColour;
    public Sprite[] playPauseButtonSprite;
    public Slider trackPlayTime;
    AudioSource _source;

    int _currentTrackIndex;
    bool _isShuffled;
    bool _isLooping;

    #region Listen to event
    private void OnEnable()
    {
        TrackInfo.OnSongChanged += PlaySong;
    }

    private void OnDisable()
    {
        TrackInfo.OnSongChanged -= PlaySong;
    }
    #endregion

    private void Start()
    {
        _source = GetComponent<AudioSource>();

        LoadTracks();
    }

    private void LoadTracks()
    {
#if UNITY_EDITOR
        if (!useWAV)
        {
            int currentTrack = 0;
            ES3.Save("Music Path MP3", "Saved", filePath: _settingsPathMP3 + "Create MP3 Music Folder.txt");
            foreach (string fileName in ES3.GetFiles(_settingsPathMP3))
            {
                if (fileName.EndsWith(".mp3"))
                {
                    AudioClip tempAudioClip = ES3.LoadAudio(_settingsPathMP3 + fileName, AudioType.MPEG);
                    tempAudioClip.name = fileName;


                    TrackInfo trackInfo = ScriptableObject.CreateInstance<TrackInfo>();
                    string[] songInfo = fileName.Split(new string[] { "-", "." }, StringSplitOptions.None);
                    if (songInfo.Length >= 1)
                    {
                        trackInfo.name = $"Track {songInfo[0]} - {songInfo[1]} Info ";
                        trackInfo.SetTracKTitle(songInfo[0]);
                        trackInfo.SetTrackArtist(songInfo[1]);
                    }
                    else
                    {
                        trackInfo.name = $"Track {songInfo[0]} Info ";
                        trackInfo.SetTracKTitle(songInfo[0]);
                        trackInfo.SetTrackArtist("Unknown");
                    }
                    trackInfo.SetTrackClip(tempAudioClip);
                    trackInfo.SetTrackNumber(currentTrack);

                    GameObject tempButton = Instantiate(trackSelectButtonUI, contentDisplayList);
                    tempButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{songInfo[0]} - {songInfo[1]}";
                    if (tempButton.TryGetComponent<TrackButton>(out TrackButton button))
                    {
                        button.SetTrackInfo(trackInfo);
                        _orderedTrackList.Add(button);
                    }

                    currentTrack++;
                }
            }
            }
        if (useWAV)
        {
#endif
            int currentTrack = 0;
            ES3.Save("Music Path WAV", "Saved", filePath: _settingsPathWAV + "Create WAV Music Folder.txt");
            foreach (string fileName in ES3.GetFiles(_settingsPathWAV))
            {
                if (fileName.EndsWith(".wav"))
                {
                    AudioClip tempAudioClip = ES3.LoadAudio(_settingsPathWAV + fileName, AudioType.WAV);
                    tempAudioClip.name = fileName;


                    TrackInfo trackInfo = ScriptableObject.CreateInstance<TrackInfo>();
                    string[] songInfo = fileName.Split(new string[] { "-", "." }, StringSplitOptions.None);
                    if (songInfo.Length >= 3)
                    {
                        trackInfo.name = $"Track {songInfo[0]} - {songInfo[1]} Info ";
                        trackInfo.SetTracKTitle(songInfo[0]);
                        trackInfo.SetTrackArtist(songInfo[1]);
                    }
                    else
                    {
                        trackInfo.name = $"Track {songInfo[0]} Info ";
                        trackInfo.SetTracKTitle(songInfo[0]);
                        trackInfo.SetTrackArtist("Unknown");
                    }

                    trackInfo.SetTrackClip(tempAudioClip);
                    trackInfo.SetTrackNumber(currentTrack);

                    GameObject tempButton = Instantiate(trackSelectButtonUI, contentDisplayList);
                    tempButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{songInfo[0]} - {songInfo[1]}";
                    if (tempButton.TryGetComponent<TrackButton>(out TrackButton button))
                    {
                        button.SetTrackInfo(trackInfo);
                        _orderedTrackList.Add(button);
                    }

                    currentTrack++;
                }
            }
#if UNITY_EDITOR
        }
#endif

    }

    private void PlaySong(TrackInfo info)
    {
        if (info != _currentPlayingSong)
        {
            _source.clip = info.trackClip;
            Title.text = info.trackTitle;
            Artist.text = info.trackArtist;
            PlayPauseSong();
        }
        _currentTrackIndex = info.trackNumber;
        _currentPlayingSong = info;
    }

    private void PlaySong(TrackInfo info, bool restartTimer)
    {
        if (info != _currentPlayingSong)
        {
            _source.clip = info.trackClip;
            Title.text = info.trackTitle;
            Artist.text = info.trackArtist;
            PlayPauseSong();
            if (restartTimer)
            {
                StopAllCoroutines();
            }
            if (_source.isPlaying)
            {
                trackPlayTime.value = 0;
                StartCoroutine(QueueNextSong(info.trackClip.length));
            }
        }
        _currentTrackIndex = info.trackNumber;
        _currentPlayingSong = info;
    }

    IEnumerator QueueNextSong(float timeOfCurrentSong)
    {
        timeOfCurrentSong -= timeOfCurrentSong - (float)Math.Truncate(timeOfCurrentSong);
        float trackPlayTimeSeconds = 0;
        trackPlayTime.maxValue = timeOfCurrentSong;
        while (trackPlayTimeSeconds <= timeOfCurrentSong - 0.5f)
        {
            if (_source.isPlaying)
            {
                trackPlayTimeSeconds += .01f;
                trackPlayTime.value = trackPlayTimeSeconds;
            }
            yield return new WaitForSeconds(.01f);
        }
        NextSong();
    }

    public void PlayPauseSong()
    {
        if (_source.isPlaying)
        {
            playPauseButtonImage.sprite = playPauseButtonSprite[0];
            _source.Pause();
        }
        else
        {
            playPauseButtonImage.sprite = playPauseButtonSprite[1];
            _source.Play();
        }
    }

    public void NextSong()
    {
        _currentTrackIndex++;
        if (_isLooping)
        {
            if (_currentTrackIndex == GetCurrentPlaylist().Count)
            {
                _currentTrackIndex = 0;
            }

            TrackButton button = GetCurrentPlaylist()[_currentTrackIndex];

            PlaySong(button.trackInfo, true);
        }
        else
        {
            if (_currentTrackIndex == GetCurrentPlaylist().Count)
            {
                _currentTrackIndex = -1;
                _source.Stop();
            }
            else
            {
                TrackButton button = GetCurrentPlaylist()[_currentTrackIndex];

                PlaySong(button.trackInfo, true);
            }
        }

    }

    public void PreviousSong()
    {
        _currentTrackIndex--;
        if (_currentTrackIndex == -1)
        {
            _currentTrackIndex = 0;
        }

        TrackButton button = GetCurrentPlaylist()[_currentTrackIndex];

        PlaySong(button.trackInfo, true);
    }

    public void SetLooping()
    {
        _isLooping = !_isLooping;
        loopButtonImage.color = _isLooping ? loopOnColour : loopOffColour;
    }

    public void ShuffleSongs()
    {
        _isShuffled = !_isShuffled;
        shuffleButtonImage.color = _isShuffled ? shuffleOnColour : shuffleOffColour;

        if (_isShuffled)
        {
            _shuffledTrackList = _orderedTrackList.OrderBy(x => UnityEngine.Random.value).ToList();

            for (int i = 0; i < _shuffledTrackList.Count; i++)
            {
                if (_shuffledTrackList[i].TryGetComponent<TrackButton>(out TrackButton trackButton))
                {
                    trackButton.trackInfo.SetTrackNumber(i);
                    trackButton.transform.SetSiblingIndex(i);
                }
            }
            _shuffledTrackList.OrderBy(x => x.trackInfo.trackNumber).ToList();
            if (GetCurrentSong(_shuffledTrackList) != null)
            {
                _currentTrackIndex = GetCurrentSong(_shuffledTrackList).trackNumber;
                PlaySong(_shuffledTrackList[_currentTrackIndex].trackInfo);
            }
        }
        else
        {
            for (int i = 0; i < _orderedTrackList.Count; i++)
            {
                if (_orderedTrackList[i].TryGetComponent<TrackButton>(out TrackButton trackButton))
                {
                    trackButton.trackInfo.SetTrackNumber(i);
                    trackButton.transform.SetSiblingIndex(i);
                }
            }
            _orderedTrackList.OrderBy(x => x.trackInfo.trackNumber).ToList();
            if (GetCurrentSong(_shuffledTrackList) != null)
            {
                _currentTrackIndex = GetCurrentSong(_orderedTrackList).trackNumber;
                PlaySong(_orderedTrackList[_currentTrackIndex].trackInfo);
            }
        }
    }

    private List<TrackButton> GetCurrentPlaylist()
    {
        if (_isShuffled)
        {
            return _shuffledTrackList.Count != 0 ? _shuffledTrackList : _orderedTrackList;
        }
        else
        {
            return _orderedTrackList;
        }
    }

    private TrackInfo GetCurrentSong(List<TrackButton> trackDetails)
    {
        TrackInfo tempTrackInfo = ScriptableObject.CreateInstance<TrackInfo>();
        for (int i = 0; i < trackDetails.Count; i++)
        {
            if (trackDetails[i].TryGetComponent<TrackButton>(out TrackButton trackButton))
            {

                if (_currentPlayingSong == null)
                {
                    return null;
                }
                if (trackButton.trackInfo.name.Equals(_currentPlayingSong.name))
                {
                    tempTrackInfo = trackButton.trackInfo;
                }
            }
        }
        return tempTrackInfo;
    }
}
