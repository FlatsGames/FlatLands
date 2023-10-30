using System;
using System.Collections.Generic;
using FlatLands.Architecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.UI
{
    [CreateAssetMenu(
        menuName = "FlatLands/UI/" + nameof(GeneralUIConfig), 
        fileName = nameof(GeneralUIConfig))]
    internal sealed class GeneralUIConfig : SingletonScriptableObject<GeneralUIConfig>
    {
        [SerializeField] 
        private Dictionary<UIWindowType, UIWindowPair> _windows;

        public IReadOnlyDictionary<UIWindowType, UIWindowPair> Windows => _windows;
    }

    public sealed class UIWindowPair
    {
        [SerializeField] 
        private bool _preload = true;
        
        [SerializeField] 
        private UIWindow _prefab;

        public bool PreloadPrefab => _preload;
        public UIWindow Prefab => _prefab;
        public Type PrefabType => _prefab != null ? _prefab.GetType() : default;
    }
}