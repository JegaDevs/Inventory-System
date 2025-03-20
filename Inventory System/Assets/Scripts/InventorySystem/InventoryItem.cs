using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Jega.InventorySystem
{
    public abstract class InventoryItem : ScriptableObject
    {
        public string Name;
        public ItemType Type;
        public float Weight;
        public Sprite Icon;
        public GameObject Prefab;
        [SerializeField, ReadOnly] private int id;

        public int ID => id;

        public enum ItemType : byte {Undefined,  Weapon, Drink, Object }
        
        public int GetCustomSavedAmount(string customKey, int startingAmount)
        {
            return PlayerPrefs.GetInt(customKey + name + "_ItemAmount", startingAmount);
        }
        public void SetCustomSavedAmount(string customKey, int startingAmount)
        {
            PlayerPrefs.SetInt(customKey + name + "_ItemAmount", startingAmount);
        }

        #if UNITY_EDITOR
        public void SetID(int newID)
        {
            id = newID;
            EditorUtility.SetDirty(this);
        }
        #endif  
    }
}
