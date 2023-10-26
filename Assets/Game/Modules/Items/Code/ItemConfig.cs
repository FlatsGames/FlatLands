using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Items
{
    [CreateAssetMenu(
        menuName = "FlatLands/Items/" + nameof(ItemConfig), 
        fileName = nameof(ItemConfig))]
    public sealed class ItemConfig : MultitonScriptableObjectsByName<ItemConfig>
    {
       
    }
    
    
}