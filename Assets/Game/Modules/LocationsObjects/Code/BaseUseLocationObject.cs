using System;
using UnityEngine;

namespace FlatLands.LocationsObjects
{
    public abstract class BaseUseLocationObject
    {
        [SerializeField] 
        private KeyCode _useKey;
        public KeyCode UseKey => _useKey;
        
        public abstract Type UseType { get; }
    }
}