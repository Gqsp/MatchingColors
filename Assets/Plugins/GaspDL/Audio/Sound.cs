using GaspDL.Utils;
using UnityEngine;

namespace GaspDL.Audio
{
    public abstract class SoundStructure {
        protected AudioSource _source;
    
        [SerializeField] protected string soundName;
    }

    [System.Serializable]
    public class Sound : SoundStructure, IAudioFile
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private RangedFloat volume = new RangedFloat(1, 1);
        [SerializeField] private RangedFloat pitch = new RangedFloat(1, 1);
    
        public string SoundID => soundName;
        public AudioClip AudioClip => clip;
        public float Volume => volume.Value;
        public float Pitch => pitch.Value;
        public AudioSource Source
        {
            get => _source;
            set => _source = value;
        }
    }

    [System.Serializable]
    public class FixedSound : SoundStructure, IAudioFile
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] [Range(0, 2)] private float volume;
        [SerializeField] [Range(0, 2)] private float pitch;

        public string SoundID => soundName;
        public AudioClip AudioClip => clip;
        public float Volume => volume;
        public float Pitch => pitch;
        public AudioSource Source
        {
            get => _source;
            set => _source = value;
        }
    }

    [System.Serializable]
    public class SoundBank : SoundStructure, IAudioFile
    {
        [SerializeField] private AudioClip[] clip;
        [SerializeField] private RangedFloat volume = new RangedFloat(1, 1);
        [SerializeField] private RangedFloat pitch = new RangedFloat(1, 1);
        
        public string SoundID => soundName;
        public AudioClip AudioClip => clip.Random();
        public float Volume => volume.Value;
        public float Pitch => pitch.Value;
        public AudioSource Source
        {
            get => _source;
            set => _source = value;
        }
    }
}

