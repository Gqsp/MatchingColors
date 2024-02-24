using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GaspDL.SaveSystem
{
    public interface ISaveable
    {
        [HideInInspector]
        public  PersistentID ID { get; }

        [SerializeField]
        public DataType DataType { get; set; }
        [SerializeField]
        public Optional<int> RememberDistance { get; set; }
        public Optional<string> SceneName { get; set; }

        object CaptureState();
        public void RestoreState(object state);
    }
}

[System.Serializable]
public enum DataType
{
    Persistent,
    Temporary,
    Scene,
}

