using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCQuestGiver : MonoBehaviour
{
    public Quest quest;
    public QuestType questType;

    void Awake()
    {
        if (quest != null) quest.questType = questType;
    }
}

