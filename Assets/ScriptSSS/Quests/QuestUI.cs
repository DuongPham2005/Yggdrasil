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
            foreach (var pair in questManager.Active)
            {
                var quest = questManager.GetQuestById(pair.Key);
                var progress = pair.Value;
                if (quest == null) continue;

                sb.AppendLine($"- {quest.title} {(progress.completed ? "(Hoàn thành)" : "")}");
                foreach (var obj in quest.objectives)
                {
                    int current = progress.counters.TryGetValue(obj.id, out var v) ? v : 0;
                    sb.AppendLine($"  • {obj.type} {obj.id}: {current}/{obj.requiredAmount}");
                }
                sb.AppendLine();
            }
            text.text = sb.ToString();
        }
    }
}
