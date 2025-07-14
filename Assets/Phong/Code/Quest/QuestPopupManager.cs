using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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