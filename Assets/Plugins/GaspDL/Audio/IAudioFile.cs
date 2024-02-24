using UnityEngine;

namespace GaspDL.Audio
{
    public interface IAudioFile
    {
        public string SoundID { get; }
        public AudioClip AudioClip { get; }
        public float Volume { get; }
        public float Pitch { get; }
        public AudioSource Source { get; set; }
    }
}

