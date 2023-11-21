using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace FlatLands.Equipments
{
	public abstract class BaseEquipmentBehaviour : SerializedMonoBehaviour
	{
		[SerializeField] private TwoBoneIKConstraint _leftHandConstraint;
		[SerializeField] private TwoBoneIKConstraint _rightHandConstraint;
		
		[SerializeField, ReadOnly] 
		private Dictionary<WeaponEquipmentSlotType, EquipmentSlotBehaviour> _equipmentSlotsBehaviourPairs;
		
		public TwoBoneIKConstraint LeftHandConstraint => _leftHandConstraint;
		public TwoBoneIKConstraint RightHandConstraint => _rightHandConstraint;

		public EquipmentSlotBehaviour GetBehaviour(WeaponEquipmentSlotType type)
		{
			if (!_equipmentSlotsBehaviourPairs.TryGetValue(type, out var behaviour))
				return default;

			return behaviour;
		}
		
#if UNITY_EDITOR
		
		[Button]
		private void RefreshSlots()
		{
			var behaviours = gameObject.GetComponentsInChildren<EquipmentSlotBehaviour>();
			_equipmentSlotsBehaviourPairs = new Dictionary<WeaponEquipmentSlotType, EquipmentSlotBehaviour>();
			foreach (var slotBehaviour in behaviours)
			{
				_equipmentSlotsBehaviourPairs[slotBehaviour.SlotType] = slotBehaviour;
			}
		}
#endif
		
		
	}

	public enum WeaponEquipmentSlotType
	{
		None = 0,
		LeftLegSlot = 50,
		SpineSlot = 100,
		BeltSlot = 150
	}
}
