using UnityEngine;
using System.Collections;

public class SimplePanelToggle : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private float slideDistance = 300f;
    [SerializeField] private float slideSpeed = 5f;
    
    private Vector2 originalPosition;
    private Vector2 hiddenPosition;
    private bool isVisible = true;
    private bool isMoving = false;
    
    void Start()
    {
        if (panel != null)
        {
            originalPosition = panel.anchoredPosition;
            hiddenPosition = originalPosition + Vector2.left * slideDistance;
        }
    }
    
    void Update()
    {
        // Nhấn V để toggle panel
        if (Input.GetKeyDown(KeyCode.V) && !isMoving)
        {
            TogglePanel();
        }
    }
    
    void TogglePanel()
    {
        if (isVisible)
        {
            StartCoroutine(MovePanel(hiddenPosition));
        }
        else
        {
            StartCoroutine(MovePanel(originalPosition));
        }
        isVisible = !isVisible;
    }
    
    IEnumerator MovePanel(Vector2 targetPosition)
    {
        isMoving = true;
        
        while (Vector2.Distance(panel.anchoredPosition, targetPosition) > 0.1f)
        {
            panel.anchoredPosition = Vector2.MoveTowards(
                panel.anchoredPosition, 
                targetPosition, 
                slideSpeed * Time.deltaTime * 100f
            );
            yield return null;
        }
        
        panel.anchoredPosition = targetPosition;
        isMoving = false;
    }
}
