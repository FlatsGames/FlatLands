using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Items
{
    public class ItemView : MonoBehaviour
    {
        private int _id;
        
        public string Id => gameObject.name + _id;
        
        public void SetId(int index)
        {
            _id = index;
        }
    }
}