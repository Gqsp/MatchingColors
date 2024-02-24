using System.Collections.Generic;
using GaspDL.Audio;
using UnityEngine;

namespace GaspDL.Utils 
{
    public static class Extensions
    {
        public static T Random<T>(this IList<T> list)
        {
            if (list.Count == 0) return default;
            else if (list.Count == 1) return list[0];
            else
            {
                return list[UnityEngine.Random.Range(0, list.Count)];
            }
        }
        
        public static void DeleteChildren(this Transform t)
        {
            foreach (Transform child in t)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform t in  gameObject.transform) t.gameObject.SetLayerRecursively(layer);
        }

        public static void SetupFromSound(this AudioSource source, IAudioFile sound)
        {
            source.clip = sound.AudioClip;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}

