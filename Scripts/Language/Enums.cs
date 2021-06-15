namespace Language
{
    public enum Tense
    {
        Past,
        Present,
        Future
    }

    public enum Case { Nominative, Objective, Genitive }
    public enum To { Be, Have, Do }

    public class Affix : Morpheme
    {
        public Affix(string characters) : base(characters) {}
    }
    
    public class Prefix : Affix
    {
        public Prefix(string characters) : base(characters) {}
    }
    
    public class Suffix : Affix
    {
        public Suffix(string characters) : base(characters) {}
    }
    
    // de,
    // dis,
    // trans,
    // dia,
    // ex,
    // e,
    // mono,
    // uni,
    // bi,
    // di,
    // tri,
    // multi,
    // poly,
    // pre,
    // post,
    // mal,
    // mis,
    // bene,
    // pro,
    // sub,
    // re,
    // inter,
    // intra,
    // co,
    // com,
    // con,
    // col,
    // be,
    // non,
    // un,
    // in,
    // im,
    // il,
    // ir,
    // a,
    // an,
    // contra,
    // counter,
    // en,
    // em
    
    // suffices
    
    // er,
    // r,
    // ly,
    // able,
    // ible,
    // hood,
    // ful,
    // less,
    // ish,
    // ness,
    // ic,
    // ist,
    // ian,
    // ing,
    // or,
    // eer,
    // ology,
    // ship,
    // some,
    // ous,
    // ive,
    // age,
    // ant,
    // ent,
    // ment,
    // ary,
    // ise,
    // ure,
    // ion,
    // ation,
    // ance,
    // ence,
    // ity,
    // al,
    // ial,
    // ate,
    // tude,
    // ism,
    // s,
    // es
}