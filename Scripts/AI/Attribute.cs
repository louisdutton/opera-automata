using System;
using Language;
using UnityEngine.UI;

namespace AI
{
    [Serializable]
    public class Attribute
    {
        public string name;
        public float value;

        public Attribute(string name, float value = default)
        {
            this.name = name;
            this.value = value;
        }
    }
}
