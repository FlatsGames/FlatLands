using System;
using FlatLands.Architecture;

namespace FlatLands.GameAttributes
{
	public abstract class BaseAttributeData
	{
		public GameAttributeType Type;
		public float Value { get; protected set; }

		public event Action OnValueChanged;
		
		public void AddValue(float amount)
		{
			Value += amount;
			OnValueChanged?.Invoke();
		}
		
		public void RemoveValue(float amount)
		{
			Value -= amount;
			OnValueChanged?.Invoke();
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
