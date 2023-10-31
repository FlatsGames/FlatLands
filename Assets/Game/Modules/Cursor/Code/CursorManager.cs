using System;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Cursors
{
    public sealed class CursorManager : SharedObject
    {
        public bool CursorActive { get; private set; }

        public event Action OnCursorStateChanged;
        
        private CursorConfig _config;

        public override void Init()
        {
            _config = CursorConfig.Instance;
            UnityEventsProvider.OnUpdate += OnUpdate;
        }

        public override void Dispose()
        {
            UnityEventsProvider.OnUpdate -= OnUpdate;
        }

        private void OnUpdate()
        {
            if (!Input.GetKeyDown(_config.CursorButton)) 
                return;
            
            if (CursorActive)
                HideCursor();
            else
                ShowCursor();
        }
        
        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CursorActive = true;
            
            OnCursorStateChanged?.Invoke();
        }

        public void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CursorActive = false;
            
            OnCursorStateChanged?.Invoke();
        }
    }
}