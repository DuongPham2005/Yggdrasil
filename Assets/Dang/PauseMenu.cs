using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPanel;      // Panel chứa các nút
    public GameObject background;     // Background mờ phía sau
    public GameObject mainMenuUI;     // Root UI (có thể là Canvas chứa cả 2)
    private bool isPaused = false;

    void Start()
    {
        // Ban đầu tắt hết
        menuPanel.SetActive(false);
        background.SetActive(false);
        mainMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        mainMenuUI.SetActive(true);
        background.SetActive(true);
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        menuPanel.SetActive(false);
        background.SetActive(false);
        mainMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnContinueButton()
    {
        ResumeGame();
    }
}
