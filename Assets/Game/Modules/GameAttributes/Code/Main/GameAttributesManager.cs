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

        public void RegisterHolder(string id, IGameAttributeHolder holder)
        {
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

        public void RemoveHolder(string id)
        {
            if (!_attributeHolders.TryGetValue(id, out var holder))
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
    }
}