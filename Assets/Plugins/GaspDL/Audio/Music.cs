using UnityEngine;

namespace GaspDL.Audio
{
    [System.Serializable]
    public class Music
    {
        public string name;
    
        public AudioClip intro;
        public AudioClip body;
        public bool loopBody = true;
        [Range(0, 2)] public float volume = 1;
    }
}

