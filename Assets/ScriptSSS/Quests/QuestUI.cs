using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptSSS.Quests
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private QuestManager questManager;
        [SerializeField] private Text text;

        private void Start()
        {
            if (questManager == null) questManager = FindObjectOfType<QuestManager>();
        }

        private void Update()
        {
            if (questManager == null || text == null) return;

            var sb = new StringBuilder();
            sb.AppendLine("Nhiệm vụ đang theo dõi:");
            foreach (var pair in questManager.Active)
            {
                var quest = questManager.GetQuestById(pair.Key);
                var progress = pair.Value;
                if (quest == null) continue;

                string status = progress.turnedIn ? "(Đã trả)" : (progress.completed ? "(Hoàn thành - Nhấn G ở NPC để trả)" : "");
                sb.AppendLine($"- {quest.title} {status}");
                foreach (var obj in quest.objectives)
                {
                    int current = progress.counters.TryGetValue(obj.id, out var v) ? v : 0;
                    sb.AppendLine($"  • {obj.type} {obj.id}: {current}/{obj.requiredAmount}");
                }
                sb.AppendLine();
            }
            sb.AppendLine("Tương tác: F để nhận, G để trả tại NPC");
            text.text = sb.ToString();
        }
    }
}
