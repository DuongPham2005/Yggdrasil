using Unity.FantasyKingdom;
using UnityEngine;

public class QuestBossNPC : MonoBehaviour
{
    public QuestBossManager questManager;
    private bool playerInRange = false;
    private bool dialogStarted = false;
    private int dialogStep = 0;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogStarted)
            {
                dialogStarted = true;
                dialogStep = 0;
                ShowNextDialog();
            }
            else
            {
                dialogStep++;
                ShowNextDialog();
            }
        }
    }

    private void ShowNextDialog()
    {
        switch (dialogStep)
        {
            case 0:
                QuestBossDialogUI.Instance.ShowDialog("NPC: Hello, lần đầu tiên bạn đến đây à?");
                break;
            case 1:
                QuestBossDialogUI.Instance.ShowDialog("Bạn: Đúng vậy, tôi đang tìm thanh gươm của vua, ông có thể chỉ cho tôi đường để lấy được không?");
                break;
            case 2:
                QuestBossDialogUI.Instance.ShowDialog("NPC: Hmmm, hiện tại thì thanh gươm đang được Veigar giữ rồi, muốn có nó có vẻ bạn phải đánh bại hắn ta.");
                break;
            case 3:
                QuestBossDialogUI.Instance.ShowDialog("Bạn: Được thôi tôi sẽ đánh bại hắn.");
                break;
            case 4:
                QuestBossDialogUI.Instance.HideDialog();
                questManager.StartQuest();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Có vật thể nào đó vừa vào trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player bước vào vùng nói chuyện.");
            QuestBossTalkHintUI.Instance.ShowHint("Nhấn E để nói chuyện");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player rời khỏi vùng nói chuyện.");
            QuestBossTalkHintUI.Instance.HideHint();
            QuestBossDialogUI.Instance.HideDialog();
        }
    }
}
