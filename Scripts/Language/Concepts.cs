using System.Collections.Generic;

namespace Language
{
    public static class Concepts
    {
        private static Dictionary<string, Concept> concepts = new Dictionary<string, Concept>
        {
            {"age", new Concept("young", "old", Unit.Time)},
            {"width", new Concept("thin", "wide", Unit.Distance)},
            {"height", new Concept("short", "tall", Unit.Distance)},
            {"weight", new Concept("light", "heavy")},
            {"size", new Concept("small", "big")},
        };

        public struct Concept
        {
            public string[] bounds;
            public string unit;

            public Concept(string lowerBound, string upperBound, string unit = null)
            {
                bounds = new[] {lowerBound, upperBound};
                this.unit = unit;
            }
        }

        public static class Unit
        {
            public static string Time = "year";
            public static string Distance = "Metre";
            public static string Mass = "Kilogram";
        }

        public static Concept Get(string name) => concepts[name];
      
    }
}