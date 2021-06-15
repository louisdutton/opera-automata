using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI;
using UnityEngine;
using Random = System.Random;

namespace Language
{
    using static Utils;

    // public class Inflection
    // {
    //     x
    // }

    public class Preposition : Word
    {
        public static Preposition To = new Preposition("to");
        
        // Locational
        public static Preposition In = new Preposition("in");
        public static Preposition At = new Preposition("at");
        public static Preposition On = new Preposition("on");
        
        // spatial
        public static Preposition Off = new Preposition("off");
        public static Preposition Across = new Preposition("across");
        public static Preposition Along = new Preposition("along");
        public static Preposition Behind = new Preposition("behind");
        public static Preposition Toward = new Preposition("toward");

        // above or below
        public static Preposition Over = new Preposition("over");
        public static Preposition Above = new Preposition("above");
        public static Preposition Below = new Preposition("below");
        public static Preposition Beneath = new Preposition("beneath");
        public static Preposition Under = new Preposition("under");
        public static Preposition Underneath = new Preposition("underneath");
        
        // near
        public static Preposition By = new Preposition("by");
        public static Preposition Near = new Preposition("near");
        public static Preposition Beside = new Preposition("beside");
        public static Preposition Opposite = new Preposition("opposite");
        
        // inside
        public static Preposition Between = new Preposition("between");
        public static Preposition Inside = new Preposition("inside");
        public static Preposition Within = new Preposition("opposite");
        public static Preposition Among = new Preposition("among");
        
        // temporal
        public static Preposition Since = new Preposition("since");
        public static Preposition For = new Preposition("for");
        public static Preposition From = new Preposition("from");
        public static Preposition During = new Preposition("during");

        public Preposition(string str) : base(str) {}
    }
    
    public class Conjunction : Word
    {
        // Coordinating 
        public static Conjunction For => new Conjunction("for");
        public static Conjunction And => new Conjunction("and");
        public static Conjunction Or => new Conjunction("or");
        public static Conjunction Nor => new Conjunction("nor");
        public static Conjunction But => new Conjunction("but");
        public static Conjunction Yet => new Conjunction("yet");
        public static Conjunction So => new Conjunction("so");

        // Temporal
        public static Conjunction Before => new Conjunction("before");
        public static Conjunction After => new Conjunction("after");
        public static Conjunction Now => new Conjunction("now");
        public static Conjunction Until => new Conjunction("until");
        public static Conjunction When => new Conjunction("when");
        public static Conjunction While => new Conjunction("while");
        public static Conjunction Since => new Conjunction("since");

        private Conjunction(string str) : base(str) {}
    }
    
    public class Noun : Word
    {
        public Case Case;
        public bool isProper;
        public bool isPlural;

        // regular noun
        public Noun(string name, bool isProper = false, bool isPlural = false, Case @case = Case.Nominative) : base(name)
        {
            this.isProper = isProper;
            this.isPlural = isPlural;
            Case = @case;
            
            SetCapitalised(isProper);
            if (isPlural) str.Pluralise();
        }

        protected Noun(string str) : base(str) {}
    }
    
    public class Pronoun : Noun
    {
        public static string[] I = {"I", "me", "mine"};
        public static string[] You = {"you", "you", "yours"};
        public static string[] He = {"he", "him", "his"};
        public static string[] She = {"she", "her", "hers"};
        public static string[] It = {"it", "it"};
        public static string[] We = {"we", "us", "ours"};
        public static string[] They = {"they", "them", "theirs"};

        public static Pronoun Calculate(Entity entity, Case @case)
        {
            var sex = entity.Get<string>("sex");
            string[] pronoun;
            if (sex != null) pronoun = sex.Equals("male") ? He : She;
            else pronoun = It;
            return new Pronoun(pronoun, @case);
        }
        
        public Pronoun(string[] type, Case @case = Case.Nominative) : base(type[(int) @case])
        {
            isProper = true;
        }
    }

    public class Article : Word
    {
        public Article(Noun noun, bool isSpecific) : base(Calculate(noun, isSpecific)) {}

        private static string Calculate(Noun noun, bool isSpecific)
        {
            if (isSpecific) return noun.IsPlural ? "the" : "this";
            return noun.StartsWithVowel ? "an" : "a";
        }
    }

    public class Intensifier
    {
        public static readonly Adverb Extremely = new Adverb("extremely");
        public static readonly Adverb Very = new Adverb("very");
        public static readonly Adverb Really = new Adverb("really");
        public static readonly Adverb Quite = new Adverb("quite");
        public static readonly Adverb Moderately = new Adverb("moderately");

        // prepend "a"
        public static readonly Adverb Little = new Adverb("little", true);

        public static Adverb Choose(float intensity)
        {
            if (intensity > .9) return Extremely;
            if (intensity > .75) return ChooseRandom(Very, Really);
            if (intensity > .5) return ChooseRandom(Quite, Little);
            return Moderately;
        }
    }
    
    public static class Quantifier
    {
        public static Adverb Much = new Adverb("much");
        public static Adverb Any = new Adverb("any");
        public static Adverb Enough = new Adverb("enough");
        public static Adverb Many = new Adverb("many");
        public static Adverb Fewer = new Adverb("fewer");
        public static Adverb More = new Adverb("more");
        public static Adverb Some = new Adverb("some");
        public static Adverb Less = new Adverb("less");
        public static Adverb No = new Adverb("no");
        
        // "a" - x 
        public static Adverb Few => new Adverb("few", true);
        
        // x - "of" 
        public static Adverb Lots => new Adverb("few", false, true);
        
        // "a" - x - "of" 
        public static Adverb Lot = new Adverb("lot", true, true);
    }

    public class Adverb : Structure
    {
        public static Adverb Not = new Adverb("not");

        public Adverb(string str, bool a = false, bool of = false)
        {
            if (a) Add("a");
            Add(str);
            if (of) Add("of");
        }
    }
    
    public class Verb : Word
    {
        public Verb(string verb) : base(verb) {}
    }
    
    public class Auxiliary : Verb
    {
        public static string[] Is = {"was", "is"};
        public static string[] Have = {"had", "have"};
        public static string[] Do = {"did", "do"};

        public Auxiliary(string[] aux, Tense tense) : base(Calculate(aux, tense)) {}
            
        private static string Calculate(string[] auxiliary, Tense tense) => auxiliary[tense == Tense.Past ? 0 : 1];
    }
    
    public class Modal : Verb
    {
        public static string[] Can = {"can", "could"};
        public static string[] May = {"may", "might"};
        public static string[] Shall = {"shall", "should"};
        public static string[] Will = {"will", "would"};
        public static string[] Must = {"must", "must"};
        
        public Modal(string[] modal, bool isPreterite = false) : base(Calculate(modal, isPreterite)) {}
        
        private static string Calculate(string[] modal, bool isPreterite) => modal[isPreterite ? 1 : 0];
    }

    public class Morpheme
    {
        public string characters;

        public Morpheme(string characters) => Init(characters);
        public Morpheme(Enum e) => Init(e.ToString());

        private void Init(string characters)
        {
            this.characters = characters;
        }

        public static Lexeme operator +(Lexeme l, Morpheme m) { l.Append(m); return l; }
        public static Lexeme operator +(Morpheme m, Lexeme l) { l.Prepend(m); return l; }
    }

    public class Word
    {
        public enum Type { Noun, Verb, Adjective, Conjugation, Quantifier }
        
        public string str;
        public Lexeme Lexeme;
        public List<Syllable> syllables;
        public Type type;
        public bool IsPlural;
        
        public char lastChar => str[str.Length - 1];
        public bool StartsWithVowel => str[0].IsVowel();

        public Word(string characters)
        {
            var root = new Morpheme(characters);
            Lexeme = new Lexeme(root);
            this.str = Lexeme.ToString();
            Syllabicate();
        }

        public void SetCapitalised(bool value)
        {
            if (value) str = char.ToUpper(str[0]) + str.Substring(1).ToLower();
            else str = char.ToLower(str[0]) + str.Substring(1).ToLower();
        }

        public void Syllabicate()
        {
            syllables = new List<Syllable>();

            if (DataManager.dictionary.TryGetValue(str, out string phonemes))
            {
                phonemes = phonemes.Replace("'", "").Replace(@"\.", "");
                char previousPhoneme = '?'; // Rogue value
                List<int> breakPoints = new List<int>();

                // Calculate Break Points
                for (int i = 0; i < phonemes.Length; i++)
                {
                    if (IsSyllableStart(phonemes[i], previousPhoneme)) breakPoints.Add(i);
                    previousPhoneme = phonemes[i];
                }

                int prevIndex = 0;

                for (int i = 0; i < breakPoints.Count; i++)
                {
                    int index = breakPoints[i];

                    // Prefix
                    string prefix = string.Empty;
                    if (i == 0) prefix = phonemes.Substring(prevIndex, index);

                    // Suffix
                    string suffix = string.Empty;
                    if (i < breakPoints.Count - 1)
                    {
                        int nextIndex = breakPoints[i + 1];
                        suffix = phonemes.Substring(index + 1, nextIndex - index - 1);
                    }
                    else suffix = phonemes.Substring(index + 1);
                    
                    // Create syllable
                    Syllable syllable = new Syllable(phonemes[index], prefix, suffix);
                    syllables.Add(syllable);

                    prevIndex = index;
                }
            }
        }
        
        public void Derive(Type target)
        {
            if (type == target) return;
            if (type == Type.Verb)
            {
                if (type == Type.Noun)
                {
                    // var morpheme = new Morpheme((lastChar == 'e' ? Suffix.r : Suffix.er));
                    // Append(morpheme);
                }
            }
        }
    }
    
    public class Lexeme
    {
        public List<Morpheme> morphemes = new List<Morpheme>();
        public Morpheme root;

        public Lexeme(Morpheme root, Morpheme prefix = null, Morpheme suffix = null)
        {
            this.root = root;
            if (prefix != null) morphemes.Add(prefix);
            morphemes.Add(root);
            if (suffix != null) morphemes.Add(suffix);
        }

        // Operators
        public void Append(Morpheme morpheme) => morphemes.Append(morpheme);
        
        public void Prepend(Morpheme morpheme) => Prepend(morpheme);

        public override string ToString()
        {
            var str = "";
            foreach (Morpheme m in morphemes) str += m.characters;
            return str;
        }
    }
}
