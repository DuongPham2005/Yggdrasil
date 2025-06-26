using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class NPCInteract : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public Quest questToGive;
    public bool autoGiveQuestAfterDialogue = true;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTying, isDialogueActive;

    public bool CanIneract()
    {
        return !PauseController.IsGamePaused || isDialogueActive;
    }
    public void Interact()
    {
        //neu ko dialogue data hoac game dang paused va khong dialogue dang hoat dong
        if (dialoguePanel == null || (PauseController.IsGamePaused && !isDialogueActive))
        {
            return;
        }

        if (isDialogueActive)
        {
            NextLine();

        }
        else
        {
            StartDialogue();
        }
    }
    // chay hoi thoai
    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.NPCname);
        portraitImage.sprite = dialogueData.npcPortrait;
        // if (dialogueData == null)
        //     Debug.LogError("dialogueData is NULL!");

        // if (nameText == null)
        //     Debug.LogError("nameText is NULL!");

        dialoguePanel.SetActive(true);
        PauseController.SetPause(false);

        StartCoroutine(TypeLine());
    }

    //////Line chay tiep theo
    public void NextLine()
    {
        if (isTying)
        {
            //bo qua animation va hien het line
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTying = false;

        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            //neu co dong tiep theo, chay tiep
            StartCoroutine(TypeLine());

        }
        else
        {
            EndDialogue();
        }
    }
    IEnumerator TypeLine()
    {
        isTying = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(dialogueData.typingSpeed);

        }

        isTying = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(dialogueData.autoProgressDelay);

            NextLine();

        }
    }
        // ket thuc hoi thoai
    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);

        if (autoGiveQuestAfterDialogue && questToGive != null && !questToGive.isCompleted)
        {
            GiveQuestToPlayer();
        }
    }
    public void GiveQuestToPlayer()
    {
        //Debug.Log($"Requested Mission {(questToGive.questType == QuestType.Main ? "Main" : "Side")}: {questToGive.questName}");
        
        Debug.Log($"Requested Mission {questToGive.questType}: {questToGive.questName}");
            // Thêm quest vào hệ thống UI (nếu chưa có)
        if (QuestUIManager.Instance != null)
        {
            QuestUIManager.Instance.AddQuest(questToGive);
            QuestUIManager.Instance.ToggleQuestPanel(); // Mở bảng nhiệm vụ luôn
        }   
         // Cộng thưởng ngay nếu bạn muốn (hoặc đợi khi người chơi hoàn thành)
        if (questToGive.rewardItem != null)
        {
            Inventory.Instance.AddItem(questToGive.rewardItem);
            Debug.Log($"Collected Item: {questToGive.rewardItem.itemName}");
        }

        questToGive.isCompleted = true;
    }

    
    ///
    public static class PauseController
    {
        private static bool isPaused = false;

        public static bool IsGamePaused => isPaused;

        public static void SetPause(bool pause)
        {
            isPaused = pause;
            Time.timeScale = pause ? 0 : 1;

            Debug.Log("Pause state: " + isPaused);
        }
    }

}
