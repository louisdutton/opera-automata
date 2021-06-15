using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Music
{
    public static class Utils
    {
        /// <summary>
        /// Generates a note with random pitch within given range
        /// </summary>
        public static Note RandomNote(int min, int max)
        {
            return new Note(Random.Range(min, max + 1));
        }
        
        /// <summary>
        /// Generates a note with random pitch within given range
        /// </summary>
        public static Note RandomNote(Vector2Int range)
        {
            return new Note(Random.Range(range.x, range.y + 1));
        }

        /// <summary>
        /// Convert pitch value to frequency in Hertz
        /// </summary>
        public static float NoteToFrequency(int note)
        {
            // Calculate distance to A4 (440Hz)
            int distance = note - 69;
            if (distance == 0)
            {
                return 440f;
            }
            else
            {
                return 440f * Mathf.Pow(2, distance/12f);
            }
        }

        /// <summary>
        /// Converts MIDI pitch value to the corresponding note name
        /// </summary>
        public static string PitchToNoteName(int pitch)
        {
            return Constants.NOTES_SHARP[pitch % 12] + (pitch / 12).ToString();
        }

        /// <summary>
        /// Alters a value by a random value up to the given amount of variance
        /// </summary>
        public static float Humanize(this float value, float variance = .05f)
        {
            return value + Random.Range(-variance, variance);
        }

        /// <summary>
        /// Returns the interval beteween two notes values
        /// </summary>
        public static Interval CalculateInterval(int n1, int n2)
        {
            int difference = Mathf.Abs(n2 - n1); // %12
            return (Interval)difference;
        }

        /// <summary>
        /// Returns the index of the closest note in a chord to a given note
        /// </summary>
        public static int IndexOfClosestNote(Chord chord, int prevNote)
        {
            int[] options = new int[]
            {   (int)CalculateInterval(chord[0], prevNote),
            (int)CalculateInterval(chord[1], prevNote),
            (int)CalculateInterval(chord[2], prevNote)
            };

            var result =
            (
                from num in options
                let diff = Mathf.Abs(prevNote - num)
                orderby diff
                select num
            ).Last();

            return System.Array.IndexOf(options, result);
        }

        public static void StopNotes(Note[] notes)
        {
            foreach (Note note in notes)
            {
                //note.eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        public static void StopNotes(List<Note> notes)
        {
            foreach (Note note in notes)
            {
                //note.eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        public static void StopNotes(Beat beat) => StopNotes(beat.notes);

        public static void StopNotes(Bar bar)
        {
            for (int i = 0; i < bar.Length; i++)
            {
                StopNotes(bar[i]);
            }
        }
        
        /// <summary>Rotates an array (Left) by the given degree.</summary>
        /// <param name="degree">The degree of rotation.</param>
        public static T[] RotateArray<T>(T[] arr, int degree)
        {
            var q = new Queue<T>(arr);
            for (int i = 0; i < degree; i++) q.Enqueue(q.Dequeue());
            return q.ToArray();
        }
    }
}