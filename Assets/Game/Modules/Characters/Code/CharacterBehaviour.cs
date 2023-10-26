using FlatLands.EntityControllable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Characters
{
    public sealed class CharacterBehaviour : SerializedMonoBehaviour, IEntityControllable
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _moveSpeed;

        [SerializeField] private CharacterController _characterController;

        private Transform _lookTarget;
        private Transform _cameraTarget;
        
        private float _vertical;
        private float _horizontal;

        private float _moveAmount;

        public void SetCharacterLookTarget(Transform target, Transform camera)
        {
            _lookTarget = target;
            _cameraTarget = camera;
        }

        private void MoveCharacter()
        {
            _vertical = Input.GetAxis("Vertical");
            _horizontal = Input.GetAxis("Horizontal");
            
            _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));

            if(_moveAmount <= 0)
                return;
            
            if(_horizontal > 0.5f)
                _characterController.Move(_cameraTarget.right * _horizontal * _moveSpeed * Time.deltaTime);
            
            if(_horizontal < -0.5f)
                _characterController.Move(-_cameraTarget.right * -_horizontal * _moveSpeed * Time.deltaTime);
            
            if(_vertical > 0.5f)
                _characterController.Move(_cameraTarget.forward * _vertical * _moveSpeed * Time.deltaTime);
            
            if(_vertical < -0.5f)
                _characterController.Move(-_cameraTarget.forward * -_vertical * _moveSpeed * Time.deltaTime);
        }

        private void RotateCharacter()
        {
            if(_lookTarget == null)
                return;
            
            var targetDir = _lookTarget.forward;
            targetDir.y = 0;
            var lookDir = Quaternion.LookRotation(targetDir);
            var targetRotate = Quaternion.Slerp(transform.rotation, lookDir, _rotationSpeed);
            transform.rotation = targetRotate;
        }

#region IEntityControllable

        public string Name => gameObject.name;

        public Transform EntityTransform => transform;

        public void EntityUpdate()
        {
            MoveCharacter();
            RotateCharacter();
        }

#endregion

    }
}

