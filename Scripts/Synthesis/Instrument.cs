using System;
using Music;
using UnityEngine;

namespace Synthesis
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class Instrument : MonoBehaviour
    {
        [Header("Waveform")]
        [Range(0, 0.5f)] public float gain = .5f;
        [Range(220f, 880)] public float targetFrequency = 440; // defaults to A3;
        [HideInInspector] public int targetVelocity = 64;
        protected Note targetNote = null;

        [Header("Modules")]
        public ADSR adsr;
        public Portamento portamento;
        public Modulation vibrato;
        private float freqMod;

        // Filter Buffers
        private float prevInput = 0f;
        private float prevOutput = 0f;

        // Components
        private AudioSource audioSource;
        
        private float sampleRate;
        private OpenSimplexNoise simplexNoise = new OpenSimplexNoise(1);
        
        // Time
        protected float timeStep;
        protected float timeInWaveform ;
        protected float totalTime;
        protected float lambda; // position in buffer
        protected float timeOnNote;
        
        protected float frequency;
        [HideInInspector] public float amplitude;
        protected float waveLength;

        [HideInInspector] public bool noteHeld;
        private float noteOffTime;
        
        // Random & Noise Modules
        protected System.Random Rand = new System.Random();

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            sampleRate = AudioSettings.outputSampleRate;
            timeStep = 1f / sampleRate;
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            for (int n = 0; n < data.Length; n += channels)
            {
                lambda = (float)n / data.Length;
                
                frequency = portamento.Calculate(frequency, targetFrequency);
                CalculateWaveStep();
                CalculateAmplitude();

                data[n] = GenerateWaveform(data, channels);
                data[n + 1] = data[n];
            }

            // Frequency Modulation (Vibrato)
            freqMod = Sin(vibrato.frequency, vibrato.amplitude);
            freqMod += (float)simplexNoise.Evaluate(totalTime * vibrato.noiseFreqMultiplier, 0) * vibrato.noise;
            frequency *= 1 + freqMod;

            PostBuffer();
        }
        
        protected virtual void PostBuffer() {}

        protected virtual float GenerateWaveform(float[] data, int channels) => 0f;

        public virtual void SetNote(Note note)
        {
            targetNote = note;
            targetFrequency = Utils.NoteToFrequency(note.pitch);
            timeOnNote = 0f;
        }
        
        public void NoteOn() // NEED TO MOVE NOTE ASSIGNMENT FROM SetNote to NoteOn/Off EVENTS
        {
            if (!audioSource.isPlaying) audioSource.Play();
            noteHeld = true;
            totalTime = 0;
        }

        public void NoteOff()
        {
            noteHeld = false;
            noteOffTime = totalTime;
        }

        private float Sin(float frequency, float amplitude)
        {
            return  Mathf.Sin(2 * Mathf.PI * totalTime * frequency) * amplitude;  
        }
        
        private void CalculateAmplitude()
        {
            // Attack
            if (totalTime < adsr.attack.duration)
            {
                var interpolant = Mathf.InverseLerp(0, adsr.attack.duration, totalTime);
                amplitude = gain * adsr.attack.curve.Evaluate(interpolant);
            }
            
            // Release
            else if (!noteHeld)
            {
                var interpolant = Mathf.InverseLerp(noteOffTime, noteOffTime + adsr.release.duration, totalTime);
                amplitude = gain * adsr.release.curve.Evaluate(interpolant);
            }

            // Sustain
            else if (totalTime < adsr.attack.duration + adsr.sustain.duration)
            {
                var interpolant = Mathf.InverseLerp(adsr.attack.duration, adsr.attack.duration + adsr.sustain.duration, totalTime);
                amplitude = gain * adsr.sustain.curve.Evaluate(interpolant);
            }
        }

        private void CalculateWaveStep()
        {
            waveLength = 1f / frequency;
            timeInWaveform += timeStep;
            totalTime += timeStep;
            timeOnNote += timeStep;

            timeInWaveform %= waveLength; // cyles time
        }
        
        [Serializable]
        public class ADSR
        {
            public Transition attack;
            public Transition decay;
            public Transition sustain;
            public Transition release;
            
            [System.Serializable]
            public class Transition
            {
                [Range(.01f, 1f)] public float duration = .1f;
                public AnimationCurve curve;
            }
        }
        
        [Serializable]
        public class Modulation
        {
            [Range(4f, 6f)] public float frequency = 5f;
            [Range(.01f, .03f)] public float amplitude = 0.01f;
            [Range(0.001f, .01f)] public float noise = 0.01f;
            [Range(0, 10)] public int noiseFreqMultiplier = 4;
        }
        
        [Serializable]
        public class Portamento
        {
            // public float duration = f;
            public Vector2 bounds = new Vector2(.1f, .5f);

            public float Calculate(float frequency, float targetFrequency)
            {
                var diff = Mathf.Abs(targetFrequency - frequency);
                var port = Mathf.Lerp(bounds.x, bounds.y, 1 - (1 / diff));
                return Mathf.Lerp(frequency, targetFrequency, port);
            }
        }

        
    }
}

