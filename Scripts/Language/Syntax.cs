using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Language
{
    using static Utils;

    public abstract class Structure
    {
        public List<Word> words = new List<Word>();

        public int Size => words.Count;

        public void Add(Structure s) => words.AddRange(s.words);
        public void Add(Word w) => words.Add(w);
        public void Add(string str) => Add(new Word(str));
        public void Prepend(Structure s) => words.InsertRange(0, s.words);
        public void Prepend(Word w) => words.Insert(0, w);
        public void Prepend(string str) => Prepend(new Word(str));

        public static List<Word> operator +(Structure a, Structure b)
        {
            a.words.AddRange(b.words);
            return a.words;
        }

        public override string ToString()
        {
            string str = null;
            foreach (Word word in words) str += $" {word.str}";
            return str;
        }
    }

    public class VerbPhrase : Structure
    {
        public Verb mainVerb;
    }

    public class NounPhrase : Structure
    {
        public Noun noun;

        public NounPhrase(Noun noun, bool isSpecific = false)
        {
            this.noun = noun;

            if (!noun.isProper) words.Add(new Article(noun, isSpecific));
            words.Add(noun);
        }
    }

    public class Predicate : Structure
    {

    }

    public class Sentence : Structure
    {
        public Structure subject;
        public Structure predicate;

        public Sentence() {}

        // public static Sentence FromString(string str)
        // {
        //     var sentence = new Sentence();
        //     var text = new string(str.Where(c => !char.IsPunctuation(c)).ToArray());
        //     string[] strWords = text.Split(' ');
        //
        //     foreach (string word in strWords) { sentence.words.Add(new Noun(word)); }
        //
        //     return sentence;
        // }

        public void Recalculate()
        {
            words = subject + predicate;
            CheckGrammar();
            Syllabicate();
        }

        protected void CheckGrammar() => words[0].SetCapitalised(true);

        protected void Syllabicate()
        {
            foreach (Word word in words) word.Syllabicate();
        }
    }

    // Operators
    // public static Clause operator +(Clause a, Clause b)
    // {
    //     if (a.tense == b.tense && a.verb == b.verb)
    //     {
    //         var index = b.words.FindIndex(word => word.GetType() == typeof(Verb));
    //         a.words.Add(Conjunction.And);
    //         a.words.AddRange(b.words.GetRange(index, b.words.Count - 1));
    //         return a;
    //     }
    //     
    //     a.words.Add(Conjunction.And);
    //     a.words.AddRange(b.words);
    //     return a;
    // }
    //
    // public static Clause operator |(Clause a, Clause b)
    // {
    //     a.words.Add(Conjunction.Or);
    //     a.words.AddRange(b.words);
    //     return a;
    // }
    //
    // public static Clause operator !(Clause a)
    // {
    //     var index = a.words.FindIndex(w => w.GetType() == typeof(Verb)) + 1;
    //     a.words.Insert(index, Adverb.Not);
    //     return a;
    // }

    // Make this a generic function to describe a numerical attribute !!!
    public class Age : Structure
    {
        public Age(int age, bool yearsOld = true)
        {
            Add(new Number(age));

            if (yearsOld)
            {
                Add("years");
                Add("old");
            }
        }
    }

    public class Number : Structure
    {
        public static string[] Units =
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
            "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
        };
        
        public static string[] Tens =
        {
            "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
        };
        
        public Number(int n)
        {
            if (n == 0) { Add("zero"); return; }

            // if (number < 0) return "minus " + NumberToWords(Mathf.Abs(number));

            if (n / 1000000 > 0)
            {
                Add(new Number(n / 1000000));
                Add("million");
                n %= 1000000;
            }

            if (n / 1000 > 0)
            {
                Add(new Number(n / 1000));
                Add("thousand");
                n %= 1000;
            }

            if (n / 100 > 0)
            {
                Add(new Number(n / 100));
                Add("hundred");
                n %= 100;
            }

            if (n > 0)
            {
                if (Size > 0) Add(Conjunction.And);

                if (n < 20) Add(Units[n]);
                else
                {
                    Add(Tens[n / 10]);
                    if ((n % 10) > 0) Add(Units[n % 10]); // change back to "-"
                }
            }
        }
    }
}



