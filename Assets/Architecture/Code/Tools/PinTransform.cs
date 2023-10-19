using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Architecture
{
    public enum ToggleState
    {
        Free, Pin, PinToZero
    }

    [ExecuteInEditMode, HideMonoScript]
    public sealed class PinTransform : SerializedMonoBehaviour
    {
        
#if UNITY_EDITOR
        [HideLabel, EnumToggleButtons] public ToggleState state;
        private bool _pinned;
        private Vector3 _position;

        private void Update()
        {
            var zero = state == ToggleState.PinToZero;
            var pin = zero || state == ToggleState.Pin;

            if (pin && !_pinned)
            {
                _position = zero ? Vector3.zero : transform.position;
                _pinned = true;
            }

            if (pin && _pinned)
            {
                transform.position = zero ? Vector3.zero : _position;
            }

            if (!pin && _pinned)
            {
                _pinned = false;
            }
        }

#endif
    }
}
