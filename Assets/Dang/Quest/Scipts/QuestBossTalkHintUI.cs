using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Unity.FantasyKingdom
{
    public class QuestBossTalkHintUI : MonoBehaviour
    {
        public TextMeshProUGUI hintText;
        public static QuestBossTalkHintUI Instance;
        

        void Awake()
        {
            Instance = this;
            hintText.text = "";
        }

        public void ShowHint(string text) => hintText.text = text;
        public void HideHint() => hintText.text = "";

    }
}
