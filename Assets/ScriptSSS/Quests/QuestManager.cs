using System.Collections.Generic;
using UnityEngine;

namespace ScriptSSS.Quests
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private List<QuestSO> availableQuests = new List<QuestSO>();

        // Active progress by questId
        private readonly Dictionary<string, QuestProgress> active = new Dictionary<string, QuestProgress>();

        public IReadOnlyDictionary<string, QuestProgress> Active => active;

        private void OnEnable()
        {
            QuestEvents.EnemyKilled += OnEnemyKilled;
        }

        private void OnDisable()
        {
            QuestEvents.EnemyKilled -= OnEnemyKilled;
        }

        public bool AcceptQuest(QuestSO quest)
        {
            if (quest == null || string.IsNullOrEmpty(quest.questId)) return false;
            if (active.ContainsKey(quest.questId)) return false; // already accepted

            var progress = new QuestProgress { questId = quest.questId };
            foreach (var obj in quest.objectives)
            {
                if (!progress.counters.ContainsKey(obj.id)) progress.counters[obj.id] = 0;
            }
            active[quest.questId] = progress;
            return true;
        }

        public bool CanTurnIn(QuestSO quest)
        {
            if (quest == null || !active.TryGetValue(quest.questId, out var progress)) return false;
            return progress.completed && !progress.turnedIn;
        }

        public bool TurnIn(QuestSO quest)
        {
            if (!CanTurnIn(quest)) return false;
            var progress = active[quest.questId];
            progress.turnedIn = true;

            // Reward hooks
            if (quest.goldReward > 0)
            {
                // TODO: tích hợp ví/túi tiền của bạn ở đây
                Debug.Log($"Reward gold +{quest.goldReward}");
            }
            if (quest.expReward > 0)
            {
                // TODO: tích hợp EXP ở đây
                Debug.Log($"Reward exp +{quest.expReward}");
            }

            // Nếu muốn, có thể xóa khỏi active sau khi turn-in
            // active.Remove(quest.questId);
            return true;
        }

        private void OnEnemyKilled(string targetId)
        {
            if (string.IsNullOrEmpty(targetId)) return;

            foreach (var pair in active)
            {
                var quest = GetQuestById(pair.Key);
                if (quest == null) continue;
                var progress = pair.Value;

                bool changed = false;
                foreach (var obj in quest.objectives)
                {
                    if (obj.type != QuestObjectiveType.Kill) continue;
                    if (obj.id != targetId) continue;

                    int current = progress.counters.TryGetValue(obj.id, out var v) ? v : 0;
                    if (current < obj.requiredAmount)
                    {
                        progress.counters[obj.id] = current + 1;
                        changed = true;
                    }
                }
                if (changed)
                {
                    // check completion
                    progress.completed = CheckCompleted(quest, progress);
                }
            }
        }

        private bool CheckCompleted(QuestSO quest, QuestProgress progress)
        {
            foreach (var obj in quest.objectives)
            {
                int current = progress.counters.TryGetValue(obj.id, out var v) ? v : 0;
                if (current < obj.requiredAmount) return false;
            }
            return true;
        }

        public QuestSO GetQuestById(string questId)
        {
            for (int i = 0; i < availableQuests.Count; i++)
            {
                if (availableQuests[i] != null && availableQuests[i].questId == questId)
                    return availableQuests[i];
            }
            return null;
        }

        // Debug helper to add/remove available quests at runtime
        public void RegisterAvailableQuest(QuestSO quest)
        {
            if (quest != null && !availableQuests.Contains(quest)) availableQuests.Add(quest);
        }
    }
}
