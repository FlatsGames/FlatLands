using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.UI
{
    public sealed class UIManager : SharedObject, IOverlayCameraHolder
    {
        public UIHierarchy Hierarchy => _hierarchy;
        private UIHierarchy _hierarchy;
        
        public override void Init()
        {
            
        }

        public override void Dispose()
        {
           
        }

        internal void InvokeSceneLoaded(UIHierarchy hierarchy)
        {
            _hierarchy =  hierarchy;
        }

#region IOverlayCameraHolder

        public string Id => "UICamera";

        public Camera GetOverlayCamera =>  _hierarchy?.UICamera;

#endregion

    }
}

