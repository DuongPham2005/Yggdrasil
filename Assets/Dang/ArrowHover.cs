using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowHover : MonoBehaviour, IPointerEnterHandler
{
    public MenuArrowController arrowController;
    public RectTransform target; // gán RectTransform của chính nút này

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (arrowController != null && target != null)
            arrowController.MoveArrowToButton(target);
    }
}
