using UnityEngine;
using UnityEngine.UI;

public class TriggerPanel : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private GameObject panel; // Panel cần hiển thị
    [SerializeField] private string playerTag = "Player"; // Tag của player
    
    void Start()
    {
        // Ẩn panel khi bắt đầu
        if (panel != null)
        {
            panel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Panel not assigned in " + gameObject.name);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && panel != null)
        {
            panel.SetActive(true);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && panel != null)
        {
            panel.SetActive(false);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && panel != null)
        {
            panel.SetActive(true);
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && panel != null)
        {
            panel.SetActive(false);
        }
    }
    
    // Public methods để điều khiển panel
    public void ShowPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }
    
    public void HidePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
    
    public void TogglePanel()
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }
}
