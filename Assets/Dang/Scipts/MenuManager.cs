using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menuPanel;    
    public GameObject background;   
    public bool pauseGameWhenOpen = true;

    private bool isPaused = false;

    void Start()
    {
 
        if (menuPanel != null) menuPanel.SetActive(false);
        if (background != null) background.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;
        if (menuPanel != null) menuPanel.SetActive(true);
        if (background != null) background.SetActive(true);

        if (pauseGameWhenOpen) Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isPaused = true;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        if (menuPanel != null) menuPanel.SetActive(false);
        if (background != null) background.SetActive(false);

        if (pauseGameWhenOpen) Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isPaused = false;
    }

   
    public void CloseMenu()
    {
        ResumeGame();
    }

   
    public void ContinueGame()
    {
        ResumeGame();
    }

    
    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
