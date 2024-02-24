
using System.Collections.Generic;
using UnityEngine;


namespace GaspDL.SaveSystem
{
    /*
    public class SceneData : MonoBehaviour, ISaveable
    {
        public PersistentID SceneID;

        public struct SaveData
        {
            public SavedData SavedInfo;
            public SavedData PersistentData;
            public SavedData TemporaryData;
        }

        private void Awake()
        {
            
        }
        public object CaptureState()
        {
            throw new System.NotImplementedException();
        }

        public void RestoreState(object state)
        {
            throw new System.NotImplementedException();
        }
        

    }

    */
    public class SavedData
    {
        public List<PersistentData<int>> Integer;
        public List<PersistentData<float>> Float;
        public List<PersistentData<bool>> Boolean;
        public List<PersistentData<Vector2>> Vector2;
    }
}

