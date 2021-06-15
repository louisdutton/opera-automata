using UnityEngine;

namespace Synthesis.Vocal
{
    public class Transient                                                                        
    {                                                                                              
        public const float LifeTime = .2f;                                                         
        public const float Strength = .3f;                                                         
        public const float Exponent = 200f;                                                        
                                                                                               
        public readonly int position;                                                              
                                                                                               
        public float timeAlive = 0f;                                                               
        public float Amplitude => Strength * Mathf.Pow(2, -Exponent * timeAlive);                  
        public bool Dead => timeAlive > LifeTime;                                                  
                                                                                               
        public Transient(int position)                                                             
        {                                                                                          
            this.position = position;                                                              
        }                                                                                          
    }                                                                                              
}