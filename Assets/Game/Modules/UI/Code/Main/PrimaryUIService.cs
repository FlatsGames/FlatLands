using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.UI
{
    public sealed class PrimaryUIService : PrimarySharedObject, IGeneralSceneLoader
    {
        [Inject] private UIManager _uiManager;
        
        public override void Init()
        {
            
        }

        public override void Dispose()
        {
            
        }

#region IGeneralSceneLoader

        public bool NeedLoad => true;

        public int LoadingSceneOrder => 20;

        public string GetLoadingSceneName() => "UIScene";

        public void InvokeSceneLoaded()
        {
            var hierarchy = GameObject.FindObjectOfType<UIHierarchy>();
            _uiManager.InvokeSceneLoaded(hierarchy);
        }

#endregion

    }
}