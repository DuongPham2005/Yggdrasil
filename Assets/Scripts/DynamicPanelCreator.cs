using UnityEngine;
using UnityEngine.UI;

public class DynamicPanelCreator : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Vector2 panelSize = new Vector2(300, 200);
    [SerializeField] private Color panelColor = Color.white;
    [SerializeField] private string panelText = "Welcome!";
    [SerializeField] private int fontSize = 24;
    [SerializeField] private Color textColor = Color.black;
    
    private GameObject currentPanel;
    private bool isPanelActive = false;
    private Canvas canvas;
    
    void Start()
    {
        // Tìm Canvas trong scene
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("No Canvas found in scene. Creating one...");
            CreateCanvas();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isPanelActive)
        {
            CreateAndShowPanel();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && isPanelActive)
        {
            HidePanel();
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && !isPanelActive)
        {
            CreateAndShowPanel();
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && isPanelActive)
        {
            HidePanel();
        }
    }
    
    private void CreateCanvas()
    {
        // Tạo GameObject mới cho Canvas
        GameObject canvasGO = new GameObject("Canvas");
        canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        // Thêm CanvasScaler
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Thêm GraphicRaycaster
        canvasGO.AddComponent<GraphicRaycaster>();
        
        // Thêm EventSystem nếu chưa có
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }
    
    private void CreateAndShowPanel()
    {
        if (canvas == null) return;
        
        // Tạo panel background
        GameObject panel = new GameObject("DynamicPanel");
        panel.transform.SetParent(canvas.transform, false);
        
        // Thêm Image component cho background
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = panelColor;
        
        // Thiết lập RectTransform
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = panelSize;
        rectTransform.anchoredPosition = Vector2.zero;
        
        // Tạo text
        GameObject textGO = new GameObject("PanelText");
        textGO.transform.SetParent(panel.transform, false);
        
        Text text = textGO.AddComponent<Text>();
        text.text = panelText;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.color = textColor;
        text.alignment = TextAnchor.MiddleCenter;
        
        // Thiết lập RectTransform cho text
        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        currentPanel = panel;
        isPanelActive = true;
    }
    
    public void HidePanel()
    {
        if (currentPanel != null)
        {
            Destroy(currentPanel);
            currentPanel = null;
            isPanelActive = false;
        }
    }
    
    public void ShowPanel()
    {
        if (!isPanelActive)
        {
            CreateAndShowPanel();
        }
    }
    
    public void TogglePanel()
    {
        if (isPanelActive)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }
    
    // Method để thay đổi text của panel
    public void SetPanelText(string newText)
    {
        panelText = newText;
        if (currentPanel != null)
        {
            Text text = currentPanel.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = newText;
            }
        }
    }
    
    // Method để thay đổi kích thước panel
    public void SetPanelSize(Vector2 newSize)
    {
        panelSize = newSize;
        if (currentPanel != null)
        {
            RectTransform rectTransform = currentPanel.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = newSize;
            }
        }
    }
    
    // Method để thay đổi màu panel
    public void SetPanelColor(Color newColor)
    {
        panelColor = newColor;
        if (currentPanel != null)
        {
            Image image = currentPanel.GetComponent<Image>();
            if (image != null)
            {
                image.color = newColor;
            }
        }
    }
}
