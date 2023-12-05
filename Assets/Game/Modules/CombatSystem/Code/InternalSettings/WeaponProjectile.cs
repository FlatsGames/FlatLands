using UnityEngine;

namespace FlatLands.CombatSystem
{
    public sealed class WeaponProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        public Rigidbody RigidbodyComponent => _rigidbody;
        public Collider ColliderComponent => _collider;
        
        public Vector3 ProjectileDirection => transform.up;

        private bool _isActive;
        private bool _hitted;
        
        public void SetActive(bool active)
        {
            _isActive = active;

            _rigidbody.isKinematic = !_isActive;
            _rigidbody.useGravity = _isActive;
            _collider.enabled = _isActive;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(_hitted)
                return;
            
            if(other.gameObject == null)
               return;
            
            transform.SetParent(other.transform);
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            _hitted = true;
            Destroy(gameObject, 10);
        }
        
        
    }
}