using System;

namespace FlatLands.Architecture
{
	public interface ITask<T>
	{
		bool IsDone { get; }

		float Progress { get; }

		event Action<T> OnCompleted;

		void Start(int priority);
	}
}