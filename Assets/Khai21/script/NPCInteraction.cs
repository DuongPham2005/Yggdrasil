using UnityEngine;
using TMPro; // DÙNG CHO TextMeshPro

public class NPCInteraction : MonoBehaviour
{

    public bool  vachamnayno = false; 
    public GameObject dialoguePanel;
    public TMP_Text dialogueText; // Đã đổi từ UnityEngine.UI.Text → TMP_Text
    
    public GameObject questPanel;
    public GameObject questProgressPanel;
    public TMP_Text questProgressText; // THÊM để hiển thị tiến trình
    public GameObject thanksPanel; 

    private bool isPlayerNear = false;
    private bool questAccepted = false;
    private int itemsCollected = 0;
    public int totalItemsNeeded = 3;

    void Update()
    {
        if (isPlayerNear && !questAccepted && Input.GetKeyDown(KeyCode.F))
        {
            if (dialoguePanel != null && dialogueText != null)
            {
                dialoguePanel.SetActive(true);
                dialogueText.text = "Chào bạn! Giúp tôi nhặt 3 món đồ nhé?";
            }
            else
            {
                Debug.LogError("dialoguePanel hoặc dialogueText chưa được gán trong Inspector.");
            }
        }
    }

   public void talk()
    {

         dialoguePanel.SetActive(false);
         questPanel.SetActive(true);   
    }
    public void AcceptQuest()
    {

       
        questPanel.SetActive(false);
        questProgressPanel.SetActive(true);
        questAccepted = true;
        UpdateProgress();
    }


    public void UpdateProgress()
    {
        if (questProgressText != null)
        {
            questProgressText.text = $"Đã nhặt: {itemsCollected}/{totalItemsNeeded}";
        }
        else
        {
            Debug.LogError("questProgressText chưa được gán trong Inspector.");
        }

      

    }

    public void done()
    {
        thanksPanel.SetActive(false); 
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
        {
            vachamnayno = true; 
            isPlayerNear = true;
        }

        if (itemsCollected >= totalItemsNeeded)
        {
            thanksPanel.SetActive(true);
            questProgressPanel.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vachamnayno = false;
            isPlayerNear = false;
            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);
        }
    }


}
