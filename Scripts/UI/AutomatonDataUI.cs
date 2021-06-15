using AI;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CharacterInfo : MonoBehaviour
    {
        public static CharacterInfo Instance;

        [SerializeField] private TextMeshProUGUI nameTMP;
        [SerializeField] private TextMeshProUGUI ageTMP;
        [SerializeField] private TextMeshProUGUI sexTMP;
        [SerializeField] private TextMeshProUGUI voiceTMP;
        [SerializeField] private TextMeshProUGUI sexualityTMP;
        [SerializeField] private TextMeshProUGUI statusTMP;
        [SerializeField] private TextMeshProUGUI personalityTMP;

        private void Awake() => Instance = this;

        private Character character;

        public void SetCharacter(Character c)
        {
            character = c;

            nameTMP.text = c.Get<string>("name");
            ageTMP.text = c.Get<string>("age");
            sexTMP.text = c.Get<string>("sex");
            // voiceTMP.text = singer.voice.type.ToString();
            sexualityTMP.text = c.Get<string>("sexuality");;
            statusTMP.text = c.Get<string>("status");;
            personalityTMP.text = c.personality.type.ToString();
        }
    }
}
