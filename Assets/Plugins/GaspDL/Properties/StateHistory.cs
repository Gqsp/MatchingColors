using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GaspDL
{
    public class StateHistory<T>
    {
        private readonly List<T> _history;
        private readonly List<T> _ignoreList;

        public StateHistory(List<T> ignoreList = null)
        {
            _history = new List<T>();
            _ignoreList = ignoreList ?? new List<T>();
        }

        public void Push(T history)
        {
            _history.Add(history);
        }
        
        public bool TryGetLast(out T last)
        {
            for (var i = _history.Count; i >= 0; i--)
            {
                if (_ignoreList.Contains(_history[i])) continue;
                last = _history[i];
                return true;
            }
    
            last = default;
            return false;
        } 
    }
}

