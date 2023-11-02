using System;
using FlatLands.EntityControllable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Characters
{
    public sealed class CharacterBehaviour : SerializedMonoBehaviour, IEntityControllableBehaviour
    {
        [SerializeField, BoxGroup("Main Components")]
        private Animator _animator;
        
        [SerializeField, BoxGroup("Main Components")]
        private Rigidbody _rigidbody;   
        
        [SerializeField, BoxGroup("Main Components")]
        private CapsuleCollider _capsuleCollider;

        public string Name => "MainCharacter";
        public Transform EntityTransform => transform;

        public Animator CharacterAnimator => _animator;
        public Rigidbody CharacterRigidbody => _rigidbody;
        public CapsuleCollider CharacterCollider => _capsuleCollider;

        public event Action OnAnimatorMoved;
        public event Action<int> OnAnimatorIks;
        
        private void OnAnimatorMove()
        {
            OnAnimatorMoved?.Invoke();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            OnAnimatorIks?.Invoke(layerIndex);
        }
    }
}

