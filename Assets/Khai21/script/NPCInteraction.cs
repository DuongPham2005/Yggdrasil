using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public GameObject questPanel;
    public GameObject questProgressPanel;

    private bool isPlayerNear = false;
    private bool questAccepted = false;
    private int itemsCollected = 0;
    public int totalItemsNeeded = 3;

    void Update()
    {
        if (isPlayerNear && !questAccepted && Input.GetKeyDown(KeyCode.E))
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "Chào bạn! Giúp tôi nhặt 3 món đồ nhé?";
        }
    }

    public void AcceptQuest()
    {
        dialoguePanel.SetActive(false);
        questPanel.SetActive(false);
        questProgressPanel.SetActive(true);
        questAccepted = true;
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        questProgressPanel.GetComponentInChildren<Text>().text = $"Đã nhặt: {itemsCollected}/{totalItemsNeeded}";

        if (itemsCollected >= totalItemsNeeded)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "Cảm ơn bạn! Nhiệm vụ hoàn thành!";
            questProgressPanel.SetActive(false);
        }
    }

    public void CollectItem()
    {
        if (questAccepted && itemsCollected < totalItemsNeeded)
        {
            itemsCollected++;
            UpdateProgress();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            dialoguePanel.SetActive(false);
        }
    }
}
