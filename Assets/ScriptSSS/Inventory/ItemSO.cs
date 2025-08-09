using UnityEngine;

namespace ScriptSSS.InventorySystem
{
    [CreateAssetMenu(menuName = "Inventory/Item", fileName = "Item_")]
    public class ItemSO : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;
        [TextArea] public string description;

        [Header("Visual")]
        public Sprite icon;

        [Header("Stacking")]
        public bool stackable = true;
        [Min(1)] public int maxStack = 99;
    }
}
