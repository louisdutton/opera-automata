using Unity.Collections;
using UnityEngine;

namespace Synthesis.Vocal
{
    public abstract class Cavity
    {
        public int N;
        public float[] R;            
        public float[] L;
        public float[] junctionOutputR;
        public float[] junctionOutputL;
        public float[] k; // kelly lochenbaum value
        public float[] diameter;     
        public float[] A;            
        public float[] maxAmplitude; 
            
        public float output;
    }
    
    [System.Serializable]
    public class NasalCavity : Cavity
    {
        public int start;  
        public float velumTarget = .1f; // target diameter of soft pallet (nasal cavity origin)        
        
        public NasalCavity(OralCavity oral)
        {
            N = Mathf.FloorToInt(oral.N * 7f / 11f); // 28:44    
            start = oral.N - N + 1;               
            R = new float[N];              
            L = new float[N];
            junctionOutputR = new float[N+1];
            junctionOutputL = new float[N+1];
            k = new float[N+1];
            A = new float[N];              
            maxAmplitude = new float[N];   
            
            // Diameter
            diameter = new float[N];            
            for (var m = 0; m < N; m++)         
            {                                        
                var d = 2f * (m / (float)N);    
                if (d < 1) d = .4f + 1.6f * d;       
                else d = .5f + 1.5f * (2 - d);       
                d = Mathf.Min(d, 1.9f);              
                diameter[m] = d;                
            }
        }
        
        public void CalculateOutput(bool updateAmplitudes, float lipReflection, float fade)                             
        {                                                                                   
            junctionOutputL[N] = R[N-1] * lipReflection;

            for (var m = 1; m < N; m++)                                                
            {                                                                               
                var w = k[m] * (R[m-1] + L[m]);                     
                junctionOutputR[m] = R[m-1] - w;                                  
                junctionOutputL[m] = L[m] + w;
            }
            
            for (var m = 0; m < N; m++)                                                
            {                                                                               
                R[m] = junctionOutputR[m] * fade;                                 
                L[m] = junctionOutputL[m+1] * fade;
            }
            

            output = R[N-1];                                                 
        }  
        
        public void CalculateReflections()                                            
        {                                                                                  
            for (var m = 0; m < N; m++)                                           
            {                                                                              
                A[m] = diameter[m] * diameter[m]; // (Simplified from Pi*r^2 to d^2)                     
            }
            
            for (var m = 1; m < N; m++)                                           
            {                                                  
                if (A[m] == 0) k[m] = .999f; // Prevents bad behaviour if 0    
                else k[m] = (A[m-1] - A[m]) / (A[m-1] + A[m]); // Kelly-Lochenbaum Equation     
            }                                                                              
        }                                                                                  
    }
    
    [System.Serializable]
    public class OralCavity : Cavity
    {
        // Sections
        public int bladeStart; // Index of Blade section
        public int tipStart; // Index of Tip section    
        public int lipStart; // Index of Lip section   
        
        // Reflections (R+, L-)                     
        public float glottalReflection = .75f;             
        public float lipReflection = -.85f;                
        
        // Kelly lochenbaum reflections
        public float kL; // Left             
        public float kR; // Right
        public float kN; // Nasal

        public OralCavity(int length)
        {
            N = length;
            bladeStart = Mathf.FloorToInt(N * (5f / 22f)); // 10:44   
            tipStart = Mathf.FloorToInt(N * (8f / 11f)); // 32:44     
            lipStart = Mathf.FloorToInt(N * (39f / 44f)); // 39:44    
            diameter = new float[N];

            R = new float[N];                  
            L = new float[N];                  
            k = new float[N+1];
            junctionOutputR = new float[N+1];  
            junctionOutputL = new float[N+1];  
            A = new float[N];                  
            maxAmplitude = new float[N];       
            
            for (int m = 0; m < N; m++)                                                    
            {                                                                              
                var d = 0f;                                                                
                if (m < 7f * N / 44f - .5f) d = .6f;                                       
                else if (m < 12f * N / 44f) d = 1.1f;                                      
                else d = 1.5f;
                diameter[m] = d;
            }

            kL = 0;
            kR = 0;
            kN = 0;  
        }

        public void ProcessGlottalOutput(float lambda, float glottalOutput)
        {
            // glottalReflection = -0.8 + 1.6 * Glottis.newTenseness;                                
            junctionOutputR[0] = L[0] * glottalReflection + glottalOutput;                 
            junctionOutputL[N] = R[N-1] * lipReflection;                                   
                                                                                          
            for (var m = 1 ; m < N; m++)                                                             
            {                                                                                        
                // var r = reflection[i] * (1-lambda) + newReflection[i] * lambda; 
                var r = k[m];
                var w = r * (R[m-1] + L[m]); // reflection * (sum of previous right & current left)                                           
                junctionOutputR[m] = R[m-1] - w;                                           
                junctionOutputL[m] = L[m] + w;                                             
            }                                                                                        
        }
        
        public void CalculateOutput(bool updateAmplitudes)                             
        {                                                                                   
            for (var m = 0; m < N; m++)                                                            
            {                                                                                      
                R[m] = junctionOutputR[m] * .999f;                                       
                L[m] = junctionOutputL[m+1] * .999f;                                     
                                                                                                   
                //R[i] = Mathf.clamp(junctionOutputR[i] * fade, -1, 1);                            
                //L[i] = Mathf.clamp(junctionOutputL[i+1] * fade, -1, 1);                          
                                                                                                   
                // if (updateAmplitudes)                                                              
                // {                                                                                  
                //     var amplitude = Mathf.Abs(R[m] + L[m]);                              
                //     if (amplitude > maxAmplitude[m]) maxAmplitude[m] = amplitude;        
                //     else maxAmplitude[m] *= .999f;                                            
                // }                                                                                  
            }                                                                                      
                                                                                                     
            output = R[N-1];                                                                                                                                                                                                                                                
        }
        
        public void CalculateReflections(NasalCavity nasal)                                                      
        {                                                                                        
            for (var m = 0; m < N; m++)                                                          
            {                                                                                    
                A[m] = diameter[m] * diameter[m]; // ignoring PI (simplified to A = d^2)                          
            }                                                                                    
            for (var m = 1; m < N; m++)                                                          
            {
                if (A[m] == 0) k[m] = .999f; // Prevents bad behaviour if 0    
                else k[m] = (A[m-1] - A[m]) / (A[m-1] + A[m]); // Kelly-Lochenbaum junction (k)                 
            }

            var n = nasal.start;
            var sum = A[n] + A[n+1] + nasal.A[0];
            kL = (2f * A[n] - sum) / sum;                                
            kR = (2f * A[n+1] - sum) / sum;                             
            kN = (2f * nasal.A[0] - sum) / sum;                                   
        }
    }
}
