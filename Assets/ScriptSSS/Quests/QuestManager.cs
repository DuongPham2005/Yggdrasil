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
                    // Optionally: check completion
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
