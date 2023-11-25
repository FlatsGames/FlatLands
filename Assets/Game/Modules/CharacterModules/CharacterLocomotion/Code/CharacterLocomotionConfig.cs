using FlatLands.Architecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.CharacterLocomotion
{
    [CreateAssetMenu(
        menuName = "FlatLands/Characters/" + nameof(CharacterLocomotionConfig), 
        fileName = nameof(CharacterLocomotionConfig))]
    public sealed class CharacterLocomotionConfig : SingletonScriptableObject<CharacterLocomotionConfig>
    {
        [SerializeField, BoxGroup("Main Settings")]
        private bool _useRootMotion = false;

        [SerializeField, FoldoutGroup("Buttons Settings")]
        private KeyCode _jumpInput = KeyCode.Space;
        
        [SerializeField, FoldoutGroup("Buttons Settings")]
        private KeyCode _strafeInput = KeyCode.Tab;
        
        [SerializeField, FoldoutGroup("Buttons Settings")]
        private KeyCode _sprintInput = KeyCode.LeftShift;

        [SerializeField, FoldoutGroup("Movement Settings")]
        private float _sprintCost;
        
        [SerializeField, FoldoutGroup("Movement Settings")]
        private CharacterLocomotionType _locomotionType = CharacterLocomotionType.FreeWithStrafe;

        [SerializeField, FoldoutGroup("Movement Settings"), HideIf("@_locomotionType == CharacterLocomotionType.OnlyStrafe")]
        private CharacterMovementPair _freeMovementPair = new CharacterMovementPair();
        
        [SerializeField, FoldoutGroup("Movement Settings"), HideIf("@_locomotionType == CharacterLocomotionType.OnlyFree")]
        private CharacterMovementPair _strafeMovementPair = new CharacterMovementPair();
        
        
        [SerializeField, FoldoutGroup("Jump Settings")]
        private bool _jumpWithRigidbodyForce = false;
        
        [SerializeField, FoldoutGroup("Jump Settings")]
        private bool _jumpAndRotate = true;
        
        [SerializeField, FoldoutGroup("Jump Settings")]
        private float _jumpTimer = 0.3f;
        
        [SerializeField, FoldoutGroup("Jump Settings")]
        private float _jumpHeight = 4f;

        [SerializeField, FoldoutGroup("Jump Settings")]
        private float _airSpeed = 5f;
        
        [SerializeField, FoldoutGroup("Jump Settings")]
        private float _airSmooth = 6f;
        
        [SerializeField, FoldoutGroup("Jump Settings")]
        private float _extraGravity = -10f;


        [SerializeField, FoldoutGroup("Ground Settings")]
        private LayerMask _groundLayer = 1 << 0;
        
        [SerializeField, FoldoutGroup("Ground Settings")]
        private float _groundMinDistance = 0.25f;
        
        [SerializeField, FoldoutGroup("Ground Settings")]
        private float _groundMaxDistance = 0.5f;
        
        [SerializeField, FoldoutGroup("Ground Settings"), Range(30, 80)]
        private float _slopeLimit = 75f;

        
        [SerializeField, FoldoutGroup("IK Settings"), Range(0, 1)]
        private float _mainWeight = 1f;
        
        [SerializeField, FoldoutGroup("IK Settings"), Range(0, 1)]
        private float _bodyIkWeight = 0.3f;
        
        [SerializeField, FoldoutGroup("IK Settings"), Range(0, 1)]
        private float _headIkWeight = 0.4f;
        
        [SerializeField, FoldoutGroup("IK Settings"), Range(0, 1)]
        private float eyesIkWeight = 0.3f;
        
        [SerializeField, FoldoutGroup("IK Settings"), Range(0, 1)]
        private float _clampIkWeight = 0.3f;
        
        public bool UseRootMotion => _useRootMotion;

        public KeyCode JumpInput => _jumpInput;
        public KeyCode StrafeInput => _strafeInput;
        public KeyCode SprintInput => _sprintInput;

        public float SprintCost => _sprintCost;
        public CharacterLocomotionType LocomotionType => _locomotionType;
        public CharacterMovementPair FreeMovementPair => _freeMovementPair;
        public CharacterMovementPair StrafeMovementPair => _strafeMovementPair;

        public bool JumpWithRigidbodyForce => _jumpWithRigidbodyForce;
        public bool JumpAndRotate => _jumpAndRotate;
        public float JumpTimer => _jumpTimer;
        public float JumpHeight => _jumpHeight;
        public float AirSpeed => _airSpeed;
        public float AirSmooth => _airSmooth;
        public float ExtraGravity => _extraGravity;
        
        public LayerMask GroundLayer => _groundLayer;
        public float GroundMinDistance => _groundMinDistance;
        public float GroundMaxDistance => _groundMaxDistance;
        public float SlopeLimit => _slopeLimit;


        public float MainWeight => _mainWeight;
        public float BodyIkWeight => _bodyIkWeight;
        public float HeadIkWeight => _headIkWeight;
        public float EyesIkWeight => eyesIkWeight;
        public float ClampIkWeight => _clampIkWeight;
    }
    
    [HideReferenceObjectPicker]
    public sealed class CharacterMovementPair
    {
        [SerializeField, BoxGroup, Range(1f, 20f)]
        private float _movementSmooth = 6f;
        
        [SerializeField, BoxGroup, Range(0f, 1f)]
        private float _animationSmooth = 0.2f;
        
        [SerializeField, BoxGroup]
        private float _rotationSpeed = 16f;
        
        [SerializeField, BoxGroup]
        private bool _walkByDefault = false;
        
        [SerializeField, BoxGroup]
        private bool _rotateWithCamera = false;
        
        [SerializeField, BoxGroup]
        private float _walkSpeed = 2f;
        
        [SerializeField, BoxGroup]
        private float _runningSpeed = 4f;
        
        [SerializeField, BoxGroup]
        private float _sprintSpeed = 6f;
        
        public float MovementSmooth => _movementSmooth;
        public float AnimationSmooth => _animationSmooth;
        public float RotationSpeed => _rotationSpeed;
        public bool WalkByDefault => _walkByDefault;
        public bool RotateWithCamera => _rotateWithCamera;
        public float WalkSpeed => _walkSpeed;
        public float RunningSpeed => _runningSpeed;
        public float SprintSpeed => _sprintSpeed;
    }
    
    public enum CharacterLocomotionType
    {
        FreeWithStrafe,
        OnlyStrafe,
        OnlyFree,
    }
}