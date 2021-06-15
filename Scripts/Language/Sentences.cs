using System.Linq;
using UnityEngine;
using AI;

namespace Language
{
    using static Utils;
    
    public class Greeting : Sentence
    {
        private static readonly string[] Greetings = {"hello", "hi", "hey"};

        private const float ChanceToReferenceTime = .33f;

        public Greeting(Singer target)
        {
            string time;
            bool useName = true;

            int hour = System.DateTime.Now.Hour;

            if (hour > 16) time = "evening";
            else if (hour > 11) time = "afternoon";
            else time = "morning";

            if (Random.value <= ChanceToReferenceTime)
            {
                words.Add(new Word("good"));
                words.Add(new Word(time));
            }
            else
            {
                words.Add(new Word(Greetings[Random.Range(0, Greetings.Length)]));
            }

            if (useName) words.Add(new Noun(target.name, true));

            Recalculate();
        }
    }

    public class Introduction : Sentence
    {
        public Introduction(Character source)
        {
            words.Add(new Pronoun(Pronoun.I));
            words.Add(new Noun("name"));
            words.Add(new Auxiliary(Auxiliary.Is, Tense.Present));
            words.Add(new Noun(source.Get<string>("name"), true));

            Recalculate();
        }
    }

    public class Description : Sentence
    {
        public Description(Entity e, Entity source, Tense tense = Tense.Present)
        {
            var noun = e == source ? new Pronoun(Pronoun.I) : new Noun(e.Get<string>("name"), true);
            
            subject = new NounPhrase(noun);
            predicate = new Predicate();
            predicate.Add(new Auxiliary(Auxiliary.Is, tense));

            // Adjective
            if (e.GetType() == typeof(Character))
            {
                var c = (Character) e;
                switch (Random.Range(0, 3))
                {   
                    case 0: predicate.Add(new Age((int)e.Get<float>("age"))); break; // create generic method to describe
                    case 1: predicate.Add(new Word(c.Get<string>("sex"))); break;
                    case 2: predicate.Add(new Word(c.Get<string>("sexuality"))); break;
                }
            }

            Recalculate();
        }
    }

    public class Comparison : Sentence
    {
        public Comparison(Entity a, Entity b, Tense tense = Tense.Present)
        {
            var noun = a == b ? new Pronoun(Pronoun.I) : new Noun(a.Get<string>("name"), true);

            subject = new NounPhrase(noun);
            predicate = new Predicate();
            predicate.Add(new Auxiliary(Auxiliary.Is, tense));

            // Adjective
            if (a.GetType() == typeof(Character))
            {
                var options = new[] {"age", "height", "width", "weight", "size"};
                var choice = options[Random.Range(0, 5)];
                Debug.Log(choice);
                var values = new[] { a.Get<float>(choice), b.Get<float>(choice) };
                var concept = Concepts.Get(choice);
                var bound = values[0] > values[1] ? concept.bounds[0] : concept.bounds[1];

                predicate.Add(bound.Comparative());
                predicate.Add(new Word("than"));
                predicate.Add(new Pronoun(Pronoun.I, Case.Objective));
            }

            Recalculate();
        }

    }
}
