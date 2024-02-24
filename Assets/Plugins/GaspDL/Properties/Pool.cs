using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GaspDL
{
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly Transform _parent;

        public List<T> Instances { get; }

        public Pool(T prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
            Instances = new List<T>();

            Instances.AddRange(parent.GetComponentsInChildren<T>());
        }
    
        public T Get()
        {
            foreach (var instance in Instances)
            {
                if (!instance.isActiveAndEnabled)
                {
                    instance.gameObject.SetActive(true);
                    return instance;
                }
            }

            T i = Object.Instantiate(_prefab, _parent);
            Instances.Add(i);

            return i;
        }

        public void Add(T instance)
        {
            if (Instances.Contains(instance)) return;
            Instances.Add(instance);
        }

        public List<T> GetLiveInstances()
        {
            List<T> list = new List<T>();
            list.AddRange(Instances.Where(i => i.isActiveAndEnabled)); 
            return list;
        }
    }
}

