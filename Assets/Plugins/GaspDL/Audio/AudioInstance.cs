using GaspDL.Utils;
using UnityEngine;

namespace GaspDL.Audio
{
    public class AudioInstance : MonoBehaviour
    {
        public AudioSource audioSource;
        private float remainingPlays;
    
        public void PlayOnce(Vector3 position, IAudioFile audioFile, float spatial = 1)
        {
            remainingPlays = 1;
            PlaySound(position, audioFile, spatial);
        }

        public void PlayTimes(Vector3 position, IAudioFile audioFile, float times, float spatial = 1)
        {
            remainingPlays = times;
            PlaySound(position, audioFile, spatial);
        }

        private void PlaySound(Vector3 position, IAudioFile audioFile, float spatial)
        {
            if (audioFile == null) return;
            transform.position = position;
            gameObject.SetActive(true);
            audioSource.SetupFromSound(audioFile);
            audioSource.spatialBlend = spatial;
            audioSource.Play();
        }

        private void Update()
        {
            if (!audioSource.isPlaying)
            {
                remainingPlays--;
                if (remainingPlays <= 0)
                    gameObject.SetActive(false);
                else 
                    audioSource.Play();
            }
        }
    }
}
