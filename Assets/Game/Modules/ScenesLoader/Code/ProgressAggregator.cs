using System;

namespace FlatLands.Loader
{
	public sealed class ProgressAggregator
	{
		public int JobsCount { get; private set; }
		public int Jobs      { get; private set; }
		
		public event Action<float> OnUpdate;
		
		public ProgressAggregator()
		{
			Reset();
		}
		
		public void Reset()
		{
			JobsCount = 0;
			Jobs      = 0;
		}
		
		public void AddJobs(int jobs)
		{
			JobsCount += jobs;
		}
		
		public void Next()
		{
			Jobs++;
			
			OnUpdate?.Invoke(Jobs / (float) JobsCount);
		}
		
		public void SetSubProgress(float value)
		{
			OnUpdate?.Invoke((Jobs + value) / (float) JobsCount);
		}
	}
}