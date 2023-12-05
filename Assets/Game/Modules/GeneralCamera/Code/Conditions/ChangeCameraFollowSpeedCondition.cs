using FlatLands.Architecture;
using FlatLands.Conditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.GeneralCamera
{
    public sealed class ChangeCameraFollowSpeedCondition : BaseCondition
    {
        [Inject] private GeneralCameraManager _generalCameraManager;
        
        [SerializeField] 
        private ChangeCameraFollowSpeedType _changeType;

        [SerializeField, ShowIf("@_changeType == ChangeCameraFollowSpeedType.Custom")] 
        private float _newCameraFollowSpeed;
        
        public override bool CanApply => true;
        public override void ApplyCondition()
        {
            if (_changeType == ChangeCameraFollowSpeedType.Default)
            {
                _generalCameraManager.SetDefaultFollowSpeed();
            }
            else
            {
                _generalCameraManager.SetCustomFollowSpeed(_newCameraFollowSpeed);
            }
        }
    }

    internal enum ChangeCameraFollowSpeedType
    {
        Default = 0,
        Custom = 10
    }
}