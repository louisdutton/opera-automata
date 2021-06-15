using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "New Generation Settings", menuName = "VOS/GenerationSettings")]
    public class GenerationSettings : ScriptableObject
    {
        [Header("Age")]
        public int[] ageRange = { 18, 50 };
        public AnimationCurve ageDistribution; // also called age composition
        
        [Header("Sex")]
        [Range(0f, 1f)] public float maleFemaleRatio = .49f;
        
        [Header("Sexuality")]
        [Range(0f, 1f)] public float heteroHomoRatio = .90f;
        [Range(0f, 1f)] public float homoBiRatio = .75f;
    }
}