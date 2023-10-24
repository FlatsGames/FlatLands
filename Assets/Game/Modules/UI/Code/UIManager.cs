using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.UI
{
    public sealed class UIManager : SharedObject, IGeneralSceneLoader, IOverlayCameraHolder
    {
        public UIHierarchy Hierarchy => _hierarchy;
        private UIHierarchy _hierarchy;
        
        public override void Init()
        {
            
        }

        public override void Dispose()
        {
           
        }

#region ISceneLoader

        public bool NeedLoad => true;
        public int LoadingSceneOrder => 20;
        public string GetLoadingSceneName() => "UIScene";
        public void InvokeSceneLoaded()
        {
            _hierarchy = GameObject.FindObjectOfType<UIHierarchy>();
        }

#endregion


#region IOverlayCameraHolder

        public string Id => "UICamera";

        public Camera GetOverlayCamera =>  _hierarchy?.UICamera;

#endregion

    }
}

