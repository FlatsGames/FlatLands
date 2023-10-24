using UnityEngine;

namespace FlatLands.Architecture
{
    public interface IOverlayCameraHolder
    {
        public string Id { get; }
        public Camera GetOverlayCamera { get; }
    }
}

