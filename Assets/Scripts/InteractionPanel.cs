using UnityEngine;
using UnityEngine.UI;

public class InteractionPanel : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private GameObject panelToShow; // Panel sẽ hiển thị
    [SerializeField] private string playerTag = "Player"; // Tag của player
    [SerializeField] private bool showOnTrigger = true; // Hiển thị khi trigger
    [SerializeField] private bool showOnCollision = false; // Hiển thị khi collision
    
    [Header("Animation Settings")]
    [SerializeField] private bool useAnimation = true; // Có sử dụng animation không
    [SerializeField] private float fadeInDuration = 0.3f; // Thời gian fade in
    [SerializeField] private float fadeOutDuration = 0.3f; // Thời gian fade out
    
    [Header("Auto Hide")]
    [SerializeField] private bool autoHide = false; // Tự động ẩn panel
    [SerializeField] private float autoHideDelay = 3f; // Thời gian delay trước khi ẩn
    
    private CanvasGroup canvasGroup;
    private bool isPanelVisible = false;
    private Coroutine autoHideCoroutine;
    
    void Start()
    {
        // Tìm CanvasGroup component, nếu không có thì tạo mới
        if (panelToShow != null)
        {
            canvasGroup = panelToShow.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = panelToShow.AddComponent<CanvasGroup>();
            }
            
            // Ẩn panel khi bắt đầu
            HidePanel();
        }
        else
        {
            Debug.LogWarning("Panel to show is not assigned in " + gameObject.name);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (showOnTrigger && other.CompareTag(playerTag))
        {
            ShowPanel();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (showOnTrigger && other.CompareTag(playerTag))
        {
            HidePanel();
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (showOnCollision && collision.gameObject.CompareTag(playerTag))
        {
            ShowPanel();
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (showOnCollision && collision.gameObject.CompareTag(playerTag))
        {
            HidePanel();
        }
    }
    
    public void ShowPanel()
    {
        if (panelToShow == null) return;
        
        // Dừng coroutine auto hide nếu có
        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
            autoHideCoroutine = null;
        }
        
        panelToShow.SetActive(true);
        isPanelVisible = true;
        
        if (useAnimation)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            canvasGroup.alpha = 1f;
        }
        
        // Bắt đầu auto hide nếu được bật
        if (autoHide)
        {
            autoHideCoroutine = StartCoroutine(AutoHideAfterDelay());
        }
    }
    
    public void HidePanel()
    {
        if (panelToShow == null) return;
        
        // Dừng coroutine auto hide
        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
            autoHideCoroutine = null;
        }
        
        if (useAnimation)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            canvasGroup.alpha = 0f;
            panelToShow.SetActive(false);
            isPanelVisible = false;
        }
    }
    
    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
    
    private System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 1f;
        
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        panelToShow.SetActive(false);
        isPanelVisible = false;
    }
    
    private System.Collections.IEnumerator AutoHideAfterDelay()
    {
        yield return new WaitForSeconds(autoHideDelay);
        HidePanel();
    }
    
    // Public method để kiểm tra trạng thái panel
    public bool IsPanelVisible()
    {
        return isPanelVisible;
    }
    
    // Public method để toggle panel
    public void TogglePanel()
    {
        if (isPanelVisible)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }
    
    // Method để thay đổi panel động
    public void SetPanel(GameObject newPanel)
    {
        if (isPanelVisible)
        {
            HidePanel();
        }
        
        panelToShow = newPanel;
        
        if (panelToShow != null)
        {
            canvasGroup = panelToShow.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = panelToShow.AddComponent<CanvasGroup>();
            }
        }
    }
}
