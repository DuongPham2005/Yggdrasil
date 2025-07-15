using UnityEngine;
using System.Collections.Generic;

public class QuestTracker : MonoBehaviour
{
    public static QuestTracker Instance;

    public List<Quest> activeQuests = new List<Quest>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
            activeQuests.Add(quest);
    }

    public void EnemyKilled()
    {
        foreach (var quest in activeQuests)
        {
            if (!quest.isCompleted && quest.requiredEnemyKills > 0)
            {
                quest.currentEnemyKills++;
                Debug.Log($"{quest.questName} progress: {quest.currentEnemyKills}/{quest.requiredEnemyKills}");

                if (quest.isCompleted)
                {
                    Debug.Log($"{quest.questName} completed!");
                    QuestUIManager.Instance.UpdateQuestList();
                }
            }
        }
    }
}