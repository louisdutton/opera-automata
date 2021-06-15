using System;
using System.Collections.Generic;
using System.Linq;
using AI;

namespace Language
{
    public static class Utils
    {
        public static readonly char[] Vowels = {'i', 'I', '@', 'e', '3', '0', 'O', '&', 'U', 'u', 'A', 'a', 'V'};
        public static readonly char[] LetterVowels = {'a', 'e', 'i', 'o', 'u'};
        
        // Plurals
        public static class Plurals
        {
            public static readonly string[] ES = { "s", "x", "z", "sh", "ch" };
            public static readonly string[] IES = { "y" };
        }

        // Choose Random
        public static T ChooseRandom<T>(T a, T b) => ChooseRandom(new[] {a, b});
        public static T ChooseRandom<T>(T a, T b, T c) => ChooseRandom(new[] {a, b, c});
        public static T ChooseRandom<T>(T[] options)
        {
            var rand = new System.Random();
            var choice = rand.Next(options.Length);;
            return options[choice];
        }

        public static string Capitalise(this string word) => char.ToUpper(word[0]) + word.Substring(1);

        public static string Pluralise(this string word)
        {
            // last chars
            var l1 = word.Substring(word.Length-1);
            var l2 = word.Substring(word.Length-2);

            if (Plurals.ES.Contains(l1) || Plurals.ES.Contains(l2)) return word + "es";
            if (Plurals.IES.Contains(l1))   return word + "ies";
            return word + "s";
        }

        public static string Comparative(this string word)
        {
            var last = word[word.Length-1];
            var second = word[word.Length-2];

            if (last == 'y') return word.Remove(word.Length-1) + "ier";
            if (last == 'e') return word + 'r';
            if (!last.IsVowel() && second.IsVowel()) word += last;
            return word + "er";
        }
        
        public static string Superlative(this string word)
        {
            var last = word[word.Length-1];
            var second = word[word.Length-2];

            if (last == 'y') return word.Remove(word.Length-1) + "iest";
            if (last == 'e') return word + "st";
            if (!last.IsVowel() && second.IsVowel()) word += last;
            return word + "est";
        }
        
        // public static Word[] Describe(Attribute<float> a)
        // {
        //     
        // }

        public static bool IsVowelPhoneme(char phoneme) => Vowels.Contains(phoneme);
        
        public static bool IsVowel(this char letter) => LetterVowels.Contains(letter);

        public static string PhonemeToSound(char phoneme)
        {
            string sound;

            // Vowels
            if (IsVowelPhoneme(phoneme))
            {
                switch (phoneme)
                {
                    case 'A': case '&':
                        sound = "a"; break;
                    case 'e':
                        sound = "e"; break;
                    case 'I': case 'i':
                        sound = "i"; break;
                    case 'O': case '@': case '0':
                        sound = "o"; break;
                    case 'U': case 'u':
                        sound = "u"; break;
                    default:
                        sound = "a"; break;
                }
            }

            // Consonants
            else
            {
                switch (phoneme)
                {
                    case 'b':
                        sound = "b"; break;
                    case 'k':
                        sound = "c"; break;
                    case 'h':
                        sound = "h"; break;
                    case 'd':
                        sound = "d"; break;
                    case 'g':
                        sound = "g"; break;
                    case 'n':
                        sound = "n"; break;
                    case 'N':
                        sound = "ng"; break;
                    case 'm':
                        sound = "m"; break;
                    case 's':
                        sound = "s"; break;
                    case 'S':
                        sound = "sh"; break;
                    case 'D': case 'T':
                        sound = "th"; break;
                    case 't':
                        sound = "t"; break;
                    case 'R':
                        sound = "r"; break;
                    case 'w': case 'v':
                        sound = "v"; break;
                    case 'l':
                        sound = "l"; break;
                    case 'f':
                        sound = "f"; break;
                    case 'z':
                        sound = "s"; break;
                    case 'p':
                        sound = "p"; break;
                    case 'j':
                        sound = "y"; break;
                    default:
                        sound = "t"; break;
                }
            }

            return sound;
        }

        public static bool IsSyllableStart(char phoneme, char previousPhoneme)
        {
            var p = phoneme;
            var prev = previousPhoneme;
            return IsVowelPhoneme(p) && (!IsVowelPhoneme(prev) || prev == 'I') || p == 'I' && prev != 'e' && prev != 'a';
        }
    }
}
