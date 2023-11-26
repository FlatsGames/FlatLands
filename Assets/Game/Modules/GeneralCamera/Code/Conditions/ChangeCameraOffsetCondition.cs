using FlatLands.Architecture;
using FlatLands.Conditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.GeneralCamera
{
    public sealed class ChangeCameraOffsetCondition : BaseCondition
    {
        [Inject] private GeneralCameraManager _generalCameraManager;
        
        [SerializeField] 
        private ChangeCameraOffsetType _offsetType;
        
        [SerializeField, ShowIf("@_offsetType == ChangeCameraOffsetType.CustomXOffset")] 
        private float _newCameraXOffset;
        
        [SerializeField, ShowIf("@_offsetType == ChangeCameraOffsetType.CustomXOffset")] 
        private float _newCameraXOffsetDuration;
        
        public override bool CanApply => true;

        public override void ApplyCondition()
        {
            if (_offsetType == ChangeCameraOffsetType.DefaultXOffset)
            {
                _generalCameraManager.SetDefaultXOffset();
            }
            else
            {
                _generalCameraManager.SetCustomXOffset(_newCameraXOffset, _newCameraXOffsetDuration);
            }
        }
    }

    internal enum ChangeCameraOffsetType
    {
        DefaultXOffset = 0,
        CustomXOffset = 10,
    }
}