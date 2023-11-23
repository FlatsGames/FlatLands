using System;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.GameAttributes
{
	public class AttributeData
	{
		[SerializeField] 
		private GameAttributeType _type;

		public GameAttributeType Type => _type;
		public float Value { get; protected set; }
		
		//1 - Add/Remove; 2 - Prev Value; 3 - CurValue
		public event Action<bool, float, float> OnValueChangedDetails;
		public event Action OnValueChanged;

		public void AddValue(float amount)
		{
			var prevValue = Value;
			Value += amount;
			OnValueChanged?.Invoke();
			OnValueChangedDetails?.Invoke(true, prevValue, Value);
		}
		
		public void RemoveValue(float amount)
		{
			var prevValue = Value;
			Value -= amount;
			OnValueChanged?.Invoke();
			OnValueChangedDetails?.Invoke(false, prevValue, Value);
		}

		internal virtual void Init()
		{
			UnityEventsProvider.OnUpdate += InvokeUpdate;
		}

		internal virtual void Dispose()
		{
			UnityEventsProvider.OnUpdate -= InvokeUpdate;
		}
		
		protected virtual void InvokeUpdate(){ }
	}
}
