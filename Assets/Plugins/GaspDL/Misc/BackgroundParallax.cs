using UnityEngine;
using UnityEngine.Serialization;

namespace GaspUtils.Background
{
    public class BackgroundParallax : MonoBehaviour
    {
        private float _length, _startPos;
        private Transform _cameraObject;
        
        public float parallaxEffect;
        public bool neverRepeat;

        private void Start()
        {
            _startPos = transform.position.x;
            if (!neverRepeat)
            {
                _length = GetComponent<SpriteRenderer>().bounds.size.x;
            }
        }

        private void FixedUpdate()
        {
            var position = _cameraObject.position;
            var temp = (position.x * (1 - parallaxEffect));
            var dist = (position.x * parallaxEffect);

            transform.position = new Vector3(_startPos + dist, transform.position.y, transform.position.z);

            if (neverRepeat) return;
            if (temp > _startPos + _length) _startPos += _length;
            else if (temp < _startPos - _length) _startPos -= _length;
        }
    }
}

