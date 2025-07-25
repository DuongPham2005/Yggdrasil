
using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class QuestBossManager : MonoBehaviour
    {
        public GameObject questPanel;
        public TextMeshProUGUI questText;

        private bool questActive = false;
        private bool bossDefeated = false;

        void Start()
        {
            questPanel.SetActive(false);
        }

        public void StartQuest()
        {
            questActive = true;
            bossDefeated = false;

            questPanel.SetActive(true);
            questText.text = "Tiêu diệt Veigar 0/1";
        }

        public void BossDefeated()
        {
            if (questActive && !bossDefeated)
            {
                bossDefeated = true;
                questText.text = "Tiêu diệt Veigar 1/1 (Hoàn thành)";
            }
        }
    }
}
