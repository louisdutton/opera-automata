using UnityEngine;
using System.Collections.Generic;
using Synthesis.Vocal;

namespace AI
{
    public enum Sexuality { Heterosexual, Homosexual, Bisexual }

    [System.Serializable]
    public class Character : Entity
    {
        public Personality personality;

        public override string ToString() => $"{Get<string>("name")}, {Get<string>("sex")}, {Get<float>("age")}";

        private GenerationSettings settings;
        
        public Character (GenerationSettings settings)
        {
            this.settings = settings;

            // Attributes
            var physicality = new[] {"width", "height", "weight", "size"};
            foreach (var p in physicality) Set(p, Random.value);
            Set("age", GenerateAge());
            Set("sex", GenerateSex());
            Set("sexuality", GenerateSexuality());
            Set("status", GenerateStatus());
            Set("name", GenerateName());
            
            personality = new Personality();
            personality.Generate();
            // relationships = new List<Relationship>();
            
            Debug.Log("Generated: " + this);
        }

        public Character(string name, float age, string sex, float weight, Vector3 size)
        {
            Set("name", name);
            Set("age", age);
            Set("sex", sex);
            
            // Physicality
            Set("width", (size.x + size.z) / 2 );
            Set("height", size.y);
            Set("size", size.magnitude);
            Set("weight", weight);
        }

        private float GenerateAge()
        {
            float interpolant = settings.ageDistribution.Evaluate(Random.Range(0f, 1f));
            return Mathf.Floor(Mathf.Lerp(settings.ageRange[0], settings.ageRange[1] + 1, interpolant));
        }
        
        private string GenerateSex()
        {
           return Random.value < settings.maleFemaleRatio ? "male" : "female";
        }
        
        private Sexuality GenerateSexuality()
        {
            if (Random.value < settings.heteroHomoRatio) return Sexuality.Heterosexual;
            if (Random.value < settings.homoBiRatio) return Sexuality.Homosexual;
            return Sexuality.Bisexual;
        }
        
        private string GenerateStatus()
        {
            return Language.Utils.ChooseRandom("low", "middle", "high");
        }

        private string GenerateName()
        {
            string name = string.Empty;

            while (!DataManager.dictionary.ContainsKey(name))
            {
                string[] namePool = Get<string>("sex") == "male" ? DataManager.names.male : DataManager.names.female;
                name = namePool[Random.Range(0, namePool.Length)];
            }

            return name;
        }

        public Voice.Type GenerateVoiceType()
        {
            var min = settings.ageRange[0];
            var max = settings.ageRange[1];
            var age = Get<float>("age");
            var sex = Get<string>("sex");
                
            int ageRange = max - min;

            if (sex == "male")
            {
                return age < min + ageRange / 2 ? (Voice.Type)Random.Range(1, 3) : Voice.Type.Bass;
            }
            return age < min + ageRange / 2 ? Voice.Type.Soprano : Voice.Type.Mezzo;
        }
    }
}
