using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GaspDL.Audio
{
    public static class AudioExtensions 
    {
        public static IAudioFile FindAudio(this IList<IAudioFile> list, string audioID)
        {
            var sound = Array.Find(list.ToArray(), sound => sound.SoundID == audioID);
            
            if (sound == null)
            {
                Debug.LogWarning("Sound : " + audioID + " Not Found.");
                return null;
            }

            return sound;
        }
        
        public static Music FindMusic(this IList<Music> list, string musicID)
        {
            var music = Array.Find(list.ToArray(), music => music.name == musicID);

            if (music == null)
            {
                Debug.LogWarning("Music : " + musicID + " Not Found.");
                return null;
            }

            return music;
        }
    }
}

