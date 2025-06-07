using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestUIManager : MonoBehaviour
{
    public GameObject questPanel;
    public GameObject questEntryPrefab;
    public Transform questListContainer;

    public List<Quest> currentQuests = new List<Quest>();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleQuestPanel();
        }
    }
    public void ToggleQuestPanel()
    {
        questPanel.SetActive(!questPanel.activeSelf);
        if (questPanel.activeSelf)
            UpdateQuestList();
    }

    public void AddQuest(Quest quest)
    {
        if (!currentQuests.Contains(quest))
            currentQuests.Add(quest);
    }

    public void UpdateQuestList()
    {
        foreach (Transform child in questListContainer)
            Destroy(child.gameObject);

        foreach (Quest quest in currentQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContainer);
            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
            text.text = $"{quest.questName} [{quest.questType}] - {(quest.isCompleted ? "✔️ Hoàn thành" : "⏳ Chưa xong")}";
        }
    }
}
