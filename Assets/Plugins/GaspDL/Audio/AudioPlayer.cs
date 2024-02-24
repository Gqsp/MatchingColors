using System.Threading.Tasks;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GaspDL.Audio
{
    [RequireComponent(typeof(TrackManager))]
    public class AudioPlayer : MonoBehaviour
    {
        #if ODIN_INSPECTOR
        [Title("References")]
        [PropertyOrder(1)] 
        #endif
        [SerializeField] protected TrackManager trackManager;
        [Space] 
        
        #if ODIN_INSPECTOR
        [Title("Audios")] [TableList(ShowIndexLabels = true)]
        [PropertyOrder(4)] 
        #endif
        [SerializeField] protected Music[] musics;
        [Space]
        
        #if ODIN_INSPECTOR
        [PropertyOrder(4)] 
        #endif
        [SerializeField] protected SoundBank[] soundEffects;

        public float MusicVolume {
            get
            {
                float masterVolume = 1;
                if (AudioManager.Instance != null) masterVolume = AudioManager.Instance.MusicVolume;
                return masterVolume;
            }
        }
        public float SoundVolume {
            get
            {
                float masterVolume = 1;
                if (AudioManager.Instance != null) masterVolume = AudioManager.Instance.SoundVolume;
                return masterVolume;
            }
        }
    
        private void Awake()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            foreach (var sfx in soundEffects)
            {
                sfx.Source = gameObject.AddComponent<AudioSource>();
                sfx.Source.clip = sfx.AudioClip;
                sfx.Source.volume = sfx.Volume;
                sfx.Source.pitch = sfx.Pitch;
            }
            
            UpdateVolume();
        }
        
        public void Play(string musicID, int trackID = 0)
        {
            var track = trackManager.GetOrCreate(trackID);
            var music = musics.FindMusic(musicID);
            track.PlayNow(music);
        }
    
        public void Play(string musicID, string trackName)
        {
            var track = trackManager.GetOrCreate(trackName);
    
            var music = musics.FindMusic(musicID);
    
            track.PlayNow(music);
        }
        
        public async void SmoothPlay(string musicID, string trackName, float fadeTime, float offset)
        {
            var runningTrack = trackManager.GetOrCreate(trackName);
            
            await trackManager.FadeTrackAsync(runningTrack, 0, fadeTime);

            await Task.Delay((int)(offset * 1000));
            
            runningTrack.PlayNow(musics.FindMusic(musicID));

            await trackManager.FadeTrackAsync(runningTrack, 1, fadeTime);
        }
            
        public void PlayOrRestart(string musicID, int track = 0)
        {
        }
    
        public void PlaySound(string soundID)
        {
            if (soundEffects.FindAudio(soundID) is { } sound)
            {
                sound.Source.clip = sound.AudioClip;
                sound.Source.volume = sound.Volume * SoundVolume;
                sound.Source.pitch = sound.Pitch;
                sound.Source.Play();
            }
        }

        public void StopPlaying(string musicID)
        {
            var track = trackManager.FindTrackPlayingMusic(musicID);
            if (track != null)
            {
                track.StopPlaying();
            }
        }

        public void StopAllMusic()
        {
            trackManager.StopAllTracks();
        }

        public void StopMusicSmooth(string track, float fadeTime)
        {
            trackManager.FadeTrack(track, 0, fadeTime);
        }

        private void UpdateVolume()
        {
            foreach (var track in trackManager.RunningTracks)
            {
                track.TrackVolume = MusicVolume * (track.currentlyPlaying?.volume ?? 1);
            }
        }
    }
}

