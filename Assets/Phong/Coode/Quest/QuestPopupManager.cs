using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;


public class QuestPopupManager : MonoBehaviour
{
    public static QuestPopupManager Instance;

    [Header("UI References")]
    public GameObject popupPanel;
    public TMP_Text questNameText;
    public TMP_Text questDescriptionText;
    public Button acceptButton;
    public Button declineButton;

    private Quest pendingQuest;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    public void ShowQuest(Quest quest)
    {
        pendingQuest = quest;

        if (questNameText != null) questNameText.text = quest.questName;
        if (questDescriptionText != null) questDescriptionText.text = quest.description;

        popupPanel.SetActive(true);

        // Ensure UI events update correctly
        EventSystem.current.SetSelectedGameObject(null);
        if (acceptButton != null)
            acceptButton.Select();
    }

    public void OnAcceptQuest()
    {

        if (pendingQuest != null)
        {
            QuestTracker.Instance.AddQuest(pendingQuest);
            QuestUIManager.Instance.AddQuest(pendingQuest);
        }

        popupPanel.SetActive(false);
    }

    public void OnDeclineQuest()
    {
        popupPanel.SetActive(false);
    }
}
    // public void ShowQuest(Quest quest)
    // {
    //     pendingQuest = quest;
    //     questNameText.text = quest.questName;
    //     questDescriptionText.text = quest.description;
    //     popupPanel.SetActive(false);
    // }

    // public void OnAcceptQuest()
    // {
    //     if (pendingQuest != null)
    //     {
    //         QuestUIManager.Instance.AddQuest(pendingQuest);
    //         Debug.Log($"Quest Accepted: {pendingQuest.questName}");
    //     }
    //     pendingQuest = null;
    //     popupPanel.SetActive(false);
    // }

    // public void OnDeclineQuest()
    // {
    //     Debug.Log("Quest Declined");
    //     pendingQuest = null;
    //     popupPanel.SetActive(false);
    // }
