using System.Collections.Generic;
using UnityEngine;

namespace ScriptSSS.InventorySystem
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private Transform gridParent;
        [SerializeField] private InventorySlotUI slotPrefab;
        [SerializeField] private GameObject rootPanel;
        [SerializeField] private KeyCode toggleKey = KeyCode.I;

        private readonly List<InventorySlotUI> slotUis = new List<InventorySlotUI>();

        private enum ToggleMode { SetActive, CanvasGroup }
        private ToggleMode toggleMode = ToggleMode.SetActive;
        private CanvasGroup canvasGroup;

        private void Start()
        {
            if (inventory == null) inventory = FindObjectOfType<Inventory>();
            if (inventory != null) inventory.OnChanged += Refresh;

            // Determine how to toggle visibility
            if (rootPanel == null) rootPanel = gameObject;
            if (rootPanel == gameObject)
            {
                canvasGroup = rootPanel.GetComponent<CanvasGroup>();
                if (canvasGroup == null) canvasGroup = rootPanel.AddComponent<CanvasGroup>();
                toggleMode = ToggleMode.CanvasGroup;
            }
            else
            {
                toggleMode = ToggleMode.SetActive;
            }

            BuildSlots();
            Refresh();
            SetVisible(false);
        }

        private void OnDestroy()
        {
            if (inventory != null) inventory.OnChanged -= Refresh;
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                SetVisible(!IsVisible());
                Refresh();
            }
        }

        private void BuildSlots()
        {
            if (gridParent == null || slotPrefab == null || inventory == null) return;

            foreach (Transform child in gridParent) Destroy(child.gameObject);
            slotUis.Clear();

            for (int i = 0; i < inventory.Capacity; i++)
            {
                var ui = Instantiate(slotPrefab, gridParent);
                slotUis.Add(ui);
            }
        }

        private void Refresh()
        {
            if (inventory == null || slotUis.Count == 0) return;
            var slots = inventory.Slots;
            for (int i = 0; i < slotUis.Count; i++)
                slotUis[i].Set(slots[i]);
        }

        private bool IsVisible()
        {
            switch (toggleMode)
            {
                case ToggleMode.CanvasGroup:
                    return canvasGroup != null && canvasGroup.alpha > 0.5f;
                default:
                    return rootPanel != null && rootPanel.activeSelf;
            }
        }

        private void SetVisible(bool visible)
        {
            switch (toggleMode)
            {
                case ToggleMode.CanvasGroup:
                    if (canvasGroup == null) return;
                    canvasGroup.alpha = visible ? 1f : 0f;
                    canvasGroup.interactable = visible;
                    canvasGroup.blocksRaycasts = visible;
                    break;
                default:
                    if (rootPanel != null) rootPanel.SetActive(visible);
                    break;
            }
        }
    }
}
