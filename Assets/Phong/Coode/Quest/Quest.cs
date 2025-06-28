using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Yggdrasil/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string description;
    public QuestType questType;
    public Item rewardItem;
    public int requiredEnemyKills;
    public int currentEnemyKills;

    public bool isCompleted => currentEnemyKills >= requiredEnemyKills;

}