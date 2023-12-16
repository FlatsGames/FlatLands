using System.Collections.Generic;
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
        private KeyCode _sprintInput = KeyCode.LeftShift;

        [SerializeField, FoldoutGroup("Movement Settings")]
        private float _sprintCost;
        
        [SerializeField, FoldoutGroup("Movement Settings")]
        private CharacterLocomotionType _defaultLocomotionType = CharacterLocomotionType.Strafe;

        [SerializeField, FoldoutGroup("Movement Settings")]
        private Dictionary<CharacterLocomotionType, CharacterMovementPair> _movementPairs;

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
        
        public bool UseRootMotion => _useRootMotion;

        public KeyCode JumpInput => _jumpInput;
        public KeyCode SprintInput => _sprintInput;

        public float SprintCost => _sprintCost;
        public CharacterLocomotionType DefaultLocomotionType => _defaultLocomotionType;
        public IReadOnlyDictionary<CharacterLocomotionType, CharacterMovementPair> MovementPairs => _movementPairs;
        // public CharacterMovementPair FreeMovementPair => _freeMovementPair;
        // public CharacterMovementPair StrafeMovementPair => _strafeMovementPair;

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
        private bool _rotateWithCamera = false;
        
        [SerializeField, BoxGroup]
        private bool _canSprinting;

        [SerializeField, BoxGroup]
        private float _mainSpeed = 4f;

        [SerializeField, BoxGroup, ShowIf(nameof(_canSprinting))]
        private float _sprintSpeed = 6f;

        public float MovementSmooth => _movementSmooth;
        public float AnimationSmooth => _animationSmooth;
        public float RotationSpeed => _rotationSpeed;
        public bool RotateWithCamera => _rotateWithCamera;
        public bool CanSprinting => _canSprinting;
        public float MainSpeed => _mainSpeed;
        public float SprintSpeed => _sprintSpeed;
    }
    
    public enum CharacterLocomotionType
    {
        Walk = 0,
        WalkBattle = 1,
        
        Strafe = 50,
        StrafeBattle = 51
    }
}