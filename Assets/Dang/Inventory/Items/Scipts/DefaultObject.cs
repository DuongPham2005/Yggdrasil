using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Unity.FantasyKingdom
{
    [CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
    public class DefaultObject : ItemObject
    {
        public void Awake()
        {
            type = ItemType.Default;
        }
    }
}
