namespace Language
{
    public class Syllable
    {
        public char vowel;
        public string prefix;
        public string suffix;

        public Syllable(char vowel, string prefix, string suffix)
        {
            this.vowel = vowel; // need to add in dipthongue support

            foreach (char c in suffix)
            {
                if (Utils.IsVowelPhoneme(c))
                {
                    suffix = suffix.Replace(c, '\0');
                }
            }

            this.prefix = prefix;
            this.suffix = suffix;
        }

        public override string ToString()
        {
            return string.Empty + prefix + vowel + suffix;
        }
    }

    // public class Phoneme
    // {
    //     public string dir;
    // }
}