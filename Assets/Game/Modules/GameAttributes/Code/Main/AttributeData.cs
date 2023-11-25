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

		public virtual void AddValue(float amount)
		{
			var prevValue = Value;
			Value += amount;
			InvokeValueChanged(true, prevValue);
		}
		
		public virtual void RemoveValue(float amount)
		{
			var prevValue = Value;
			Value -= amount;
			InvokeValueChanged(false, prevValue);
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

		protected void InvokeValueChanged(bool addValue, float prevValue)
		{
			OnValueChanged?.Invoke();
			OnValueChangedDetails?.Invoke(addValue, prevValue, Value);
		}
	}
}
