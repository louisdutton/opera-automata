using AI;
using UnityEngine;

namespace Language
{
    public class NarrativeGenerator : MonoBehaviour
    {
        public bool generateOnStart = true;

        private Narrative narrative;

        private void Start()
        {
            if (generateOnStart) Generate();
        }

        public void Generate()
        {
            narrative = new Narrative();
            foreach (var c in narrative.characters) print(c);
        }
    }

    public class Narrative
    {
        public Character[] characters;
        
        public Exposition exposition;
        public Incident incident;
        public Rise rise;
        public Climax climax;
        public Fall fall;
        public Resolution resolution;
 
        public int length = 6;
        public Section this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return exposition;
                    case 1: return incident;
                    case 2: return rise;
                    case 3: return climax;
                    case 4: return fall;
                    case 5: return resolution;
                    default: return null;
                }
            }
        }

        public Narrative()
        {
            exposition = new Exposition();
            incident = new Incident();
            rise = new Rise();
            climax = new Climax();
            fall = new Fall();
            resolution = new Resolution();
        }
    }

    public class Section
    {
        public override string ToString()
        {
            return "Section";
        }
    }

    #region Sections

    public class Exposition : Section
    {
       
    }

    public class Incident : Section
    {

    }

    public class Rise : Section
    {

    }

    public class Climax : Section
    {

    }

    public class Fall : Section
    {

    }

    public class Resolution : Section
    {

    }

    #endregion
}
