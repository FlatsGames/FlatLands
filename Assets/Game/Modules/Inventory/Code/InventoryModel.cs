using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.Items;

namespace FlatLands.Inventory
{
    public class InventoryModel : SharedObject
    {

        private Dictionary<int, ItemData> _inventoryDatas = new Dictionary<int, ItemData>();
        

        public override void Init()
        {
            
        }

        public override void Dispose()
        {
            
        }

        #region AddItem
        
        public bool CanAddItem(ItemData data)
        {
            return true;
        }

        public bool TryAddItem(ItemData data)
        {
            if (!CanAddItem(data))
                return false;
            
            return true;
        }

        #endregion

        #region RemoveItem
        
        public bool CanRemoveItem(ItemData data)
        {
            return true;
        }
        
        public bool TryRemoveItem(ItemData data)
        {
            if (!CanRemoveItem(data))
                return false;

            return true;
        }
        
        #endregion

        #region AddSlots

        public void AddSlots(int count)
        {
            for (var i = _inventoryDatas.Count; i < count + _inventoryDatas.Count; i++)
            {
                _inventoryDatas.Add(i, null);
            }
        }

        public void RemoveSlots(int count)
        {
            for (int i = _inventoryDatas.Count - 1; i < count + _inventoryDatas.Count; i--)
            {
                //нужно дописать дроп предметов
                _inventoryDatas.Remove(i);
            }
            
        }
        
        #endregion

        // public bool HaveEmptySlots()
        // {
        //     var emptySlots = GetEmptySlots();
        //
        //     if (emptySlots.Count > 0)
        //         return true;
        //     
        //     return false;
        // }

        public List<int> GetEmptySlots()
        {
            var emptySlots = new List<int>();

            foreach (var slot in _inventoryDatas)
            {
                if (slot.Value != null)
                {
                    continue;
                }
                emptySlots.Add(slot.Key);
            }

            return emptySlots;
        }

    }
}