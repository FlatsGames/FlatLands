using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Architecture
{
    public abstract class InjectedSerializedMonoBehaviour : SerializedMonoBehaviour
    {
        [Inject] protected Container _container { get; private set; }
        
        public bool Inited { get; private set; }
        
        private void Awake()
        {
            GlobalContainer.InjectAt(this);
            _container = GlobalContainer.GetContainer();
            Init();
        }
        
        protected abstract void Init();

        protected void CheckInit()
        {
            if(!Inited)
                Awake();
        }

        private void OnDestroy()
        {
            Destroy();
        }
        
        public abstract void Destroy();
    }
}