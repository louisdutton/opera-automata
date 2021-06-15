using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Synthesis.Vocal
{
    public class Tract : MonoBehaviour
    {
        // Transients
        private int lastObstruction = -1; // index of tract obstruction   
        private List<Transient> transients; 
        
        private float fade = 1f; // 0.9999
        private float movementSpeed = 15; // cm per second

        // Cavities
        [Range(0, 1)] public float glottalReflection = .75f;             
        [Range(0, -1)] public float lipReflection = -.85f;       
        [Range(16, 128)] public int sections = 44; 
        
        [Space]
        public NasalCavity nasal;
        public OralCavity oral;
        
        // GLottis
        public Glottis glottis;

        // private OpenSimplexNoise simplexNoise;
        private float sampleRate;
        private float timeStep;

        private System.Random rand;

        private void Update()
        {
            oral.glottalReflection = glottalReflection;
            oral.lipReflection = lipReflection;
        }

        private void Awake()
        {
            // Required Values
            rand = new System.Random();
            sampleRate = AudioSettings.outputSampleRate;
            timeStep = 1f / sampleRate;
            // simplexNoise = new OpenSimplexNoise();
            
            // Transients
            transients = new List<Transient>();
            
            // Cavities
            oral = new OralCavity(sections);
            nasal = new NasalCavity(oral);

            nasal.CalculateReflections();
            oral.CalculateReflections(nasal);
            nasal.diameter[0] = nasal.velumTarget;
        }     
        
        private void ReshapeTract(float deltaTime)
        {
            var amount = deltaTime * movementSpeed; ;    
            var newLastObstruction = -1;
            
            // for (var i = 0; i < oral.N; i++)
            // {
            //     var d = oral.diameter[i];
            //     var td = oral.targetDiameter[i];
            //     if (d <= 0) newLastObstruction = i;
            //     
            //     var slowReturn = CalculateSlowReturn(i); 
            //    
            //     oral.diameter[i] = MoveTowards(d, td, slowReturn * amount, 2f * amount);
            // }
            
            // Handle new obstructions
            if (lastObstruction > -1 && newLastObstruction == -1 && nasal.A[0] < .5f)
            {
                transients.Add(new Transient(lastObstruction));
            }
            lastObstruction = newLastObstruction;
            
            amount = deltaTime * movementSpeed; 
            nasal.diameter[0] = MoveTowards(nasal.diameter[0], nasal.velumTarget, amount * .25f, amount * .1f);
            nasal.A[0] = nasal.diameter[0] * nasal.diameter[0];        
        }
        
        private float CalculateSlowReturn(int i)
        {
            if (i < nasal.start) return .6f;
            if (i >= oral.tipStart) return 1f; 
            return .6f + .4f * (i-nasal.start) / (oral.tipStart-nasal.start);
        }

        // Replace with cleaner parametric interpolation method
        private float MoveTowards(float current, float target, float up, float down)
        {
            if (current < target) return Mathf.Min(current + up, target);
            return Mathf.Max(current - down, target);
        }

        public float GetOutput(float glottalOutput, float turbulenceNoise, float lambda)
        {
            // var updateAmplitudes = (float)rand.NextDouble() < .1f;

            // ProcessTransients();
            // AddTurbulenceNoise(turbulenceNoise);

            // At glottis
            oral.ProcessGlottalOutput(lambda, glottalOutput);
            
            // A Oral-Nasal Junction
            var n = nasal.start;
            oral.junctionOutputL[n] = oral.kL * oral.R[n-1] + (1+oral.kL) * (nasal.L[0] + oral.L[n]);
            oral.junctionOutputR[n] = oral.kR * oral.L[n] + (1+oral.kR) * (oral.R[n-1] + nasal.L[0]);
            nasal.junctionOutputR[0] = oral.kN * nasal.L[0] + (1+oral.kN) * (oral.L[n] + oral.R[n-1]);
            
            // Output at nose / lip end
            oral.CalculateOutput(false);
            nasal.CalculateOutput(false, oral.lipReflection, fade);

            return oral.output + nasal.output;
        }

        public void AddTransient()
        {
            transients.Add(new Transient(0)); // test position !!!
        }

        public void PostBuffer(float deltaTime)
        {         
            // ReshapeTract(deltaTime);
            // oral.CalculateReflections(nasal);
        }
        
        private void ProcessTransients()
        {
            for (var i = 0; i < transients.Count; i++)  
            {
                var t = transients[i];
                var amp = t.Amplitude;
                oral.R[t.position] += amp / 2f;
                oral.L[t.position] += amp / 2f;
                t.timeAlive += timeStep;
            }
            for (var i=transients.Count - 1; i >= 0; i--)
            {
                var t = transients[i];
                if (t.Dead) transients.Remove(t);
            }
        }
        
        // NO IDEA HOW THIS WORKS
        private void AddTurbulenceNoise(float turbulenceNoise)
        {
            var intensity = .1f;
            AddTurbulenceNoiseAtIndex(.66f * turbulenceNoise * intensity, 0, .5f); // text values
        }
        
        private void AddTurbulenceNoiseAtIndex(float turbulenceNoise, int index, float diameter)
        {   
            var i = Mathf.FloorToInt(index);
            var delta = index - i;
            turbulenceNoise = .1f;
            turbulenceNoise *= glottis.NoiseModulation;
            var thinness0 = Mathf.Clamp(8f * (.7f - diameter), 0f, 1f);
            var openness = Mathf.Clamp(30f * (diameter - .3f), 0f, 1f);
            var noise0 = turbulenceNoise * (1f - delta) * thinness0 * openness;
            var noise1 = turbulenceNoise * delta * thinness0 * openness;
            oral.R[i+1] += noise0 / 2f;
            oral.L[i+1] += noise0 / 2f;
            oral.R[i+2] += noise1 / 2f;
            oral.L[i+2] += noise1 / 2f;
        }

    }
}

