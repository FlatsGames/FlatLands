namespace FlatLands.Conditions
{
	public abstract class BaseCondition
	{
		public abstract bool CanApply { get; }
		public abstract void ApplyCondition();
	}
}
