using Music;
using UnityEngine;

namespace Synthesis
{
    public class Piano : MonoBehaviour
    {
        public AudioClip[] notes;
        public AudioSource audioSource;

        public int[] voicing = { -24, 0, -12 };

        public void Play(int note)
        {
            audioSource.PlayOneShot(notes[note + 9]);
        }
        
        public void Play(Chord chord)
        {
            for (int i = 0; i < 3; i++) Play(chord[i] + voicing[i] - 48);
        }
    }
}