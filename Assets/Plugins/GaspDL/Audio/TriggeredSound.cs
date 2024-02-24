using UnityEngine;

namespace GaspDL.Audio
{
    public class TriggeredSound : MonoBehaviour
    {
        public SoundBank OnTriggeredAudio;
        public bool playOnce;
        public float minDelayBetweenPlay;
        public bool playOnEnter, playOnExit, playOnStay;
        public float playOnStayAfterTime;
    
        private float _lastPlayTime;
        private void OnTriggerEnter2D(Collider2D col)
        {
            _stayTime = 0;
            if (!playOnEnter) return;
            PlaySound();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _hasPlayedStay = false;
            if (!playOnExit) return;
            PlaySound();
        }

        private float _stayTime = 0;
        private bool _hasPlayedStay;
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!playOnStay || _hasPlayedStay) return;

            _stayTime += Time.deltaTime;
            if (_stayTime < playOnStayAfterTime) return;
            PlaySound();
            _hasPlayedStay = true;
        }

        private void PlaySound()
        {
            if (minDelayBetweenPlay + _lastPlayTime > Time.time) return;
        
            AudioManager.Instance.PlaySoundAtPoint(transform.position, OnTriggeredAudio);
        
            if (playOnce) Destroy(this);

            _lastPlayTime = Time.time;
        }
    }
}

