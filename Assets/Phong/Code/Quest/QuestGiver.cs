using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class QuestGiver : MonoBehaviour, IInteractable
{
    public Quest questToGive;

    public bool CanIneract() => !questToGive.isCompleted;

    public void Interact()
    {
        if (!questToGive.isCompleted)
        {
            // Debug.Log("Requested Mission: " + questToGive.questName);
            // Inventory.Instance.AddItem(questToGive.rewardItem);
            // questToGive.isCompleted = true;
            QuestPopupManager.Instance.ShowQuest(questToGive);
        }
    }
}
