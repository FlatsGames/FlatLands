using System.Collections;
using System.Linq;
using FlatLands.Architecture;
using FlatLands.GeneralCamera;
using FlatLands.Locations;
using FlatLands.Loader;
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

            _container.Add<LoaderManager>();

            //UI
            _container.Add<PrimaryUIService>();
            _container.Add<UIManager>();

            //Locations
            _container.Add<PrimaryLocationsService>();
            _container.Add<LocationsManager>();
            
            //Main Menu
            _container.Add<PrimaryMainMenuService>();
            _container.Add<MainMenuManager>();
            
            //Cameras
            _container.Add<PrimaryCameraManager>();
            _container.Add<GeneralCameraManager>();
            
            _container.ApplyDependencies();
            
            StartCoroutine(StartLoading());
        }
        
        private IEnumerator StartLoading()
        {
            var loaderManager = _container.Get<LoaderManager>();
            loaderManager.ShowLoadingScreen();
            
            var SharedObjects = _container.SharedObjects.Distinct().ToList();
            var Loadable= _container.GetAll<IGeneralSceneLoader>().Distinct().OrderBy(l => l.LoadingSceneOrder).ToList();
            
            var aggregator = new ProgressAggregator();
            aggregator.AddJobs(SharedObjects.Count);
            aggregator.AddJobs(Loadable.Count);
            
            aggregator.OnUpdate += value => loaderManager.UpdateLoadingScreenProgress(value, aggregator.JobsCount);

            yield return null;
            
            _container.PrimaryInit(()=> aggregator.Next());
            
            yield return null;
            
            foreach (var loadable in Loadable)
            {
                if(loadable.NeedLoad)
                {
                    var task = loaderManager.LoadSceneAsync(loadable.GetLoadingSceneName(), false, () => loadable.InvokeSceneLoaded());
                    task.Start(true);
                    while (!task.IsDone)
                    {
                        aggregator.SetSubProgress(task.Progress);
                        yield return null;
                    }
                }
            
                aggregator.Next();
                yield return null;
            }
            
            yield return null;

            _container.Init(() => aggregator.Next());
            
            yield return null;

            HandleLoadingComplete();
        }
        
        private void HandleLoadingComplete()
        {
            var loaderManager = _container.Get<LoaderManager>();
            loaderManager.HideLoadingScreen();
            Debug.Log("[Loading] COMPLETED!");
        }
    }
}
