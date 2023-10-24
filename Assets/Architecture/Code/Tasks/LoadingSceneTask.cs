using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace FlatLands.Architecture
{
	public sealed class LoadingSceneTask : ITask<LoadingSceneTask>
	{
		public string SceneName { get; }
		public float Progress => _loadingOperation?.progress ?? 0;
		public bool IsDone => _loadingOperation?.isDone ?? false;
		
		public event Action<LoadingSceneTask> OnCompleted;

		private AsyncOperation _loadingOperation;
		private bool _isLoading;

		public LoadingSceneTask(string sceneName, bool isLoading = true)
		{
			SceneName     = sceneName;
			_isLoading     = isLoading;
		}
		
		public LoadingSceneTask(Object sceneAsset,  bool isLoading = true)
		{
			#if UNITY_EDITOR
			
			if (!AssetDatabase.GetAssetPath(sceneAsset).EndsWith(".unity"))
				throw new ArgumentException($"Asset '{sceneAsset}' must be scene!");
			
			#endif
			
			SceneName     = sceneAsset.name;
			_isLoading     = isLoading;
		}

		public void Start(bool allowSceneActivation = false, int priority = 0)
		{
			 _loadingOperation = _isLoading ?
				SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive) :
				SceneManager.UnloadSceneAsync(SceneName);

			 _loadingOperation.priority = priority;
			 _loadingOperation.allowSceneActivation = allowSceneActivation;
			 
			_loadingOperation.completed += HandleComplete;
		}

		private void HandleComplete(AsyncOperation operation)
		{
			_loadingOperation.completed -= HandleComplete;
			
			OnCompleted?.Invoke(this);
		}
	}
}