using Music;
using UnityEngine;
using UnityEngine.Audio;

namespace Synthesis.Vocal
{
    [RequireComponent(typeof(Glottis))]
    [RequireComponent(typeof(Tract))]
    public class Voice : Instrument
    {
        public enum Type { Bass, Baritone, Tenor, Mezzo, Soprano }

        private class Vowel
        {
            public float[] frequency;
            public float[] gain;

            public Vowel(float[] frequency, float[] gain)
            {
                this.frequency = frequency;
                this.gain = gain;
            }
        }
        
        [Header("Voice Settings")]
        public Type type;
        public Vector2Int range;
        public AudioMixer mixer;
        public float[] vowel;
        public float aspirationRatio = .5f;
        public float tensionRatio = .5f;
        [Range(0, 1)] public float turbulence = 0f;

        private Vector2 frequencyRange;
        private readonly float interpolant = 0.001f;
        private Glottis glottis;
        private Tract tract;

        protected override void Awake()
        {
            base.Awake();
            frequencyRange = new Vector2(Utils.NoteToFrequency(range.x), Utils.NoteToFrequency(range.y));
            glottis = GetComponent<Glottis>();
            tract = GetComponent<Tract>();
        }

        protected override float GenerateWaveform(float[] data, int channels)
        {
            // dynamic
            // var targetVelocity = targetNote?.velocity ?? 64f;
            // var velocity = Mathf.Lerp(0f, 1f, targetVelocity / 128f);
            // glottis.intensity = Mathf.Lerp(glottis.intensity, velocity, interpolant);
            // glottis.aspirationGain = glottis.intensity * aspirationRatio;
            // var tension = velocity * tensionRatio;
            // glottis.tension = Mathf.Lerp(glottis.tension, tension,interpolant);
            //     
            var position = timeInWaveform / waveLength; // position in wave
            var glottalExcitation = glottis.Excitation(position);
            
            var voice = tract.GetOutput(glottalExcitation, turbulence, lambda);

            return voice * amplitude;
        }
    }
}