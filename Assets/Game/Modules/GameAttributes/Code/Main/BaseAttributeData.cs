using System;
using FlatLands.Architecture;

namespace FlatLands.GameAttributes
{
	public abstract class BaseAttributeData
	{
		public GameAttributeType Type;
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

		protected BaseAttributeData(GameAttributeType type, float baseValue = default)
		{
			Value = baseValue;
		}
	}
}
