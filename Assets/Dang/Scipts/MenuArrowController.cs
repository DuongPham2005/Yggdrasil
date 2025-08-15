using UnityEngine;

public class MenuArrowController : MonoBehaviour
{
    public RectTransform arrow;           
    public RectTransform[] menuButtons;    
    public float moveSpeed = 10f;
    public float pulseSpeed = 5f;
    public float pulseAmount = 6f;

    private Vector3 targetPos;
    private float baseX;
    private int currentIndex = 0;

    void Start()
    {
        if (arrow == null) arrow = GetComponent<RectTransform>();
        baseX = arrow.localPosition.x;
        if (menuButtons != null && menuButtons.Length > 0)
            MoveArrowToButton(menuButtons[currentIndex], true);
    }

    void Update()
    {
      
        arrow.localPosition = Vector3.Lerp(arrow.localPosition, targetPos, Time.unscaledDeltaTime * moveSpeed);

        float offsetX = Mathf.Sin(Time.unscaledTime * pulseSpeed) * pulseAmount;
        arrow.localPosition = new Vector3(arrow.localPosition.x + offsetX, arrow.localPosition.y, arrow.localPosition.z);
    }

 
    public void MoveArrowToButton(RectTransform button, bool instant = false)
    {
        if (button == null) return;
        currentIndex = System.Array.IndexOf(menuButtons, button);
        targetPos = new Vector3(baseX, button.localPosition.y, arrow.localPosition.z);
        if (instant)
        {
            arrow.localPosition = new Vector3(targetPos.x + Mathf.Sin(Time.unscaledTime * pulseSpeed) * pulseAmount, targetPos.y, targetPos.z);
        }
    }
}
