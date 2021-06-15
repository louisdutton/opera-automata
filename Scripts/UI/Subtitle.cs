using AI;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class Subtitle : MonoBehaviour
    {
        public static Subtitle Instance;
    
        public TMPro.TextMeshProUGUI content;
    
        public float wordDuration = 1f;
        public float characterDuration = .5f;
        public float revealDuration = 1f;
        public float fadeDuration = 1f;
        public float readingTime = 1f;
        public int heightDisplacement = 10;

        private Language.Sentence sentence;
        private Singer singer;
        private Color color;

        void Awake()
        {
            Instance = this;
            color = content.color;
            content.text = string.Empty;
            DOTween.defaultAutoPlay = AutoPlay.None;
        }

        public void SetText(Language.Sentence sentence)
        {
            var textDuration = sentence.words.Count * wordDuration;
            this.sentence = sentence;
            content.text = sentence.ToString();
            content.alpha = 0f;

            var animator = new DOTweenTMPAnimator(content);
            var sequence = DOTween.Sequence().OnComplete(FadeOut);
            var charCount = animator.textInfo.characterCount;
            var step = revealDuration / charCount;
            for (int i = 0; i < charCount; ++i)
            {
                if (!animator.textInfo.characterInfo[i].isVisible) continue;
                
                var time = i * step;
                var offset = animator.GetCharOffset(i);
                animator.SetCharOffset(i, offset + Vector3.up * heightDisplacement);
                sequence.Insert(time, animator.DOOffsetChar(i, offset, characterDuration));
                sequence.Insert(time, animator.DOFadeChar(i, 1f, characterDuration));
            }
        }

        private void FadeOut()
        {
            content.alpha = 1f;
            content.DOFade(0f, fadeDuration).SetDelay(readingTime);
        }

        public void HighlightWord(int index)
        {
            // string text = string.Empty;
            // for (int i = 0; i < sentence.words.Count; i++)
            // {
            //     string word = i == index ? $" <u>{sentence.words[i].str}</u>" : $" {sentence.words[i].str}";
            //     text += word;
            // }
            //
            // //Debug.Log(text);
            //
            // content.text = text;
        }

    }
}
