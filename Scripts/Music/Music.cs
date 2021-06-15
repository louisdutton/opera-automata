using System.Collections.Generic;
using Language;
using UnityEngine;

namespace Music
{
    public enum Interval { Unison, m2, M2, m3, M3, P4, Tritone, P5, m6, M6, m7, M7, Octave }
    public enum Degree { Tonic, Supertonic, Mediant, Subdominant, Dominant, Submediant, Subtonic }
    public enum Mode { Ionian, Dorian, Phrygian, Lydian, Mixolydian, Aeolian, Locrian }

    [System.Serializable]
    public class Scale
    {
        private static int[] ToneStructure = { 2, 2, 1, 2, 2, 2, 1 };

        public int[] notes = new int[7];
        public int this[int index] => notes[index];
        public int IndexOf(int value) => System.Array.IndexOf(notes, value);

        public Scale(Mode mode)
        {
            // rotate tone structure base on mode
            var structure = Utils.RotateArray(ToneStructure, (int) mode);
            
            var n = 0;
            for (int i = 0; i < notes.Length; i++)
            {
                notes[i] = n;
                n += structure[i];
            }
            
            Debug.Log($"Created {mode} scale with structure {structure}");
        }
    }
    
    [System.Serializable]
    public class Key
    {
        private Scale scale;
        
        public int root;
        public Mode mode;
        public int this[int index] => root + scale[index];

        public Key(int root, Mode mode)
        {
            this.root = root;
            SetMode(mode);
        }

        public Chord RandomChord()
        {
            var root = 2; // chord III
            while (root == 2) root = Random.Range(0, 6);
            return new Chord(root, this);
        }

        /// <summary>Shifts the key's root note by the specified degree.</summary>
        /// <param name="degree">The degree in semitones.</param>
        public void Modulate(int degree) => root = (root + (degree * 5)) % 12;

        /// <summary>Sets the key's root note to the specified value.</summary>
        /// <param name="root">Root in range 0-11.</param>
        public void ModulateTo(int root)
        {
            if (root >= 0 && root <= 11) this.root = root;
            else Debug.Log("Error: Root outside range 0-11");
        }

        /// <summary>Changes the key to the specified mode.</summary>
        /// <param name="mode">The new mode.</param>
        public void SetMode(Mode mode)
        {
            this.mode = mode;
            scale = new Scale(mode);
        }

        /// <summary>Changes the key to the specified mode.</summary>
        /// <param name="note">The note used calculate the scale degree</param>
        public Degree GetScaleDegree(int note)
        {
            int value = ((note - root) % 12);
            int index = scale.IndexOf(value) % 7;
            return (Degree)index;
        }

        public int GetPitch(int index, int octave = 6)
        {
            octave += index / 7;
            if (index < 0) index = 7 - Mathf.Abs(index);
            index = index % 7;
            int origin = 12 + root + (octave - 1) * 12;
            return origin + scale[index];
        }
    }

    public class Note
    {
        public enum Letter { C, D, E, F, G, A, B }
        
        public Syllable syllable;
        public int pitch;
        public int velocity;
        public float frequency => Utils.NoteToFrequency(pitch);
        public override string ToString() => Utils.PitchToNoteName(pitch);

        public Note(int pitch, int velocity = 48, Syllable syllable = null)
        {
            this.velocity = velocity + Random.Range(-8, 8);
            this.pitch = pitch;
            this.syllable = syllable;
        }
    }

    public class Cadence
    {
        public enum Type { Perfect, Imperfect, Plagal, Deceptive }
        public Type type;
        public Chord[] chords;
        public Key key;
        public Chord this[int index] => chords[index];

        public Cadence(Type type, Key key)
        {
            this.type = type;

            switch (type)
            {
                case Type.Perfect:
                    chords = new Chord[]
                    {
                        new Chord(1, key),
                        new Chord(4, key),
                        new Chord(0, key)
                    }; break;
                case Type.Imperfect: 
                    chords = new Chord[]
                    {
                        new Chord(3, key),
                        new Chord(0, key),
                        new Chord(4, key)
                    }; break;
                case Type.Plagal:
                    chords = new Chord[]
                    {
                        new Chord(1, key),
                        new Chord(3, key),
                        new Chord(0, key)
                    }; break;
                case Type.Deceptive:
                    chords = new Chord[]
                    {
                        new Chord(1, key),
                        new Chord(4, key),
                        new Chord(5, key)
                    }; break;
            }
        }
    }

    public class Chord
    {
        public enum Type { Diminished, Minor, Major, Augmented }
        public enum Symbol { I, II, III, IV, V, VI, VII }

        public int inversion;
        public int root;
        public Key key;
        public int this[int index] => key.GetPitch(root + (index * 2));
        public override string ToString() => Utils.PitchToNoteName(key.GetPitch(root)) + " Chord";

        public Chord(int root, Key key)
        {
            this.root = root;
            this.key = key;
        }

        public int RandomPitch() => this[Random.Range(0, 2)];
    }
    
    public class Bar
    {
        public Beat[] beats;
        public int Length => beats.Length;
        public Beat this[int i]
        {
            get => beats[i];
            set => beats[i] = value;
        } 

        public Bar(int metre)
        {
            beats = new Beat[metre];
            for (int i = 0; i < metre; i++) beats[i] = new Beat();
        }
    }

    public class Beat
    {
        public List<Note> notes;
        public Chord chord;
        public Beat() => notes = new List<Note>();
    }

    public class Phrase
    {
        public Bar[] bars;
        public int Length => bars.Length;
        public Bar this[int i]
        {
            get => bars[i];
            set => bars[i] = value;
        } 

        public Phrase(Key key, int metre, int length = 4)
        {
            var cadence = new Cadence((Cadence.Type)Random.Range(0, 3), key); // create cadence calculation !!!
            
            bars = new Bar[length];
            
            // first bar
            bars[0] = new Bar(metre) { [0] = { chord = key.RandomChord() } };

            // cadence bars
            for (int i = 1; i < length; i++) // turn this into  Array<T>.Populate();
            {
                bars[i] = new Bar(metre) { [0] = { chord = cadence.chords[i - 1] } };
            }
        }
    }
}
