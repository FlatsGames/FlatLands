using FlatLands.Architecture;

namespace FlatLands.GeneralCamera
{
    public sealed class GeneralCameraManager : SharedObject
    {
        public CameraHierarchy Hierarchy { get; private set; }

        public override void Init()
        {
            
        }

        internal void InvokeCameraCreated(CameraHierarchy hierarchy)
        {
            Hierarchy = hierarchy;
        }
    }
}