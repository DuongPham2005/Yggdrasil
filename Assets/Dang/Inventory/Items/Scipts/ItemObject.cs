using UnityEngine;
using System.Collections;
using System.Collections.Concurrent;

namespace Unity.FantasyKingdom
{
    public enum ItemType
    {
        Food,
        Equipment,
        Default
    }
    public abstract class ItemObject : ScriptableObject
    {
        public GameObject prefab;
        public ItemType type;
        [TextArea(15, 20)]
        public string description;
    }
}
