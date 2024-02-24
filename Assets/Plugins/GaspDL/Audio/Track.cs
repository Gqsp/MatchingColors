using System.Collections.Generic;
using UnityEngine;

namespace GaspDL.Audio
{
    [System.Serializable]
    public class Track
    {
        public string trackName;
        public int trackID;
        [HideInInspector] public AudioSource introSource, bodySource;

        public bool destroyTrackWhenDone;
        public bool loopQueue;
        public Music currentlyPlaying;
        public List<Music> queuedMusic = new List<Music>();

        private float _trackVolume, _musicVolume;
        private int currentMusicID;
        public float GetVolume => _trackVolume * _musicVolume;

        public float TrackVolume
        {
            get => _trackVolume;
            set
            {
                _trackVolume = value;
                introSource.volume = value;
                bodySource.volume = value;
            }
        }

        public bool isFading;

        public Track(AudioPlayer player, int id, AudioSource source1, AudioSource source2, bool destroyWhenDone = true)
        {
            trackID = id;
            trackName = "Track" + (trackID < 10 ? "0" + trackID : trackID.ToString());
            introSource = source1;
            bodySource = source2;
            destroyTrackWhenDone = destroyWhenDone;
            TrackVolume = player.MusicVolume;
        }

        public Track(AudioPlayer player, int id, string name, AudioSource source1, AudioSource source2,
            bool destroyWhenDone = true)
        {
            trackID = id;
            trackName = name;
            introSource = source1;
            bodySource = source2;

            introSource.loop = false;
            bodySource.loop = true;
            destroyTrackWhenDone = destroyWhenDone;
            TrackVolume = player.MusicVolume;
        }

        public void PlayNow(Music music)
        {
            if (currentlyPlaying != null) StopPlaying();

            introSource.clip = music.intro;
            bodySource.clip = music.body;
            bodySource.loop = music.loopBody;
            _musicVolume = music.volume;

            introSource.volume = GetVolume;
            bodySource.volume = GetVolume;

            if (music.intro != null)
            {
                double len1 = introSource.clip.samples;

                double t0 = AudioSettings.dspTime;
                double clipTime1 = len1;
                clipTime1 /= introSource.clip.frequency;
                introSource.PlayScheduled(t0);
                introSource.SetScheduledEndTime(t0 + clipTime1);
                bodySource.PlayScheduled(t0 + clipTime1);
                bodySource.time = 0;
            }
            else
            {
                bodySource.Play();
            }

            currentlyPlaying = music;
            AddQueue(music);
            currentMusicID = queuedMusic.Count - 1;
        }

        public void AddQueue(Music music)
        {
            queuedMusic.Add(music);
        }

        public void StopPlaying()
        {
            if (currentlyPlaying == null) return;

            if (introSource != null)
            {
                if (introSource.isPlaying)
                    introSource.Stop();
            }

            if (bodySource.isPlaying)
                bodySource.Stop();

            currentlyPlaying = null;
        }

        private bool isIntroPaused, isBodyPaused;

        public void Pause()
        {
            if (currentlyPlaying == null) return;

            if (introSource != null)
            {
                if (introSource.isPlaying)
                {
                    introSource.Pause();
                    bodySource.Stop();
                    isIntroPaused = true;
                }
                else isIntroPaused = false;
            }

            if (bodySource.isPlaying)
            {
                bodySource.Pause();
                isBodyPaused = true;
            }
            else isBodyPaused = false;
        }

        public void Restart()
        {
            if (currentlyPlaying == null) return;

            if (introSource != null)
            {
                if (isIntroPaused)
                {
                    double fullLen = introSource.clip.samples;
                    double len1 = introSource.timeSamples;
                    double t0 = AudioSettings.dspTime;
                    double clipTime1 = fullLen - len1;
                    clipTime1 /= introSource.clip.frequency;
                    introSource.PlayScheduled(t0);
                    introSource.SetScheduledEndTime(t0 + clipTime1);
                    bodySource.PlayScheduled(t0 + clipTime1);
                    bodySource.time = 0;
                }
            }

            if (isBodyPaused)
                bodySource.Play();
        }

        public void Clear()
        {
            if (currentlyPlaying != null) StopPlaying();

            queuedMusic = new List<Music>();
        }

        public void RemoveMusic(string musicID)
        {
            if (IsCurrentlyPlaying(musicID))
            {
                currentlyPlaying = null;
                PlayNext();
            }

            queuedMusic.RemoveAll(music => music.name == musicID);
        }

        public void PlayNext()
        {
            currentMusicID++;

            if (!loopQueue && currentMusicID >= queuedMusic.Count)
            {
                currentlyPlaying = null;
                Clear();
            }
            else
            {
                currentMusicID %= queuedMusic.Count;
                PlayNow(queuedMusic[currentMusicID]);
            }
        }

        public void Shuffle()
        {
        }

        private void OnMusicDone()
        {
            PlayNext();
        }

        public void Destroy()
        {
            Object.Destroy(introSource);
            Object.Destroy(bodySource);
        }

        public bool IsCurrentlyPlaying(string musicID)
        {
            return (currentlyPlaying.name == musicID);
        }

        public bool TrackContains(string musicID)
        {
            if (IsCurrentlyPlaying(musicID)) return true;

            foreach (var music in queuedMusic)
            {
                if (music.name == musicID) return true;
            }

            return false;
        }
    }
}