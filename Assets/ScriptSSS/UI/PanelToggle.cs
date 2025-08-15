using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelToggle : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private float slideDistance = 300f; // Khoảng cách di chuyển sang trái
    [SerializeField] private float slideDuration = 0.3f; // Thời gian di chuyển
    
    [Header("Input Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.V; // Phím để toggle panel
    
    private Vector2 originalPosition; // Vị trí ban đầu của panel
    private Vector2 targetPosition; // Vị trí đích khi di chuyển
    private bool isPanelVisible = true; // Trạng thái hiện tại của panel
    private bool isAnimating = false; // Đang trong quá trình animation
    
    private void Start()
    {
        // Lưu vị trí ban đầu của panel
        if (panel != null)
        {
            originalPosition = panel.anchoredPosition;
            targetPosition = originalPosition + Vector2.left * slideDistance;
        }
        else
        {
            Debug.LogError("Panel không được gán! Hãy gán RectTransform của panel vào script này.");
        }
    }
    
    private void Update()
    {
        // Kiểm tra input khi nhấn phím V
        if (Input.GetKeyDown(toggleKey) && !isAnimating)
        {
            TogglePanel();
        }
    }
    
    /// <summary>
    /// Toggle panel giữa vị trí hiển thị và ẩn
    /// </summary>
    public void TogglePanel()
    {
        if (isAnimating) return;
        
        if (isPanelVisible)
        {
            // Ẩn panel (di chuyển sang trái)
            StartCoroutine(SlidePanel(targetPosition));
        }
        else
        {
            // Hiển thị panel (trở về vị trí ban đầu)
            StartCoroutine(SlidePanel(originalPosition));
        }
        
        isPanelVisible = !isPanelVisible;
    }
    
    /// <summary>
    /// Di chuyển panel đến vị trí đích với animation mượt mà
    /// </summary>
    /// <param name="targetPos">Vị trí đích</param>
    private IEnumerator SlidePanel(Vector2 targetPos)
    {
        isAnimating = true;
        Vector2 startPos = panel.anchoredPosition;
        float elapsedTime = 0f;
        
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / slideDuration;
            
            // Sử dụng EaseInOut để animation mượt mà hơn
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);
            
            panel.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedProgress);
            yield return null;
        }
        
        // Đảm bảo panel ở đúng vị trí cuối cùng
        panel.anchoredPosition = targetPos;
        isAnimating = false;
    }
    
    /// <summary>
    /// Hiển thị panel (trở về vị trí ban đầu)
    /// </summary>
    public void ShowPanel()
    {
        if (!isPanelVisible && !isAnimating)
        {
            StartCoroutine(SlidePanel(originalPosition));
            isPanelVisible = true;
        }
    }
    
    /// <summary>
    /// Ẩn panel (di chuyển sang trái)
    /// </summary>
    public void HidePanel()
    {
        if (isPanelVisible && !isAnimating)
        {
            StartCoroutine(SlidePanel(targetPosition));
            isPanelVisible = false;
        }
    }
    
    /// <summary>
    /// Thiết lập panel từ bên ngoài
    /// </summary>
    /// <param name="newPanel">RectTransform của panel mới</param>
    public void SetPanel(RectTransform newPanel)
    {
        panel = newPanel;
        originalPosition = panel.anchoredPosition;
        targetPosition = originalPosition + Vector2.left * slideDistance;
        isPanelVisible = true;
    }
    
    /// <summary>
    /// Thiết lập khoảng cách di chuyển
    /// </summary>
    /// <param name="distance">Khoảng cách mới</param>
    public void SetSlideDistance(float distance)
    {
        slideDistance = distance;
        targetPosition = originalPosition + Vector2.left * slideDistance;
    }
    
    /// <summary>
    /// Thiết lập thời gian di chuyển
    /// </summary>
    /// <param name="duration">Thời gian mới</param>
    public void SetSlideDuration(float duration)
    {
        slideDuration = duration;
    }
}
