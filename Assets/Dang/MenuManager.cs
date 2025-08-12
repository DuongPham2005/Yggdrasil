using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menuPanel;    // panel chứa menu (bao gồm background nếu bạn đặt chung)
    public GameObject background;   // nếu background tách ra, gán vào đây
    public bool pauseGameWhenOpen = true;

    private bool isPaused = false;

    void Start()
    {
        // đảm bảo trạng thái khởi tạo
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

    // --- Compatibility alias: giữ tên CloseMenu() để code cũ không lỗi
    public void CloseMenu()
    {
        ResumeGame();
    }

    // Continue gọi Resume
    public void ContinueGame()
    {
        ResumeGame();
    }

    // Restart / Quit helper (thay tên/nội dung nếu cần)
    public void RestartGame(string sceneName = "Test2")
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
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
