using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace FlatLands.Equipments
{
	public abstract class BaseEquipmentProviderBehaviour : SerializedMonoBehaviour
	{
		[SerializeField] private Transform _leftHandHolder;
		[SerializeField] private Transform _rightHandHolder;
		
		[SerializeField, ReadOnly] 
		private Dictionary<EquipmentSlotType, EquipmentSlotBehaviour> _equipmentSlotsBehaviourPairs;

		public Transform LeftHandHolder => _leftHandHolder;
		public Transform RightHandHolder => _rightHandHolder;

		public EquipmentSlotBehaviour GetBehaviour(EquipmentSlotType type)
		{
			if (!_equipmentSlotsBehaviourPairs.TryGetValue(type, out var behaviour))
				return default;

			return behaviour;
		}

		public List<EquipmentSlotType> GetAllSlots()
		{
			return _equipmentSlotsBehaviourPairs.Keys.ToList();
		}

#if UNITY_EDITOR
		
		[Button]
		private void RefreshSlots()
		{
			var behaviours = gameObject.GetComponentsInChildren<EquipmentSlotBehaviour>();
			_equipmentSlotsBehaviourPairs = new Dictionary<EquipmentSlotType, EquipmentSlotBehaviour>();
			foreach (var slotBehaviour in behaviours)
			{
				_equipmentSlotsBehaviourPairs[slotBehaviour.SlotType] = slotBehaviour;
			}
		}
#endif
		
	}
}
