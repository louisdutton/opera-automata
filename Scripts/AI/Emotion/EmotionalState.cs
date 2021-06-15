using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UI;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace AI.Emotion
{
    public class EmotionalState : MonoBehaviour
    {
        public PolyGraph graph;
            
        // Emotions
        public Emotion[] emotions;

        public Emotion Anticipation => emotions[0];
        public Emotion Joy => emotions[1];
        public Emotion Trust => emotions[2];
        public Emotion Fear => emotions[3];
        public Emotion Surprise => emotions[4];
        public Emotion Sadness => emotions[5];
        public Emotion Disgust => emotions[6];
        public Emotion Anger => emotions[7];

        // Dyads
        public Dyad[] dyads;

        public Dyad Optimism => dyads[0];
        public Dyad Hope => dyads[1];
        public Dyad Anxiety => dyads[2];
        public Dyad Love => dyads[3];
        public Dyad Guilt => dyads[4];
        public Dyad Delight => dyads[5];
        public Dyad Submission => dyads[6];
        public Dyad Curiosity => dyads[7];
        public Dyad Sentimentality => dyads[8];
        public Dyad Awe => dyads[9];
        public Dyad Despair => dyads[10];
        public Dyad Shame => dyads[11];
        public Dyad Disapproval => dyads[12];
        public Dyad Shock => dyads[13];
        public Dyad Outrage => dyads[14];
        public Dyad Remorse => dyads[15];
        public Dyad Envy => dyads[16];
        public Dyad Pessimism => dyads[17];
        public Dyad Contempt => dyads[18];
        public Dyad Cynicism => dyads[19];
        public Dyad Morbidness => dyads[20];
        public Dyad Aggressiveness => dyads[21];
        public Dyad Pride => dyads[22];
        public Dyad Dominance => dyads[23];
        
        // Utils
        public Emotion this[int i] => emotions[i];
        
        public Emotion DominantEmotion => emotions.OrderByDescending(e => e.value).FirstOrDefault();

        public float AverageValue => (float) emotions.Average(e => e.value);

        // Init
        private void Awake()
        {
            emotions = new Emotion[8];
            for (int i = 0; i < 8; i++)
            {
                emotions[i] = new Emotion((E)i);
            }

            dyads = new Dyad[24];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var index = i * 3 + j;
                    var a = emotions[i];
                    var b = emotions[(i + j + 1) % 8];
                    
                    dyads[index] = new Dyad((D)index, a, b);
                } 
            }
        }

        private void Update()
        {
            graph.SetAttributes(emotions);
        }

        public void Shift()
        {
            for (int i = 0; i < 8; i++)
            {
                emotions[i].value = UnityEngine.Random.value;
            }
        }
        
        // public float optimism => anticipation + joy;
        // public float hope => anticipation + trust;
        // public float anxiety => anticipation + fear;
        // public float love => joy + trust;
        // public float guilt => joy + fear;
        // public float delight => joy + surprise;
        // public float submission => trust + fear;
        // public float curiosity => trust + surprise;
        // public float sentimentality => trust + sadness;
        // public float awe => fear + surprise;
        // public float despair => fear + sadness;
        // public float shame => fear + sadness;
        // public float disapproval => surprise + sadness;
        // public float shock => surprise + disgust;
        // public float outrage => surprise + anger;
        // public float remorse => sadness + disgust;
        // public float envy => sadness + anger;
        // public float pessimism => sadness + anticipation;
        // public float contempt => disgust + anger;
        // public float cynicism => disgust + anticipation;
        // public float morbidness => disgust + joy;
        // public float aggressiveness => anger + anticipation;
        // public float pride => anger + joy;
        // public float dominance => anger + trust;
    }
}
