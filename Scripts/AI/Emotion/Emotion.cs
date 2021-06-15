using UnityEngine;

namespace AI.Emotion
{
    public enum E { Anticipation, Joy, Trust, Fear, Surprise, Sadness, Disgust, Anger }
    public enum D
    {
        Optimism, Hope, Anxiety, Love, Guilt, Delight, Submission, Curiosity,
        Sentimentality, Awe, Despair, Shame, Disapproval, Shock, Outrage, Remorse,
        Ency, Pessimism, Contempt, Cynicism, Aggressiveness, Morbidness, Pride, Dominance
    }
    
    [System.Serializable]
    public class Emotion : Attribute
    {
        public Emotion(E name) : base(name.ToString()) {}
        public Emotion(D name) : base(name.ToString()) {}
    }

    [System.Serializable]
    public class Dyad : Emotion
    {
        [Header("Components")]
        public Emotion a;
        public Emotion b;

        public new float value => a.value + b.value;
        
        public Dyad(D name, Emotion a, Emotion b) : base(name)
        {
            this.a = a;
            this.b = b;
        }
    }
}
