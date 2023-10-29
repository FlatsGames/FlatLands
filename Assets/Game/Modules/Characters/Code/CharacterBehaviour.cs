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

        public string Name { get; }
        public Transform EntityTransform { get; }

        public Animator CharacterAnimator => _animator;
        public Rigidbody CharacterRigidbody => _rigidbody;
        public CapsuleCollider CharacterCollider => _capsuleCollider;

        public event Action OnAnimatorMoved;
        
        private void OnAnimatorMove()
        {
            OnAnimatorMoved?.Invoke();
        }
    }
}

