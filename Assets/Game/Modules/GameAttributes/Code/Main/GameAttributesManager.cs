using System.Collections.Generic;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.GameAttributes
{
    public sealed class GameAttributesManager : SharedObject
    {
        private Dictionary<string, IGameAttributeHolder> _attributeHolders;

        public GameAttributesManager()
        {
            _attributeHolders = new Dictionary<string, IGameAttributeHolder>();
        }

        public void RegisterHolder(IGameAttributeHolder holder)
        {
            var id = holder.HolderId;
            if (_attributeHolders.ContainsKey(id))
            {
                Debug.LogError($"[GameAttributesManager] Holder with id: {id} already exist!");
                return;
            }

            var attributes = holder.GetAttributes();
            foreach (var pair in attributes)
            {
                pair.Value.Init();
            }
            
            _attributeHolders[id] = holder;
        }

        public void RemoveHolder(IGameAttributeHolder holder)
        {
            var id = holder.HolderId;
            if (!_attributeHolders.ContainsKey(id))
            {
                Debug.LogError($"[GameAttributesManager] Holder with id: {id} not fount!");
                return;
            }
            
            var attributes = holder.GetAttributes();
            foreach (var pair in attributes)
            {
                pair.Value.Dispose();
            }
            
            _attributeHolders.Remove(id);
        }

        public T GetAttributeHolder<T>(string holderId) where T: IGameAttributeHolder
        {
            var holder = GetAttributeHolder(holderId);
            return (T) holder;
        }
        
        public IGameAttributeHolder GetAttributeHolder(string holderId)
        {
            if (!_attributeHolders.TryGetValue(holderId, out var holder))
                return default;

            return holder;
        }
    }
}