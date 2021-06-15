using System.Collections;
using Language;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class Entity
    {
        protected Dictionary<string, object> attributes = new Dictionary<string, object>
        {
            {"name", "John"},
            {"age", 10},
            {"width", 0},
            {"height", 0},
            {"weight", 0},
            {"size", 0},
        };

        public T Get<T>(string key) => (T)attributes[key];
        public void Set(string key, object value) => attributes[key] = value;
       
    }
    
}