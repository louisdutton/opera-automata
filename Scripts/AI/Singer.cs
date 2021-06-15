using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Language;
using Music;
using static Music.Utils;
using Synthesis.Vocal;
using UI;

namespace AI
    {
        using Emotion;
        
        public class Singer : MonoBehaviour
        {
            [Header("Character")]
            public string name;
            public float age;
            public string sex;
            public float weight;
          
            public Character character;
            private Character Self => character;
            
            [Header("Narrative Generation")]
            public GenerationSettings characterGenerationSettings;
            
            [Header("Components")]
            public Voice voice;
            public EmotionalState emotions;
            public FaceSettings face;

            private CoroutineQueue actions;

            private void Awake()
            {
                actions = new CoroutineQueue(StartCoroutine);
                StartCoroutine(BlinkRoutine());
            }

            // Event Subscriptions
            private void Start() => EventManager.instance.OnDataLoaded += Generate;
            private void OnDisable() => EventManager.instance.OnDataLoaded -= Generate;

            private void Generate()
            {
                character = new Character(name, age, sex, weight, transform.localScale);
            }

            private void Update()
            {
                if (voice.noteHeld) emotions.Shift();
                
                if (Input.GetKeyDown(KeyCode.I)) print(new Introduction(Self));
                if (Input.GetMouseButtonDown(0)) voice.NoteOn();
                if (Input.GetMouseButtonUp(0)) voice.NoteOff();
                if (Input.GetKeyDown(KeyCode.S))
                {
                    var a = new Character(characterGenerationSettings);
                    var tense = Tense.Present;
                    actions.Enqueue(SingSentence(new Comparison(a, Self, tense)));
                }
                
                // mouth
                var mouthShape = Mathf.Lerp(0, 100, voice.amplitude / voice.gain);
                face.SetBlendShapeWeight(face.oVowel, mouthShape);
            }

            // Singing Algorithm (need to move into Language / Music class)
            public IEnumerator SingSentence(Sentence sentence)
            {
                Subtitle.Instance.SetText(sentence);

                // Start audio
                voice.NoteOn();

                foreach (Word word in sentence.words)
                {
                    float syllableDuration = .5f / (word.syllables.Count * .75f);

                    // Update subtitles to highlight current word
                    // subtitle.HighlightWord(sentence.words.IndexOf(word));

                    //int previousPitch = -1;
                    for (int i = 0; i < word.syllables.Count; i++)
                    {
                        var syllable = word.syllables[i];
                        //print(syllable.vowel);

                        // Change note
                        Note note = RandomNote(voice.range);
                        voice.SetNote(note);

                        yield return new WaitForSeconds(syllableDuration.Humanize());
                    }

                    // Gap between words
                    yield return new WaitForSeconds(0.2f);
                }

                voice.NoteOff();
            }

            public IEnumerator BlinkRoutine()
            {
                var duration = face.blinkDuration / 2;
                
                while (true)
                {
                    // Opening Eyes
                    var t = 0f;
                    do
                    {
                        var value = (t / duration) * 100;
                        face.SetBlendShapeWeight(face.leftEye, value);
                        face.SetBlendShapeWeight(face.rightEye, value);
                        t += Time.deltaTime;
                        yield return null;
                    } 
                    while (t < duration);
                   
                    
                    t = 0f; // reset
                    
                    // Closing Eyes
                    do
                    {
                        var value = (1 - t / duration) * 100;
                        face.SetBlendShapeWeight(face.leftEye, value);
                        face.SetBlendShapeWeight(face.rightEye, value);
                        t += Time.deltaTime;
                        yield return null;
                    } 
                    while (t < duration);

                    // Finished Blinking
                    yield return new WaitForSeconds(Random.Range(3f, 4f));
                }
            }
        }

        [System.Serializable]
        public class FaceSettings
        {
            public SkinnedMeshRenderer face;
            public float blinkDuration = .2f;
            public int leftEye;
            public int rightEye;
            public int oVowel;
            public int aVowel;
            public int eVowel;

            public void SetBlendShapeWeight(int i, float value) => face.SetBlendShapeWeight(i, value);

        }
    }
