using System;
using System.Collections.Generic;
using UnityEngine;

namespace GaspDL.SaveSystem
{
    public class PersistentID
    {
        public static List<PersistentID> UsedIds;

        [SerializeField]
        private string _id = string.Empty;

        public string ID => _id;

        [ContextMenu("Generate Id")]
        public void GenerateUniqueID()
        {
            //return gameObject.scene + "_" + gameObject.name + "_" + gameObject.transform.GetSiblingIndex();
            _id = Guid.NewGuid().ToString();
        }
    }
}
