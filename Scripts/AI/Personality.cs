using System;
using Random = UnityEngine.Random;

namespace AI
{
    [System.Serializable]
    public class Personality
    {
        private enum T { Mind, Energy, Nature, Tactics, Identity }
        public enum Role { Analyst, Idealist, Pragmatist, Explorer }
        public enum Type
        {
            INTJ, INTP, ENTJ, ENTP, // Analyst
            INFJ, INFP, ENFJ, ENFP, // Idealist
            ISTJ, ISFJ, ESTJ, ESFJ, // Pragmatist
            ISTP, ISFP, ESTP, ESFP  // Explorer
        }

        public Role role;
        public Type type;
    
        public Trait[] traits;

        public Trait mind => traits[0];
        public Trait energy => traits[1];
        public Trait nature => traits[2];
        public Trait tactics => traits[3];
        public Trait identity => traits[4];

        public Personality()
        {
            traits = new Trait[5];
            for (int i = 0; i < 5; i++)
            {
                traits[i] = new Trait(((T)i).ToString());
            }
        }

        public void Generate()
        {
            for (int i = 0; i < 5; i++)
            {
                traits[i] = new Trait(((T)i).ToString(), Random.value);
            }
            Classify(); 
        }

        private void Classify()
        {
            var i = mind.value < .5f;
            var e = !i;

            var s = energy.value < .5f;
            var n = !s;

            var t = nature.value < .5f;
            var f = !t;

            var j = tactics.value < .5f;
            var p = !j;

            // Determine Role
            if (n) role = t ? Role.Analyst : Role.Idealist;
            else role = j ? Role.Pragmatist : Role.Explorer;

            // Determine Type
            var row = (int)role;
            var columnStart = i ? 0 : 2;
            var determinant = row <= 2 ? j : t;
            var increment = Convert.ToInt32(!determinant);
            type = (Type)(row * 4 + columnStart + increment);
        }

        [System.Serializable]
        public class Trait : Attribute
        {
            public Trait(string name, float value = 0) : base(name, value) {}
        }
    }
}