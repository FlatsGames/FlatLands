using System.Linq;
using FlatLands.Architecture;
using FlatLands.Locations;
using FlatLands.UI;
using UnityEngine;

namespace FlatLands.Main
{
    public sealed class GameStarter : MonoBehaviour
    {
        private const string Main_Container_Name = "MainContainer";
        
        private Container _container;
        
        private  void Start()
        {
            
            _container = new Container(Main_Container_Name);
            GlobalContainer.SetContainer(_container);

            _container.Add<UIManager>();
            _container.Add<LocationsManager>();
            _container.Add<MainMenuManager>();
            
            _container.ApplyDependencies();
            LoadingScenes();
        }

        private async void LoadingScenes()
        {
            var scenes = _container.GetAll<ISceneLoader>()
                .OrderBy(loader => loader.LoadingSceneOrder);

            foreach (var scene in scenes)
            {
                var sceneName = scene.GetLoadingSceneName();
                var task = new LoadingSceneTask(sceneName);
                
                task.OnCompleted += LoadingSceneCompleted;
                task.Start(scene.LoadingSceneOrder);
            }

            void LoadingSceneCompleted(LoadingSceneTask loadingScene)
            {
                loadingScene.OnCompleted -= LoadingSceneCompleted;
                Debug.Log($"Scene Loaded: {loadingScene.SceneName}");
            }
        }
    }
}
