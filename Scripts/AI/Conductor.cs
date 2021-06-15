using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Language;
using Music;
using Synthesis;

namespace AI
{
    public class Conductor : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The music's tonal centre")]
        public Key key;
        [Tooltip("Beats per minute (BPM)")]
        [Range(60, 240)] public int tempo; // bpm
        [Tooltip("Beats per bar")]
        [Range(2, 12)] public int metre;

        [Header("Components")]
        public Singer singer;
        public Piano piano;
        public AudioSource metronome;
        public bool useMetronome;

        private void Awake()
        {
            key.SetMode(key.mode);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M)) StartCoroutine(Perform());
        }
        
        public IEnumerator Perform()
        {
            // Notes & Chords
            Note note = null;
            int prevPitch = -1;
            Chord chord = new Chord(0, key); // initial chord
            Chord prevChord = null;
            
            // Phrase
            var phraseTotal = 4;
            var phraseLength = 4;
            var phrase = new Phrase(key, metre, phraseLength);
            phrase[0][0].chord = chord; // adds initial chord to start of phrase
            var phraseIndex = 0;
            var phraseCount = 0;
            
            // Bars
            Queue<Bar> bars = new Queue<Bar>();
            bars.Enqueue(phrase[phraseIndex]);
            var barCount = 0;

            singer.voice.NoteOn();
            while (bars.Count > 0)
            {
                var bar = bars.Dequeue();
                
                // add cadence logic !!!
                // Debug.Log($"Bar {barCount}: {chord}");
                
                // Calculation
                for (int i = 0; i < bar.beats.Length; i++)
                {
                    var beat = bar.beats[i]; 
                    chord = beat.chord ?? chord;
                    
                    if (i == 0) piano.Play(chord);
                    
                    var pitch = chord.RandomPitch();
                    note = new Note(pitch);
                    var interval = Music.Utils.CalculateInterval(prevPitch, note.pitch);
                    
                    beat.notes.Add(note);
                    
                    // Passing notes
                    if ((interval == Interval.M3 || interval == Interval.m3) && i > 0 && Random.value < .5f)
                    {
                        var deg = (int)key.GetScaleDegree(note.pitch);
                        var dir = (int)Mathf.Sign(pitch - prevPitch);
                        var passingPitch = key.GetPitch(deg - (1*dir));
                        var passingNote = new Note(passingPitch);
                        bar.beats[i-1].notes.Add(passingNote);
                        Debug.Log($"direction: {dir}, start {deg}");
                    }
                    
                    prevPitch = note.pitch;
                }

                // Performance
                for (int i = 0; i < bar.beats.Length; i++)
                {
                    // play metronome beat
                    if (useMetronome) {
                        metronome.pitch = (i == 0) ? 1.5f : 1f;
                        metronome.Play();
                    }

                    // separate singer routine so the synchronisation can be humanized !!!
                    var notes = bar.beats[i].notes;
                    var timeTaken = 0f;
                    var increment = 60f / tempo / (float)notes.Count;
                    for (int n = 0; n < notes.Count; n++)
                    {
                        singer.voice.SetNote(notes[n]);
                        Debug.Log($"Beat: {i}, Note: {notes[n]}");
                        timeTaken += increment;
                        yield return new WaitForSeconds(increment);
                    }
                   

                    yield return new WaitForSeconds((60f / tempo) - timeTaken);
                }
                
                // At the end of each bar
                phraseIndex++;
                barCount++;
                
                // Reached end of phrase
                if (phraseIndex > phrase.Length - 1)
                {
                    Debug.Log($"End of Phrase");
                    phrase = new Phrase(key, metre, phraseLength);
                    phraseCount++;
                    phraseIndex = 0;
                }
                
                if (phraseCount < phraseTotal) bars.Enqueue(phrase[phraseIndex]);

                yield return null;
            }
            singer.voice.NoteOff();
        }
    }
    
    
}
