using TMPro;
using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class QuestBossDialogUI : MonoBehaviour
{
    public static QuestBossDialogUI Instance;
    public GameObject panel;               
    public TextMeshProUGUI dialogText;     

    private void Awake()
    {
        Instance = this;
        HideDialog();
    }

    public void ShowDialog(string text)
    {
        panel.SetActive(true);
        dialogText.text = text;
    }

    public void HideDialog()
    {
        panel.SetActive(false);
    }
}
}



