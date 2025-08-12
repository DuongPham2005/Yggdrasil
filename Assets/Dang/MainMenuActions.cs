using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    public MenuManager menuManager; // gán MenuManager

    public void Continue()
    {
        // nếu đang ở trong gameplay: chỉ đóng menu
        if (menuManager != null) menuManager.CloseMenu();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // chắc chắn resume trước khi load
        SceneManager.LoadScene("Test2"); // đổi "GameScene" thành tên scene của bạn
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
