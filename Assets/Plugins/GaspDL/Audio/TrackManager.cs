using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GaspDL.Audio
{
    public class TrackManager : MonoBehaviour
    {
        public AudioPlayer linkedPlayer;
        public List<Track> RunningTracks = new List<Track>();

        private void Awake()
        {
            foreach (var track in RunningTracks)
            {
                track.PlayNext();
            }
        }

        public Track AddTrack(AudioSource audioSource)
        {
            return AddTrack(GetFirstUnusedTrack());
        }

        public Track AddTrack(int id)
        {
            var source = gameObject.AddComponent<AudioSource>();
            var source2 = gameObject.AddComponent<AudioSource>();
            var track = new Track(linkedPlayer, id, source, source2);
            RunningTracks.Add(track);
            return track;
        }

        public Track AddTrack(string trackName)
        {
            return AddTrack(GetFirstUnusedTrack(), trackName);
        }

        public Track AddTrack(int id, string trackName)
        {
            var source = gameObject.AddComponent<AudioSource>();
            var source2 = gameObject.AddComponent<AudioSource>();
            var track = new Track(linkedPlayer, id, trackName, source, source2);
            RunningTracks.Add(track);
            return track;
        }

        public Track GetOrCreate(int id)
        {
            var t = FindTrack(id);

            if (t != null)
            {
                return t;
            }
            else
            {
                return AddTrack(id);
            }
        }

        public Track GetOrCreate(string trackName)
        {
            var t = FindTrack(trackName);

            if (t != null)
            {
                return t;
            }
            else
            {
                return AddTrack(trackName);
            }
        }

        public Track FindTrack(int trackID)
        {
            return RunningTracks.Find(track => track.trackID == trackID);
        }

        public Track FindTrack(string trackName)
        {
            return RunningTracks.Find(track => track.trackName == trackName);
        }

        public void StopAllTracks()
        {
            foreach (var track in RunningTracks)
            {
                StopTrackPlaying(track);
            }
        }

        public void StopTrack(int trackID)
        {
            var track = FindTrack(trackID);
            if (track != null)
            {
                StopTrackPlaying(track);
            }
        }

        public void StopTrack(string trackName)
        {
            var track = FindTrack(trackName);
            if (track != null)
            {
                StopTrackPlaying(track);
            }
        }

        private void StopTrackPlaying(Track track)
        {
            track.StopPlaying();
            if (track.destroyTrackWhenDone)
            {
                track.Destroy();
                RunningTracks.Remove(track);
            }
        }

        public Track FindTrackPlayingMusic(string musicID)
        {
            foreach (var track in RunningTracks)
            {
                if (track.IsCurrentlyPlaying(musicID)) return track;
            }

            return null;
        }

        /// <summary>
        /// Remove track with the given id
        /// </summary>
        /// <param name="id">track identifier</param>
        /// <returns>true if track was found, false otherwise</returns>
        public bool RemoveTrack(int id)
        {
            var count = RunningTracks.Count;

            for (var i = 0; i < count; i++)
            {
                if (RunningTracks[i].trackID == id)
                {
                    RunningTracks.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Remove track with the given name
        /// </summary>
        /// <param name="name">track name</param>
        /// <returns>true if track was found, false otherwise</returns>
        public bool RemoveTrack(string name)
        {
            var count = RunningTracks.Count;

            for (var i = 0; i < count; i++)
            {
                if (RunningTracks[i].trackName == name)
                {
                    RunningTracks.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Finds the lowest unused track ID
        /// </summary>
        /// <returns>Lowest unused track ID</returns>
        public int GetFirstUnusedTrack()
        {
            var lowestID = 0;

            for (var i = 0; i < RunningTracks.Count + 1; i++)
            {
                var foundID = true;

                foreach (var track in RunningTracks)
                {
                    if (track.trackID == i)
                    {
                        foundID = false;
                        break;
                    }
                }

                if (foundID)
                {
                    lowestID = i;
                    break;
                }
            }

            return lowestID;
        }

        public void FadeTrack(string trackName, float fadeTo, float time, float offset = 0)
        {
            Track track = FindTrack(trackName);
            if (track.isFading) return;
            track.isFading = true;
            StartCoroutine(FadeOutTrackCoroutine(track, fadeTo, time, offset));
        }

        public void InterFade(string trackFadeOut, string trackFadeIn, float fadeTime, float offset)
        {
            FadeTrack(trackFadeOut, 0, fadeTime);
            FadeTrack(trackFadeIn, 1, fadeTime, offset);
        }

        IEnumerator FadeOutTrackCoroutine(Track track, float fadeTo, float time, float offset = 0)
        {
            yield return new WaitForSeconds(offset);

            float t = 0;
            float startVolume = track.TrackVolume;

            while (track.TrackVolume != fadeTo)
            {
                t += Time.deltaTime;

                track.TrackVolume = Mathf.Lerp(startVolume, fadeTo, Mathf.Clamp(t / time, 0, 1));
                yield return null;
            }

            if (fadeTo == 0)
            {
                track.StopPlaying();
            }
        }

        public async Task FadeTrackAsync(Track track, float fadeTo, float time)
        {
            float t = 0;
            float startVolume = track.TrackVolume;

            while (track.TrackVolume != fadeTo)
            {
                t += Time.deltaTime;

                track.TrackVolume = Mathf.Lerp(startVolume, fadeTo, Mathf.Clamp(t / time, 0, 1));
                await Task.Yield();
            }
        }
    }
}