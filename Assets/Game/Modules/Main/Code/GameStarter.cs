using System.Collections;
using System.Collections.Generic;
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
            _container.Add<UIManager>();
            _container.Add<LocationsManager>();
            _container.Add<MainMenuManager>();
            _container.Add<GeneralCameraManager>();
            
            _container.ApplyDependencies();
            _container.PreInit();
            
            StartCoroutine(StartLoading());
        }

        private IEnumerator StartLoading()
        {
            var loaderManager = _container.Get<LoaderManager>();
            loaderManager.ShowLoadingScreen();
            
            var SharedObjects = _container.SharedObjects.Distinct().ToList();
            var Loadable      = _container.GetAll<IGeneralSceneLoader>().Distinct().ToList();

            var aggreagator = new ProgressAggregator();
            aggreagator.Reset();
            aggreagator.AddJobs(3);
            aggreagator.AddJobs(SharedObjects.Count);
            aggreagator.AddJobs(Loadable.Count);
            
            aggreagator.OnUpdate += value => loaderManager.UpdateLoadingScreenProgress(value, aggreagator.JobsCount);

            yield return null;

            aggreagator.Next();
            yield return null;

            aggreagator.Next();
            yield return null;

            foreach (var shared in SharedObjects)
            {
                aggreagator.Next();
                yield return null;
            }

            foreach (var loadable in Loadable)
            {
                if(loadable.NeedLoad)
                {
                    var task = loaderManager.LoadSceneAsync(loadable.GetLoadingSceneName(), false, loadable.InvokeSceneLoaded);
                    task.Start(true);
                    while (!task.IsDone)
                    {
                        aggreagator.SetSubProgress(task.Progress);
                        yield return null;
                    }
                }

                aggreagator.Next();
                yield return null;
            }
            
            yield return null;

            aggreagator.Next();

            yield return new WaitForSeconds(3);

            _container.Init();

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
