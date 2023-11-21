using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

namespace FlatLands.Equipments
{
    [ExecuteInEditMode]
    public sealed class CharacterRiggingHelper : SerializedMonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private List<RiggingHelperPair> _helperPairs;

        [Button]
        private void ApplyHelpers()
        {
            foreach (var helper in _helperPairs)
            {
                helper.GetValueHelper();
            }
        }

        private void Update()
        {
            //_animator.Update(Time.deltaTime);
        }
    }

    public sealed class RiggingHelperPair
    {
        [SerializeField, BoxGroup] private MultiPositionConstraint _posConstraint;
        [SerializeField, BoxGroup] private TwoBoneIKConstraint _constraint;
        [SerializeField, BoxGroup] private Transform _target;
        [SerializeField, BoxGroup] private Vector3 _targetPos;
        [SerializeField, BoxGroup] private Vector3 _targetRot;

        [Space]
        [SerializeField, BoxGroup] private Transform _hint;
        [SerializeField, BoxGroup] private Vector3 _hintPos;
        [SerializeField, BoxGroup] private Vector3 _hintRot;

        public void GetValueHelper()
        {
            if (_target != null)
            {
                _targetPos = _target.localPosition;
                _targetRot = _target.localRotation.eulerAngles;
            }

            if (_hint != null)
            {
                _hintPos = _hint.localPosition;
                _hintRot = _hint.localRotation.eulerAngles;
            }
        }

        [Button]
        private void ApplyTarget()
        {
            if (_target != null)
            {
                _target.localPosition = _targetPos;
                _target.localRotation = Quaternion.Euler(_targetRot);
            }
        }
        
        [Button]
        private void ApplyHint()
        {
            if (_hint != null)
            {
                _hint.localPosition = _hintPos;
                _hint.localRotation = Quaternion.Euler(_hintPos);
            }
        }
    }
}