using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GaspDL.SaveSystem
{
    public static class SaveSystem
    {

        private static string SavePath => $"{Application.persistentDataPath}/";

        public static void SaveData(object data, string fileNme)
        {
            using (var stream = File.Open(SavePath + fileNme, FileMode.OpenOrCreate))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            };
        }

        public static object LoadData(string fileNme)
        {
            if (!File.Exists(SavePath + fileNme))
            {
                return null;
            }

            using (FileStream stream = File.Open(SavePath + fileNme, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }

        }
    }
}

