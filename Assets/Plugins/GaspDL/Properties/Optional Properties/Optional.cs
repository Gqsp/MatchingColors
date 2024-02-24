using System;
using UnityEngine;

namespace GaspDL
{
    [Serializable]
    public struct Optional<T>
    {
        [SerializeField] private bool enabled;
        [SerializeField] private T value;

        public bool Enabled { get { return enabled; } set { enabled = value; } }
        public T Value { get { return value; } set { this.value = value; } }

        public Optional(T initialValue)
        {
            enabled = true;
            value = initialValue;
        }
    }
}

