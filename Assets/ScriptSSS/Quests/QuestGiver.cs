using UnityEngine;

namespace ScriptSSS.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        public QuestSO quest;
        public KeyCode interactKey = KeyCode.F;   // nhận nhiệm vụ
        public KeyCode turnInKey = KeyCode.G;     // trả nhiệm vụ
        public string playerTag = "Player";

        private QuestManager questManager;
        private bool playerInside;

        private void Start()
        {
            questManager = FindObjectOfType<QuestManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag)) playerInside = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag)) playerInside = false;
        }

        private void Update()
        {
            if (!playerInside || questManager == null || quest == null) return;

            if (Input.GetKeyDown(interactKey))
            {
                questManager.AcceptQuest(quest);
            }

            if (Input.GetKeyDown(turnInKey))
            {
                if (questManager.CanTurnIn(quest))
                {
                    questManager.TurnIn(quest);
                }
            }
        }
    }
}
