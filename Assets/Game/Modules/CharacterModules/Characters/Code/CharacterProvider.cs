using FlatLands.Architecture;
using FlatLands.Cursors;
using FlatLands.EntityControllable;
using UnityEngine;

namespace FlatLands.Characters
{
    public sealed class CharacterProvider : IEntityControllableProvider
    {
        private const string Horizontal_Input_Name = "Horizontal";
        private const string Vertical_Input_Name = "Vertical";

        private int AnimatorInputHorizontal => Animator.StringToHash("InputHorizontal");
        private int AnimatorInputVertical => Animator.StringToHash("InputVertical");
        private int AnimatorInputMagnitude => Animator.StringToHash("InputMagnitude");
        private int AnimatorIsGrounded => Animator.StringToHash("IsGrounded");
        private int AnimatorIsStrafing => Animator.StringToHash("IsStrafing");
        private int AnimatorIsSprinting => Animator.StringToHash("IsSprinting");
        private int AnimatorGroundDistance => Animator.StringToHash("GroundDistance");

        private float AnimatorWalkSpeed => 0.5f;
        private float AnimatorRunningSpeed => 1f;
        private float AnimatorSprintSpeed => 1.5f;

        [Inject] private CursorManager _cursorManager;
        
        public CharacterConfig Config { get; }
        public CharacterBehaviour Behaviour { get; private set; }
        
        public bool IsJumping { get; private set; }
        public bool IsStrafing { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsSprinting { get; private set; }
        public bool StopMove { get; private set; }
        
        public bool IsActive { get; internal set; }

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

        
#region Main

        public CharacterProvider(CharacterConfig config)
        {
            Config = config;
        }

        public void SetBehaviour(CharacterBehaviour behaviour)
        {
            if(Behaviour != null)
            {
                Behaviour.OnAnimatorMoved -= HandleAnimatorMoved;
                Behaviour.OnAnimatorIks -= HandleAnimatorIk;
            }
            
            Behaviour = behaviour;
            
            if(Behaviour != null)
            {
                Behaviour.OnAnimatorMoved += HandleAnimatorMoved;
                Behaviour.OnAnimatorIks += HandleAnimatorIk;
            }
        }

        public void SetRotateTarget(Transform targetTransform)
        {
            _rotateTarget = targetTransform;
        }

        public void Init()
        {
            Behaviour.CharacterAnimator.updateMode = AnimatorUpdateMode.AnimatePhysics;

            CreatePhysicsMaterials();

            var collider = Behaviour.CharacterCollider;
            _colliderCenter = collider.center;
            _colliderHeight = collider.height;

            IsGrounded = true;
            IsActive = true;
        }
        
        public void EntityUpdate()
        {
            UpdateAnimator();
            UpdateInput();
        }

        public void EntityFixedUpdate()
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
            StrafeInput();
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

        private void StrafeInput()
        {
            if (Input.GetKeyDown(Config.StrafeInput))
                Strafe();
        }

        private void SprintInput()
        {
            if (Input.GetKeyDown(Config.SprintInput))
                Sprint(true);
            else if (Input.GetKeyUp(Config.SprintInput))
                Sprint(false);
        }
        
        private void JumpInput()
        {
            var canJump = IsGrounded && GroundAngle() < Config.SlopeLimit && !IsJumping && !StopMove;
            if (Input.GetKeyDown(Config.JumpInput) && canJump)
                Jump();
        }

#endregion


#region Movement

        private void ControlLocomotionType()
        {
            if (_lockMovement) 
                return;

            if (Config.LocomotionType.Equals(CharacterLocomotionType.FreeWithStrafe) && !IsStrafing 
                || Config.LocomotionType.Equals(CharacterLocomotionType.OnlyFree))
            {
                SetControllerMovementSpeed(Config.FreeMovementPair);
                SetAnimatorMoveSpeed(Config.FreeMovementPair);
            }
            else if (Config.LocomotionType.Equals(CharacterLocomotionType.OnlyStrafe) 
                     || Config.LocomotionType.Equals(CharacterLocomotionType.FreeWithStrafe) && IsStrafing)
            {
                IsStrafing = true;
                SetControllerMovementSpeed(Config.StrafeMovementPair);
                SetAnimatorMoveSpeed(Config.StrafeMovementPair);
            }

            if (!Config.UseRootMotion)
                MoveCharacter(_moveDirection);
        }
        
        private void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (_inputAxis.magnitude <= 0.01)
            {
                var movementPair = IsStrafing
                    ? Config.StrafeMovementPair.MovementSmooth
                    : Config.FreeMovementPair.MovementSmooth;
                
                _moveDirection = Vector3.Lerp(_moveDirection, Vector3.zero, movementPair * Time.deltaTime);
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
            var firstSpeed = IsSprinting 
                ? movementPair.RunningSpeed 
                : movementPair.WalkSpeed;
            
            var secondSpeed = IsSprinting 
                ? movementPair.SprintSpeed 
                : movementPair.RunningSpeed;
            
            _moveSpeed = movementPair.WalkByDefault 
                ? Mathf.Lerp(_moveSpeed, firstSpeed, movementPair.MovementSmooth * Time.deltaTime)
                : Mathf.Lerp(_moveSpeed, secondSpeed , movementPair.MovementSmooth * Time.deltaTime);
        }

        private void MoveCharacter(Vector3 _direction)
        {
            var smooth = IsStrafing 
                ? Config.StrafeMovementPair.MovementSmooth 
                : Config.FreeMovementPair.MovementSmooth;
            
            _inputSmooth = Vector3.Lerp(_inputSmooth, _inputAxis, smooth * Time.deltaTime);

            if (!IsGrounded || IsJumping) 
                return;

            _direction.y = 0;
            _direction.x = Mathf.Clamp(_direction.x, -1f, 1f);
            _direction.z = Mathf.Clamp(_direction.z, -1f, 1f);
            
            if (_direction.magnitude > 1f)
                _direction.Normalize();

            var curPos = Config.UseRootMotion
                ? Behaviour.CharacterAnimator.rootPosition
                : Behaviour.CharacterRigidbody.position;
            
            var targetPosition = curPos + _direction * (StopMove ? 0 : _moveSpeed) * Time.deltaTime;
            var targetVelocity = (targetPosition - Behaviour.transform.position) / Time.deltaTime;
            targetVelocity.y = Behaviour.CharacterRigidbody.velocity.y;
            
            Behaviour.CharacterRigidbody.velocity = targetVelocity;
        }

        private void CheckSlopeLimit()
        {
            if (_inputAxis.sqrMagnitude < 0.1) return;

            var transform = Behaviour.transform;
            var collider = Behaviour.CharacterCollider;
            
            if (Physics.Linecast(transform.position + Vector3.up * (collider.height * 0.5f), transform.position + _moveDirection.normalized * (collider.radius + 0.2f), out var hitInfo, Config.GroundLayer))
            {
                var hitAngle = Vector3.Angle(Vector3.up, hitInfo.normal);

                var targetPoint = hitInfo.point + _moveDirection.normalized * collider.radius;
                if ((hitAngle > Config.SlopeLimit) && Physics.Linecast(transform.position + Vector3.up * (collider.height * 0.5f), targetPoint, out hitInfo, Config.GroundLayer))
                {
                    hitAngle = Vector3.Angle(Vector3.up, hitInfo.normal);

                    if (hitAngle > Config.SlopeLimit && hitAngle < 85f)
                    {
                        StopMove = true;
                        return;
                    }
                }
            }
            
            StopMove = false;
        }
        
        private void Sprint(bool value)
        {
            var sprintConditions = _inputAxis.sqrMagnitude > 0.1f && IsGrounded &&
                                    !(IsStrafing && !Config.StrafeMovementPair.WalkByDefault && (_horizontalSpeed >= 0.5 || _horizontalSpeed <= -0.5 || _verticalSpeed <= 0.1f));

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

        private void Strafe()
        {
            IsStrafing = !IsStrafing;
        }
        
#endregion


#region Rotation
        
        private void ControlRotationType()
        {
            if (_lockRotation) 
                return;

            var rotationWithCamera = IsStrafing
                ? Config.StrafeMovementPair.RotateWithCamera
                : Config.FreeMovementPair.RotateWithCamera;
            
            var validInput = _inputAxis != Vector3.zero || rotationWithCamera;

            if (!validInput)
                return;

            var curSmooth = IsStrafing
                ? Config.StrafeMovementPair.MovementSmooth
                : Config.FreeMovementPair.MovementSmooth;
            
            _inputSmooth = Vector3.Lerp(_inputSmooth, _inputAxis, curSmooth * Time.deltaTime);
            
            
            var dir = (IsStrafing && (!IsSprinting || Config.SprintOnlyFree == false) || (Config.FreeMovementPair.RotateWithCamera && _inputAxis == Vector3.zero)) 
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
            var movementPair = IsStrafing 
                ? Config.StrafeMovementPair.RotationSpeed 
                : Config.FreeMovementPair.RotationSpeed;
            
            RotateToDirection(direction, movementPair);
        }

        private void RotateToDirection(Vector3 direction, float rotationSpeed)
        {
            if (!Config.JumpAndRotate && !IsGrounded) 
                return;
            
            direction.y = 0f;
            var desiredForward = Vector3.RotateTowards( Behaviour.transform.forward, direction.normalized, rotationSpeed * Time.deltaTime, .1f);
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

            if (Config.UseRootMotion)
                MoveCharacter(_moveDirection);
        }
        
        private void UpdateAnimator()
        {
            var animator = Behaviour.CharacterAnimator;
            
            animator.SetBool(AnimatorIsStrafing, IsStrafing);
            animator.SetBool(AnimatorIsSprinting, IsSprinting);
            animator.SetBool(AnimatorIsGrounded, IsGrounded);
            animator.SetFloat(AnimatorGroundDistance, _groundDistance);

            var verticalSpeed = StopMove ? 0 : _verticalSpeed;
            var strafeSmooth = Config.StrafeMovementPair.AnimationSmooth;
            var freeSmooth = Config.FreeMovementPair.AnimationSmooth;

            if (IsStrafing)
            {
                var horizontalSpeed = StopMove ? 0 : _horizontalSpeed;
                animator.SetFloat(AnimatorInputHorizontal, horizontalSpeed, strafeSmooth, Time.deltaTime);
                animator.SetFloat(AnimatorInputVertical, verticalSpeed, strafeSmooth, Time.deltaTime);
            }
            else
            {
                animator.SetFloat(AnimatorInputVertical, verticalSpeed, freeSmooth, Time.deltaTime);
            }

            var magnitude = StopMove ? 0f : _inputMagnitude;
            var curSmooth = IsStrafing ? strafeSmooth : freeSmooth;
            
            animator.SetFloat(AnimatorInputMagnitude, magnitude, curSmooth, Time.deltaTime);
        }

        private void SetAnimatorMoveSpeed(CharacterMovementPair movementPair)
        {
            var relativeInput = Behaviour.transform.InverseTransformDirection(_moveDirection);
            _verticalSpeed = relativeInput.z;
            _horizontalSpeed = relativeInput.x;

            var newInput = new Vector2(_verticalSpeed, _horizontalSpeed);
            var walkAnimSpeed = IsSprinting 
                ? AnimatorRunningSpeed 
                : AnimatorWalkSpeed;

            var curMagnitude = IsSprinting 
                ? newInput.magnitude + 0.5f 
                : newInput.magnitude;

            var runAnimSpeed = IsSprinting 
                ? AnimatorSprintSpeed 
                : AnimatorRunningSpeed;

            _inputMagnitude = movementPair.WalkByDefault 
                ? Mathf.Clamp(newInput.magnitude, 0, walkAnimSpeed) 
                : Mathf.Clamp(curMagnitude, 0, runAnimSpeed);
        }

        private void HandleAnimatorIk(int layer)
        {
            if(_rotateTarget == null)
                return;
            
            Behaviour.CharacterAnimator.SetLookAtWeight(Config.MainWeight, Config.BodyIkWeight, Config.HeadIkWeight, Config.EyesIkWeight, Config.ClampIkWeight);
            Behaviour.CharacterAnimator.SetLookAtPosition(_rotateTarget.position);
        }

#endregion


#region Jump

        private void Jump()
        {
            _jumpCounter = Config.JumpTimer;
            IsJumping = true;

            if (_inputAxis.sqrMagnitude < 0.1f)
                Behaviour.CharacterAnimator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                Behaviour.CharacterAnimator.CrossFadeInFixedTime("JumpMove", .2f);
        }

        private void ControlJumpBehaviour()
        {
            if (!IsJumping) 
                return;

            _jumpCounter -= Time.deltaTime;
            if (_jumpCounter <= 0)
            {
                _jumpCounter = 0;
                IsJumping = false;
            }

            var rigidbody = Behaviour.CharacterRigidbody;
            var vel = rigidbody.velocity;
            vel.y = Config.JumpHeight;
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
            
            _inputSmooth = Vector3.Lerp(_inputSmooth, _inputAxis, Config.AirSmooth * Time.deltaTime);

            if (Config.JumpWithRigidbodyForce && !IsGrounded)
            {
                rigidbody.AddForce(_moveDirection * Config.AirSpeed * Time.deltaTime, ForceMode.VelocityChange);
                return;
            }

            _moveDirection.y = 0;
            _moveDirection.x = Mathf.Clamp(_moveDirection.x, -1f, 1f);
            _moveDirection.z = Mathf.Clamp(_moveDirection.z, -1f, 1f);

            var targetPosition = rigidbody.position + (_moveDirection * Config.AirSpeed) * Time.deltaTime;
            var targetVelocity = (targetPosition - transform.position) / Time.deltaTime;

            targetVelocity.y = rigidbody.velocity.y;
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, targetVelocity, Config.AirSmooth * Time.deltaTime);
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
            
            if (_groundDistance <= Config.GroundMinDistance)
            {
                IsGrounded = true;
                if (!IsJumping && _groundDistance > 0.05f)
                    rigidbody.AddForce(transform.up * (Config.ExtraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);

                _heightReached = transform.position.y;
            }
            else
            {
                if (_groundDistance >= Config.GroundMaxDistance)
                {
                    IsGrounded = false;
                    _verticalVelocity = rigidbody.velocity.y;
                    if (!IsJumping)
                    {
                        rigidbody.AddForce(transform.up * Config.ExtraGravity * Time.deltaTime, ForceMode.VelocityChange);
                    }
                }
                else if (!IsJumping)
                {
                    rigidbody.AddForce(transform.up * (Config.ExtraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
                }
            }
         }

         private void ControlMaterialPhysics()
         {
            var collider = Behaviour.CharacterCollider;
            
            collider.material = (IsGrounded && GroundAngle() <= Config.SlopeLimit + 1) 
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
            if (Physics.Raycast(ray2, out _groundHit, (_colliderHeight * 0.5f) + dist, Config.GroundLayer) && !_groundHit.collider.isTrigger)
                dist = transform.position.y - _groundHit.point.y;
            
            if (dist >= Config.GroundMinDistance)
            {
                var pos = transform.position + Vector3.up * (collider.radius);
                var ray = new Ray(pos, -Vector3.up);
                if (Physics.SphereCast(ray, radius, out _groundHit, collider.radius + Config.GroundMaxDistance, Config.GroundLayer) && !_groundHit.collider.isTrigger)
                {
                    Physics.Linecast(_groundHit.point + (Vector3.up * 0.1f), _groundHit.point + Vector3.down * 0.15f, out _groundHit, Config.GroundLayer);
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
             
            var dir = IsStrafing && _inputAxis.magnitude > 0 ? (transform.right * _inputAxis.x + transform.forward * _inputAxis.z).normalized : transform.forward;
            var movementAngle = Vector3.Angle(dir, _groundHit.normal) - 90;
            return movementAngle;
         }
        
#endregion

    }
}