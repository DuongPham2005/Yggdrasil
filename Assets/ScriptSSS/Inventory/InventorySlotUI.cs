using UnityEngine;
using UnityEngine.UI;

namespace ScriptSSS.InventorySystem
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private Text countText;

        public void Set(InventorySlot slot)
        {
            if (slot == null || slot.IsEmpty)
            {
                if (icon != null)
                {
                    icon.enabled = false;
                    icon.sprite = null;
                }
                if (countText != null) countText.text = string.Empty;
                return;
            }

            if (icon != null)
            {
                icon.enabled = true;
                icon.sprite = slot.item.icon;
            }

            if (countText != null)
            {
                countText.text = (slot.item.stackable && slot.quantity > 1)
                    ? slot.quantity.ToString()
                    : string.Empty;
            }
        }
    }
}
