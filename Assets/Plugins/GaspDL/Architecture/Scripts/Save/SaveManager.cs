using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GaspDL.SaveSystem
{
    public class SaveManager : ManagerMonoBehavior
    {
        public int fileIndex;
        public const string fileName = "ReapFox_Save_";

        private List<SavedDataStructure> SavedData = new List<SavedDataStructure>();
        private List<SavedDataStructure> UnsavedData = new List<SavedDataStructure>();
        private List<SavedDataStructure> TemporaryData = new List<SavedDataStructure>();

        private List<ISaveable> SceneData = new List<ISaveable>();

        public void Subscribe(ISaveable data)
        {
            SceneData.Add(data);
        }

        private void Awake()
        {
            // LOAD ALL DATA IN SAVED DATA
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            // FIRST CHECK FOR OUT OF RANGE DATA IN TEMPORARY DATA
            foreach (var data in TemporaryData)
            {
                // IF DATA OUT OF RANGE THEN REMOVE IT
                if (data.identifier.RememberDistance.Enabled && data.identifier.SceneName.Enabled)
                {
                    if (CalculateDistanceBetweenScenes(data.identifier.SceneName.Value, SceneManager.GetActiveScene().name) > data.identifier.RememberDistance.Value)
                    {
                        TemporaryData.Remove(data);
                    }
                }
            }

            // THEN TRY LOAD DATA FOR ALL ISAVEABLE THAT SUBSCRIBED TO SAVEMANAGER ON AWAKE
            foreach (var data in SceneData)
            {
                // FIRST LOOK FOR TEMPORARY DATA
                if (data.DataType == DataType.Temporary)
                {
                    // CHECK IF DATA IS SAVED IN TEMPORARY DATA
                }
                // THEN CHECK FOR PERSISTENT, CHECK UNSAVED DATA FIRST TO NOT OVERWRITE UNSAVED DATA (UNSAVED DATA IS NEWER DATA THAN SAVED ONE)
                else if ( data.DataType == DataType.Persistent)
                {
                    if (UnsavedData.Exists(d => d.identifier == data.ID))
                    {
                        var unsavedData = UnsavedData.Find(d => d.identifier == data.ID);
                        data.RestoreState(unsavedData.capturedData);
                    } else if (SavedData.Exists(d => d.identifier == data.ID))
                    {
                        var savedData = SavedData.Find(d => d.identifier == data.ID);
                        data.RestoreState(savedData.capturedData);
                    }
                }
            }
        }


        // When Scene is Unloaded, Remember necessary data into different categories
        protected override void OnSceneUnloaded(Scene scene)
        {
            foreach (var data in SceneData)
            {
                switch (data.DataType)
                {
                    case DataType.Persistent:
                        UnsavedData.Add(GetDataStructure(data));
                        break;
                    case DataType.Temporary:
                        TemporaryData.Add(GetDataStructure(data));
                        break;
                    default:
                        break;
                }

                SceneData.Remove(data);
            }
        }

        [ContextMenu("SaveData")]
        public void OnSaveData()
        {
            foreach (var data in UnsavedData)
            {
                int index = SavedData.FindIndex(d => d.identifier.ID == data.identifier.ID);

                if (index >= 0)
                {
                    SavedData[index] = data;
                } else
                {
                    SavedData.Add(data);
                }
            }

            SaveSystem.SaveData(SavedData, GetFileName(fileIndex));

            // Clear other 
            List<SavedDataStructure> temp = TemporaryData.FindAll(d => d.identifier.SceneName.Value == SceneManager.GetActiveScene().name);
            TemporaryData = new List<SavedDataStructure>();
            TemporaryData.AddRange(temp);
            UnsavedData = new List<SavedDataStructure>();
        }

        [ContextMenu("LoadData")]
        public void LoadData(int saveId)
        {
            object data = SaveSystem.LoadData(GetFileName(saveId));

            SavedData = (List<SavedDataStructure>)data;

            fileIndex = saveId;
        }

        public SavedDataStructure GetDataStructure(ISaveable data)
        {
            return new SavedDataStructure
            {
                identifier = data,
                capturedData = data.CaptureState(),
            };
        }

        public string GetFileName(int saveId)
        {
            return fileName + saveId.ToString();
        }

        public int CalculateDistanceBetweenScenes(string a, string b)
        {
            // TO DO
            return 2;
        }
    }

    public struct SavedDataStructure
    {
        public ISaveable identifier;
        public object capturedData;
    }
}

