using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.GeneralCamera
{
    [CreateAssetMenu(
        menuName = "FlatLands/Cameras/" + nameof(GeneralCameraConfig), 
        fileName = nameof(GeneralCameraConfig))]
    public sealed class GeneralCameraConfig : SingletonScriptableObject<GeneralCameraConfig>
    {
        [SerializeField] 
        private CameraHierarchy _cameraPrefab;

        public CameraHierarchy CameraPrefab => _cameraPrefab;
    }
}