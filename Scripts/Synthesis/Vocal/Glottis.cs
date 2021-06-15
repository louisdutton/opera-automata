using System;
using UnityEngine;

namespace Synthesis.Vocal
{
    public class Glottis : MonoBehaviour
    {
        [Header("Glottal Modes")]
        [Range(0, 1)] public float intensity;
        [Range(0f, 1f)] public float tension = .5f;
        public AnimationCurve pressed;
        public AnimationCurve breathy;
        
        [Header("Aspiration")]
        [Range(0, 1)] public float aspirationGain;
        private float prevIn = 0f;
        private float prevOut = 0f;
        [Range(0, 1)] public float alpha = .99f;
        public bool solo = false;

        private System.Random rand;
        [HideInInspector] public float NoiseModulation;

        private void Awake()
        {
            rand = new System.Random();
        }

        public float Excitation(float position)
        {
            var sourceWave = !solo ? SourceWave(position) * intensity : 0;
            var aspiration = Aspiration(position) * aspirationGain;
            return sourceWave + aspiration;
        }
    
        private float SourceWave(float position)
        {
            var a = breathy.Evaluate(position);
            var b = pressed.Evaluate(position);
            return Mathf.Lerp(a, b, tension);
        }
        
        private float Aspiration(float position)
        {
            // white noise
            var input = (float) rand.NextDouble();
            
            // amplitude modulation
            input *= GetNoiseModulation(position);

            // low-pass filter
            var output = alpha * input + (1-alpha) * prevOut;
            prevIn = input;
            prevOut = output;
            
            return output;
        }

        // Hanning window synchronised with glottal waveform
        public float GetNoiseModulation(float position)
        {
            var floor = .1f;
            var scale = .2f;
            var modulation = floor + Mathf.Max(0f,Mathf.Sin(2f*Mathf.PI * position)) * scale;
            NoiseModulation = modulation;
            return modulation;
        }
    }
}
