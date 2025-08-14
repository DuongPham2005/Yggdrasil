using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptSSS.Quests
{
    public enum QuestObjectiveType
    {
        Kill,
        Collect,
        Talk
    }

    [Serializable]
    public class QuestObjective
    {
        public string id; // e.g., "skeleton"
        public QuestObjectiveType type = QuestObjectiveType.Kill;
        public int requiredAmount = 1;
    }

    [CreateAssetMenu(menuName = "Quests/Quest", fileName = "Quest_")]
    public class QuestSO : ScriptableObject
    {
        [Header("Info")]
        public string questId;
        public string title;
        [TextArea] public string description;

        [Header("Objectives")]
        public List<QuestObjective> objectives = new List<QuestObjective>();

        [Header("Rewards (optional)")]
        public int goldReward;
        public int expReward;
    }
}
