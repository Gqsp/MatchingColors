using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
namespace GaspDL.Audio
{
    [RequireComponent(typeof(TrackManager))]
    public class AudioManager : AudioPlayer
    {
        public static AudioManager Instance;
        #if ODIN_INSPECTOR
        [PropertyOrder(2)]
        #endif
        public AudioInstance audioPlayerPrefab;


        [Space]
        #if ODIN_INSPECTOR
        [Title("Volumes")]
        [PropertyOrder(3)]
        #endif
        [Range(0, 1)]
        public float masterVolume;
        
        #if ODIN_INSPECTOR
        [PropertyOrder(3)]
        #endif
        [Range(0, 1)]
        public float musicVolume, soundVolume;

        public new float MusicVolume => masterVolume * musicVolume;
        public new float SoundVolume => soundVolume * musicVolume;
        
        private Pool<AudioInstance> _audioPlayers;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _audioPlayers = new Pool<AudioInstance>(audioPlayerPrefab, transform);
            
            Setup();
        }
     
        public void PlaySoundAtPoint(Vector3 position, string soundID)
        {
            _audioPlayers.Get().PlayOnce(position, soundEffects.FindAudio(soundID));
        }

        public void PlaySoundAtPoint(Vector3 position, IAudioFile audio)
        {
            _audioPlayers.Get().PlayOnce(Vector3.positiveInfinity, audio);
        }

        public void PlaySoundGlobal(IAudioFile audio, float playTimes, float spatialBlend)
        {
            _audioPlayers.Get().PlayTimes(Vector3.zero, audio, playTimes, spatialBlend);
        }
    }
}