using UnityEngine;
using UnityEngine.UI;

public class SimplePanel : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private GameObject panelPrefab; // Prefab của panel
    [SerializeField] private Transform canvasParent; // Parent của canvas
    [SerializeField] private string playerTag = "Player";
    
    private GameObject currentPanel;
    private bool isPanelActive = false;
    
    void Start()
    {
        // Nếu không có canvas parent, tìm Canvas trong scene
        if (canvasParent == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                canvasParent = canvas.transform;
            }
            else
            {
                Debug.LogWarning("No Canvas found in scene. Please assign a canvas parent.");
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isPanelActive)
        {
            ShowPanel();
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
            ShowPanel();
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && isPanelActive)
        {
            HidePanel();
        }
    }
    
    public void ShowPanel()
    {
        if (panelPrefab == null || canvasParent == null) return;
        
        if (currentPanel == null)
        {
            currentPanel = Instantiate(panelPrefab, canvasParent);
        }
        
        currentPanel.SetActive(true);
        isPanelActive = true;
    }
    
    public void HidePanel()
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
            isPanelActive = false;
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
    
    // Method để thay đổi panel prefab
    public void SetPanelPrefab(GameObject newPrefab)
    {
        panelPrefab = newPrefab;
        
        // Ẩn panel hiện tại nếu có
        if (currentPanel != null)
        {
            Destroy(currentPanel);
            currentPanel = null;
            isPanelActive = false;
        }
    }
}
