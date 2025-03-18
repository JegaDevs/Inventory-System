using System.Collections.Generic;
using System.Collections.ObjectModel;
using NaughtyAttributes;
using UnityEngine;

namespace Jega.InventorySystem
{
    [CreateAssetMenu(fileName = "Item Colection", menuName = "JegaInventory/ItemCollection")]
    public class InventoryItemCollection : ScriptableObject
    {
        [SerializeField] private List<InventoryItem> collection;

        public ReadOnlyCollection<InventoryItem> Collection => collection.AsReadOnly();

        [Button]
        public void UpdateIds()
        {
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].SetID(i);
            }
        }
    }
}
