using System.Collections.Generic;
using UnityEngine;

namespace Jega.InventorySystem
{
    [CreateAssetMenu(fileName = "Inventory Data", menuName = "JegaInventory/InventoryData")]
    public class InventoryData : ScriptableObject
    {
        [Header("Game Design Params")]
        public List<Inventory.StartingItem> startingItems;
        public int numberOfSlots;

        [Header("Programming Params")]
        [SerializeField] public InventoryItemCollection itemCollection;
        [SerializeField] public string inventorySaveKey;
    }
}
