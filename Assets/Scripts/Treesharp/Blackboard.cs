using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using UnityEngine;

namespace InterativaSystem.Treesharp
{
    public class Blackboard
    {
        public class Memory
        {
            public Memory()
            {
                
            }
            public Memory(string key)
            {
                this.key = key;
                value = null;
            }

            public string key;
            public object value;

            public void Set(string key, object value)
            {
                this.key = key;
                this.value = value;
            }
        }

        //private Memory _baseMemory ;
        private Dictionary<string, List<Memory>> _treeMemory ;

        public Blackboard()
        {
            //Init here
            Initialize();
        }
        protected virtual void Initialize()
        {
            //_baseMemory = new Memory();
            _treeMemory = new Dictionary<string, List<Memory>>();
        }

        public void Set(string key, object value, string treeScope, string nodeScope)
        {
            var memory = GetMemory(key, treeScope, nodeScope);
            memory.Set(key, value);
        }
        public void Set(string key, object value, string treeScope)
        {
            var memory = GetMemory(key, treeScope, null);
            memory.Set(key, value);
        }

        public Memory Get(string key, string treeScope, string nodeScope)
        {
            var memory = GetMemory(key, treeScope, nodeScope);

            if (memory.key == key)
                return memory;
            else
            {
                Debug.LogError("Notfound: " + key);
                return memory;
            }
        }
        public Memory Get(string key, string treeScope)
        {
            var memory = GetMemory(key, treeScope, null);

            if (memory.key == key)
                return memory;
            else
            {
                Debug.LogError("Notfound: " + key);
                return memory;
            }
        }

        protected Memory GetTreeMemory(string key, string treeScope)
        {
            if (!_treeMemory.ContainsKey(treeScope))
            {
                Debug.Log("Adding " + treeScope);
                _treeMemory.Add(treeScope, new List<Memory>());
            }
            if(!_treeMemory[treeScope].Exists(x => x.key == key))
            {
                Debug.Log("Adding on " + treeScope + " key: " + key);
                _treeMemory[treeScope].Add(new Memory(key));
            }

            return _treeMemory[treeScope].Find(x => x.key == key);
        }

        protected Memory GetNodeMemory(string key, Dictionary<string, List<Memory>> treeMemory, string nodeScope)
        {
            if (!treeMemory.ContainsKey(nodeScope))
            {
                Debug.Log("Adding " + nodeScope);
                treeMemory.Add(nodeScope, new List<Memory>());
            }
            else if (!_treeMemory[nodeScope].Exists(x => x.key == key))
            {
                treeMemory[nodeScope].Add(new Memory(key));
            }

            return treeMemory[nodeScope].Find(x => x.key == key);
        }

        protected Memory GetMemory(string key, string treeScope, string nodeScope)
        {
            var memory = GetTreeMemory(key, treeScope);

            if (nodeScope != null && _treeMemory.ContainsKey(nodeScope))
            {
                memory = GetNodeMemory(key, _treeMemory, nodeScope);
            }
 
            return memory;
        }
    }
}