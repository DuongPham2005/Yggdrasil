
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
namespace Unity.FantasyKingdom
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/ Inventory")]
    public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public string savePath;
        private ItemDatabaseObject databse;
        public List<InventorySlot> Container = new List<InventorySlot>();

        private void OnEnable()
        {
#if UNITY_EDITOR
       
            databse = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Dang/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
            databse = Resources.Load<ItemDatabaseObject>("DataBase");
#endif
        }






        public void AddItem(ItemObject _item, int _amount)
        {
            
            for (int i = 0; i< Container.Count; i++)
            {
                if (Container[i].Item == _item)
                {
                    Container[i].AddAmount(_amount);
                    return;
                }
            }
            
                Container.Add(new InventorySlot(databse.GetId[_item],_item, _amount));
            
        }
        public void Save()
        {
            string saveData = JsonUtility.ToJson(this, true);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
            bf.Serialize(file, saveData);
            file.Close();

        }
        public void Load ()
        {
            if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
                JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(),this);
                file.Close();
            }
        }
        public void OnAfterDeserialize()
        {
            for (int i = 0; i < Container.Count; i++)
            {
                Container[i].Item = databse.GetItem[Container[i].ID];
             }
        }
        public void OnBeforeSerialize()
        {

        }
    }
    [System.Serializable]
    public class InventorySlot
    {
        public int ID;
        public ItemObject Item;
        public int amount;
        public InventorySlot(int _id, ItemObject _item, int _amount)
        {
            ID = _id;
            Item = _item;
            amount = _amount;
        }
        public void AddAmount(int value)
        {
            amount += value;
        }
        
    }

}
