using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class Relationship
    {
        public enum Type { Blood, Platonic, Romantic }
        public enum Strength { Weak, Average, Strong }

        public Type type;
        public Strength strength;
        public Character[] members;

        public Relationship(Type type, Character a, Character b)
        {
            this.type = type;
            strength = (Strength)Random.Range(0, 3);
            members = new Character[] { a, b };

            // Cache in members
            // a.relationships.Add(this);
            // b.relationships.Add(this);
        }

        public override string ToString()
        {
            string tail = null;
            switch (type)
            {
                case Type.Blood: tail = "related"; break;
                case Type.Platonic: tail = "friends"; break;
                case Type.Romantic: tail = "in a relationship"; break;
            }

            return $"{members[0].Get<string>("name")} & {members[1].Get<string>("name")} are {tail}.";
        }
    }
}
