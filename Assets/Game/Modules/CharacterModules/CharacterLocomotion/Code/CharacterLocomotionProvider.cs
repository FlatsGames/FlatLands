using System;
using FlatLands.Architecture;
using FlatLands.CharacterAttributes;
using FlatLands.GameAttributes;
using FlatLands.Characters;
using UnityEngine;

namespace FlatLands.CharacterLocomotion
{
    public sealed class CharacterLocomotionProvider : ICharacterProvider
    {
        private const string Horizontal_Input_Name = "Horizontal";
        private const string Vertical_Input_Name = "Vertical";
        
        private const string Jumping_SubMachine_Name = "Jumping";
        private const string Jumping_Start_Simple = "JumpingStart_Simple";
        private const string Jumping_Start_Run = "JumpingStart_Run";

        [Inject] private GameAttributesManager _gameAttributesManager;

        private int AnimatorInputHorizontal => Animator.StringToHash("InputHorizontal");
        private int AnimatorInputVertical => Animator.StringToHash("InputVertical");
        private int AnimatorInputMagnitude => Animator.StringToHash("InputMagnitude");
        private int AnimatorMovementType => Animator.StringToHash("MovementType");
        private int AnimatorIsGrounded => Animator.StringToHash("IsGrounded");
        private int AnimatorIsSprinting => Animator.StringToHash("IsSprinting");
        private int AnimatorGroundDistance => Animator.StringToHash("GroundDistance");

        private int LocomotionLayerIndex => 0;
        private string LocomotionLayerName => Behaviour.CharacterAnimator.GetLayerName(LocomotionLayerIndex);

        private float AnimatorSimpleThreshold => 1f;
        private float AnimatorSprintThreshold => 1.5f;
        private float DeltaTime => UnityEventsProvider.DeltaTime;
        
        public CharacterLocomotionConfig LocomotionConfig { get; private set; }
        public CharacterBehaviour Behaviour { get; private set; }

        public CharacterLocomotionType LocomotionType
        {
            get => _locomotionType;
            set
            {
                _locomotionType = value;
                _prevMovementPair = _currentMovementPair;
                LocomotionConfig.MovementPairs.TryGetValue(_locomotionType, out _currentMovementPair);
                
                OnLocomotionTypeChanged?.Invoke();
            }
        }

        public bool IsJumping { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsSprinting { get; private set; }
        public bool CanSprinting { get; private set; }
        public bool StopMove { get; private set; }
        
        public bool IsActive { get; set; }

        public event Action OnLocomotionTypeChanged;

        private CharacterLocomotionType _locomotionType;
        private CharacterMovementPair _prevMovementPair;
        private CharacterMovementPair _currentMovementPair;
        
        private PhysicMaterial _frictionPhysics;
        private PhysicMaterial _maxFrictionPhysics;
        private PhysicMaterial _slippyPhysics;
        
        private float _inputMagnitude;
        private float _verticalSpeed;
        private float _horizontalSpeed;   
        private float _moveSpeed;
        private float _verticalVelocity;
        private float _colliderHeight;
        private float _heightReached;
        private float _jumpCounter;
        private float _groundDistance;
        
        private RaycastHit _groundHit;
        
        private bool _lockMovement = false;
        private bool _lockRotation = false;   
        
        private Transform _rotateTarget;
        
        private Vector3 _inputAxis;
        private Vector3 _colliderCenter;        
        private Vector3 _inputSmooth;       
        private Vector3 _moveDirection;

        private AttributeRegeneratedData _staminaAttribute;
        
#region Main

        public void SetBehaviour(CharacterBehaviour behaviour)
        {
            if(Behaviour != null)
            {
                Behaviour.OnAnimatorMoved -= HandleAnimatorMoved;
            }
            
            Behaviour = behaviour;
            
            if(Behaviour != null)
            {
                Behaviour.OnAnimatorMoved += HandleAnimatorMoved;
            }
        }

        public void SetRotateTarget(Transform targetTransform)
        {
            _rotateTarget = targetTransform;
        }

        public void Init()
        {
            LocomotionConfig = CharacterLocomotionConfig.Instance;
            Behaviour.CharacterAnimator.updateMode = AnimatorUpdateMode.AnimatePhysics;
            LocomotionType = LocomotionConfig.DefaultLocomotionType;

            CreatePhysicsMaterials();

            var collider = Behaviour.CharacterCollider;
            _colliderCenter = collider.center;
            _colliderHeight = collider.height;

            _staminaAttribute = _gameAttributesManager
                .GetAttributeHolder(CharacterAttributesProvider.CharacterHolderId)
                .GetAttribute<AttributeRegeneratedData>(GameAttributeType.Stamina);
            
            IsGrounded = true;
            IsActive = true;
        }
        
        public void Dispose()
        {
            
        }

        public void HandleUpdate()
        {
            UpdateAnimator();
            UpdateInput();
        }

        public void HandleFixedUpdate()
        {
            CheckGround();
            CheckSlopeLimit();
            ControlJumpBehaviour();
            AirControl();
            
            ControlLocomotionType();
            ControlRotationType();
        }

        private void HandleAnimatorMoved()
        {
            ControlAnimatorRootMotion();
        }

#endregion


#region Input

        private void UpdateInput()
        {
            MoveInput();
            UpdateMoveDirection(_rotateTarget);
            SprintInput();
            JumpInput();
        }
        
        private void MoveInput()
        {
            if(!IsActive)
            {
                _inputAxis = new Vector3(0, 0, 0);
                return;
            }
            
            _inputAxis.x = Input.GetAxis(Horizontal_Input_Name);
            _inputAxis.z = Input.GetAxis(Vertical_Input_Name);
        }

        private void SprintInput()
        {
            UpdateSprint();
            
            if(Input.GetKey(LocomotionConfig.SprintInput))
                ApplySprintingCost();
            
            if (Input.GetKeyDown(LocomotionConfig.SprintInput))
                Sprint(true);
            else if (Input.GetKeyUp(LocomotionConfig.SprintInput))
                Sprint(false);
        }
        
        private void JumpInput()
        {
            var canJump = IsGrounded && GroundAngle() < LocomotionConfig.SlopeLimit && !IsJumping && !StopMove;
            if (Input.GetKeyDown(LocomotionConfig.JumpInput) && canJump)
                Jump();
        }

#endregion


#region Movement

        private void ControlLocomotionType()
        {
            if (_lockMovement) 
                return;
            
            SetControllerMovementSpeed(_currentMovementPair);
            UpdateAnimatorMoveSpeed();
            
            if (!LocomotionConfig.UseRootMotion)
                MoveCharacter(_moveDirection);
        }
        
        private void UpdateMoveDirection(Transform referenceTransform = null)
        {
            Behaviour.LookAiming.position = _rotateTarget.position;
            
            if (_inputAxis.magnitude <= 0.01)
            {
                var movementSmooth = _currentMovementPair.MovementSmooth;
                
                _moveDirection = Vector3.Lerp(_moveDirection, Vector3.zero, movementSmooth * DeltaTime);
                return;
            }

            if (!referenceTransform) 
                return;
            
            var right = referenceTransform.right;
            right.y = 0;
                
            var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
            _moveDirection = (_inputSmooth.x * right) + (_inputSmooth.z * forward);
        }

        private void SetControllerMovementSpeed(CharacterMovementPair movementPair)
        {
            var secondSpeed = IsSprinting 
                ? movementPair.SprintSpeed 
                : movementPair.MainSpeed;
            
            _moveSpeed = Mathf.Lerp(_moveSpeed, secondSpeed , movementPair.MovementSmooth * DeltaTime);
        }

        private void MoveCharacter(Vector3 _direction)
        {
            var movementSmooth = _currentMovementPair.MovementSmooth;
            
            _inputSmooth = Vector3.Lerp(_inputSmooth, _inputAxis, movementSmooth * DeltaTime);

            if (!IsGrounded || IsJumping) 
                return;

            _direction.y = 0;
            _direction.x = Mathf.Clamp(_direction.x, -1f, 1f);
            _direction.z = Mathf.Clamp(_direction.z, -1f, 1f);
            
            if (_direction.magnitude > 1f)
                _direction.Normalize();

            var curPos = LocomotionConfig.UseRootMotion
                ? Behaviour.CharacterAnimator.rootPosition
                : Behaviour.CharacterRigidbody.position;
            
            var targetPosition = curPos + _direction * (StopMove ? 0 : _moveSpeed) * DeltaTime;
            var targetVelocity = (targetPosition - Behaviour.transform.position) / DeltaTime;
            targetVelocity.y = Behaviour.CharacterRigidbody.velocity.y;
            
            Behaviour.CharacterRigidbody.velocity = targetVelocity;
        }

        private void CheckSlopeLimit()
        {
            if (_inputAxis.sqrMagnitude < 0.1) return;

            var transform = Behaviour.transform;
            var collider = Behaviour.CharacterCollider;
            
            if (Physics.Linecast(transform.position + Vector3.up * (collider.height * 0.5f), transform.position + _moveDirection.normalized * (collider.radius + 0.2f), out var hitInfo, LocomotionConfig.GroundLayer))
            {
                var hitAngle = Vector3.Angle(Vector3.up, hitInfo.normal);

                var targetPoint = hitInfo.point + _moveDirection.normalized * collider.radius;
                if ((hitAngle > LocomotionConfig.SlopeLimit) && Physics.Linecast(transform.position + Vector3.up * (collider.height * 0.5f), targetPoint, out hitInfo, LocomotionConfig.GroundLayer))
                {
                    hitAngle = Vector3.Angle(Vector3.up, hitInfo.normal);

                    if (hitAngle > LocomotionConfig.SlopeLimit && hitAngle < 85f)
                    {
                        StopMove = true;
                        return;
                    }
                }
            }
            
            StopMove = false;
        }

        private void ApplySprintingCost()
        {
            if(!CanSprinting || !IsSprinting)
                return;
            
            _staminaAttribute?.RemoveValue(LocomotionConfig.SprintCost);
        }

        private void UpdateSprint()
        {
            if(_staminaAttribute == null)
                return;
            
            CanSprinting = _staminaAttribute.CanRemoveValue(LocomotionConfig.SprintCost);
            _staminaAttribute.SetCanRegenerated(!IsSprinting);
            
            if (!CanSprinting)
                Sprint(false);
        }
        
        private void Sprint(bool value)
        {
            var sprintConditions = _inputAxis.sqrMagnitude > 0.1f && IsGrounded && _currentMovementPair.CanSprinting;
            if (value && sprintConditions)
            {
                if (_inputAxis.sqrMagnitude > 0.1f && !IsSprinting)
                {
                    IsSprinting = true;
                }
            }
            else if (IsSprinting)
            {
                IsSprinting = false;
            }
        }

#endregion


#region Rotation
        
        private void ControlRotationType()
        {
            if (_lockRotation) 
                return;

            var rotationWithCamera = _currentMovementPair.RotateWithCamera;
            var validInput = _inputAxis != Vector3.zero || rotationWithCamera;

            if (!validInput)
                return;

            var movementSmooth = _currentMovementPair.MovementSmooth;
            _inputSmooth = Vector3.Lerp(_inputSmooth, _inputAxis, movementSmooth * DeltaTime);
            
            var dir = (!IsSprinting || (rotationWithCamera && _inputAxis == Vector3.zero)) 
                      && _rotateTarget ? _rotateTarget.forward : _moveDirection;
            RotateToDirection(dir);
        }

        private void RotateToPosition(Vector3 position)
        {
            var desiredDirection = position - Behaviour.transform.position;
            RotateToDirection(desiredDirection.normalized);
        }

        private void RotateToDirection(Vector3 direction)
        {
            var rotationSpeed = _currentMovementPair.RotationSpeed;
            RotateToDirection(direction, rotationSpeed);
        }

        private void RotateToDirection(Vector3 direction, float rotationSpeed)
        {
            if (!LocomotionConfig.JumpAndRotate && !IsGrounded) 
                return;
            
            direction.y = 0f;
            var desiredForward = Vector3.RotateTowards( Behaviour.transform.forward, direction.normalized, rotationSpeed * DeltaTime, .1f);
            var rotation = Quaternion.LookRotation(desiredForward);
            
            Behaviour.transform.rotation = rotation;
        }

#endregion


#region Animator

        private void ControlAnimatorRootMotion()
        {
            if (_inputSmooth == Vector3.zero)
            {
                var transform = Behaviour.transform;
                transform.position = Behaviour.CharacterAnimator.rootPosition;
                transform.rotation = Behaviour.CharacterAnimator.rootRotation;
            }

            if (LocomotionConfig.UseRootMotion)
                MoveCharacter(_moveDirection);
        }
        
        private void UpdateAnimator()
        {
            var animator = Behaviour.CharacterAnimator;
            
            animator.SetBool(AnimatorIsSprinting, IsSprinting);
            animator.SetBool(AnimatorIsGrounded, IsGrounded);
            animator.SetFloat(AnimatorGroundDistance, _groundDistance);
            animator.SetInteger(AnimatorMovementType, (int)LocomotionType);

            var verticalSpeed = StopMove ? 0 : _verticalSpeed;
            var horizontalSpeed = StopMove ? 0 : _horizontalSpeed;

            var animationSmooth = _currentMovementPair.AnimationSmooth;
            
            animator.SetFloat(AnimatorInputVertical, verticalSpeed, animationSmooth, DeltaTime);
            animator.SetFloat(AnimatorInputHorizontal, horizontalSpeed, animationSmooth, DeltaTime);

            var magnitude = StopMove ? 0f : _inputMagnitude;
            
            animator.SetFloat(AnimatorInputMagnitude, magnitude, animationSmooth, DeltaTime);
        }

        private void UpdateAnimatorMoveSpeed()
        {
            var relativeInput = Behaviour.transform.InverseTransformDirection(_moveDirection);
            _verticalSpeed = Mathf.Clamp(relativeInput.z, -1, 1);
            _horizontalSpeed = Mathf.Clamp(relativeInput.x, -1, 1);

            var newInput = new Vector2(_verticalSpeed, _horizontalSpeed);
            var curThreshold =  IsSprinting 
                ? newInput.magnitude + 0.5f 
                : newInput.magnitude;

            var maxThreshold = IsSprinting 
                ? AnimatorSprintThreshold 
                : AnimatorSimpleThreshold;

            _inputMagnitude = Mathf.Clamp(curThreshold, 0, maxThreshold);
        }

#endregion


#region Jump

        private void Jump()
        {
            _jumpCounter = LocomotionConfig.JumpTimer;
            IsJumping = true;

            if (_inputAxis.sqrMagnitude < 0.1f)
                Behaviour.CharacterAnimator.PlayCrossFadeInFixedTime(LocomotionLayerIndex,Jumping_Start_Simple, 0.1f);
            else
                Behaviour.CharacterAnimator.PlayCrossFadeInFixedTime(LocomotionLayerIndex,Jumping_Start_Run, 0.2f);
        }

        private void ControlJumpBehaviour()
        {
            if (!IsJumping) 
                return;

            _jumpCounter -= DeltaTime;
            if (_jumpCounter <= 0)
            {
                _jumpCounter = 0;
                IsJumping = false;
            }

            var rigidbody = Behaviour.CharacterRigidbody;
            var vel = rigidbody.velocity;
            vel.y = LocomotionConfig.JumpHeight;
            rigidbody.velocity = vel;
        }

        private void AirControl()
        {
            if (IsGrounded && !IsJumping) 
                return;
            
            var transform = Behaviour.transform;
            var rigidbody = Behaviour.CharacterRigidbody;
            
            if (transform.position.y > _heightReached) 
                _heightReached = transform.position.y;
            
            _inputSmooth = Vector3.Lerp(_inputSmooth, _inputAxis, LocomotionConfig.AirSmooth * DeltaTime);

            if (LocomotionConfig.JumpWithRigidbodyForce && !IsGrounded)
            {
                rigidbody.AddForce(_moveDirection * LocomotionConfig.AirSpeed * DeltaTime, ForceMode.VelocityChange);
                return;
            }

            _moveDirection.y = 0;
            _moveDirection.x = Mathf.Clamp(_moveDirection.x, -1f, 1f);
            _moveDirection.z = Mathf.Clamp(_moveDirection.z, -1f, 1f);

            var targetPosition = rigidbody.position + (_moveDirection * LocomotionConfig.AirSpeed) * DeltaTime;
            var targetVelocity = (targetPosition - transform.position) / DeltaTime;

            targetVelocity.y = rigidbody.velocity.y;
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, targetVelocity, LocomotionConfig.AirSmooth * DeltaTime);
        }
        
#endregion


#region Physics Materials

        private void CreatePhysicsMaterials()
        {
            // slides the character through walls and edges
            _frictionPhysics = new PhysicMaterial();
            _frictionPhysics.name = "FrictionPhysics";
            _frictionPhysics.staticFriction = .25f;
            _frictionPhysics.dynamicFriction = .25f;
            _frictionPhysics.frictionCombine = PhysicMaterialCombine.Multiply;

            // prevents the collider from slipping on ramps
            _maxFrictionPhysics = new PhysicMaterial();
            _maxFrictionPhysics.name = "MaxFrictionPhysics";
            _maxFrictionPhysics.staticFriction = 1f;
            _maxFrictionPhysics.dynamicFriction = 1f;
            _maxFrictionPhysics.frictionCombine = PhysicMaterialCombine.Maximum;

            // air physics 
            _slippyPhysics = new PhysicMaterial();
            _slippyPhysics.name = "SlippyPhysics";
            _slippyPhysics.staticFriction = 0f;
            _slippyPhysics.dynamicFriction = 0f;
            _slippyPhysics.frictionCombine = PhysicMaterialCombine.Minimum;
        }
        
#endregion


#region Ground                

         private void CheckGround()
         {
            CheckGroundDistance();
            ControlMaterialPhysics();

            var rigidbody = Behaviour.CharacterRigidbody;
            var transform = Behaviour.transform;
            
            if (_groundDistance <= LocomotionConfig.GroundMinDistance)
            {
                IsGrounded = true;
                if (!IsJumping && _groundDistance > 0.05f)
                    rigidbody.AddForce(transform.up * (LocomotionConfig.ExtraGravity * 2 * DeltaTime), ForceMode.VelocityChange);

                _heightReached = transform.position.y;
            }
            else
            {
                if (_groundDistance >= LocomotionConfig.GroundMaxDistance)
                {
                    IsGrounded = false;
                    _verticalVelocity = rigidbody.velocity.y;
                    if (!IsJumping)
                    {
                        rigidbody.AddForce(transform.up * LocomotionConfig.ExtraGravity * DeltaTime, ForceMode.VelocityChange);
                    }
                }
                else if (!IsJumping)
                {
                    rigidbody.AddForce(transform.up * (LocomotionConfig.ExtraGravity * 2 * DeltaTime), ForceMode.VelocityChange);
                }
            }
         }

         private void ControlMaterialPhysics()
         {
            var collider = Behaviour.CharacterCollider;
            
            collider.material = (IsGrounded && GroundAngle() <= LocomotionConfig.SlopeLimit + 1) 
                ? _frictionPhysics 
                : _slippyPhysics;

            if (IsGrounded && _inputAxis == Vector3.zero)
                collider.material = _maxFrictionPhysics;
            else if (IsGrounded && _inputAxis != Vector3.zero)
                collider.material = _frictionPhysics;
            else
                collider.material = _slippyPhysics;
         }

         private void CheckGroundDistance()
         {
            var collider = Behaviour.CharacterCollider;
            var transform = Behaviour.transform;

            var radius = collider.radius * 0.9f;
            var dist = 10f;
            
            var ray2 = new Ray(transform.position + new Vector3(0, _colliderHeight * 0.5f, 0), Vector3.down);
            if (Physics.Raycast(ray2, out _groundHit, (_colliderHeight * 0.5f) + dist, LocomotionConfig.GroundLayer) && !_groundHit.collider.isTrigger)
                dist = transform.position.y - _groundHit.point.y;
            
            if (dist >= LocomotionConfig.GroundMinDistance)
            {
                var pos = transform.position + Vector3.up * (collider.radius);
                var ray = new Ray(pos, -Vector3.up);
                if (Physics.SphereCast(ray, radius, out _groundHit, collider.radius + LocomotionConfig.GroundMaxDistance, LocomotionConfig.GroundLayer) && !_groundHit.collider.isTrigger)
                {
                    Physics.Linecast(_groundHit.point + (Vector3.up * 0.1f), _groundHit.point + Vector3.down * 0.15f, out _groundHit, LocomotionConfig.GroundLayer);
                    var newDist = transform.position.y - _groundHit.point.y;
                    if (dist > newDist) dist = newDist;
                }
            }
            _groundDistance = (float)System.Math.Round(dist, 2);
            
         }

         private float GroundAngle()
         {
            var groundAngle = Vector3.Angle(_groundHit.normal, Vector3.up);
            return groundAngle;
         }

         private float GroundAngleFromDirection()
         {
            var transform = Behaviour.transform;
             
            var dir =  _inputAxis.magnitude > 0 ? (transform.right * _inputAxis.x + transform.forward * _inputAxis.z).normalized : transform.forward;
            var movementAngle = Vector3.Angle(dir, _groundHit.normal) - 90;
            return movementAngle;
         }
        
#endregion

    }
}