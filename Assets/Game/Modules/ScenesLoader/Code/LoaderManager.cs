using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Loader
{
    public sealed class LoaderManager : SharedObject
    {
        private LoadingScreen _loadingScreen;
        private LoadingScreen Loading
        {
            get
            {
                if (_loadingScreen == null)
                    _loadingScreen = GameObject.FindObjectOfType<LoadingScreen>();

                return _loadingScreen;
            }
        }

        public void ShowLoadingScreen(Action callback = null)
        {
            Loading.ShowScreen(callback);
        }

        public void HideLoadingScreen(Action callback = null)
        {
            Loading.HideScreen(callback);
        }

        public void UpdateLoadingScreenProgress(float progress, float maxProgress)
        {
            Loading.UpdateProgress(progress, maxProgress);
        }
        
        public LoadingSceneTask LoadSceneAsync(string sceneName, bool start = true, Action callback = null)
        {
            var task = new LoadingSceneTask(sceneName);
            
            task.OnCompleted += HandleTaskCompleted;
            if(start)
                task.Start();

            void HandleTaskCompleted(LoadingSceneTask loadingScene)
            {
                loadingScene.OnCompleted -= HandleTaskCompleted;
                callback?.Invoke();
            }

            return task;
        }
    }
}