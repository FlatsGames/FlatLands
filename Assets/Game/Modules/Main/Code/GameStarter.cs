using System.Collections;
using System.Diagnostics;
using System.Linq;
using FlatLands.Architecture;
using FlatLands.Characters;
using FlatLands.CharactersMediator;
using FlatLands.Conditions;
using FlatLands.Cursors;
using FlatLands.EntityControllable;
using FlatLands.Equipments;
using FlatLands.GameAttributes;
using FlatLands.GeneralCamera;
using FlatLands.Inventory;
using FlatLands.Items;
using FlatLands.Locations;
using FlatLands.Loader;
using FlatLands.LocationsCamera;
using FlatLands.LocationsObjects;
using FlatLands.UI;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace FlatLands.Main
{
    public sealed class GameStarter : MonoBehaviour
    {
        private const string Main_Container_Name = "MainContainer";
        
        private Container _container;

        private Stopwatch _loadingWatcher;
        
        private  void Start()
        {
            _loadingWatcher = new Stopwatch();
            _loadingWatcher.Start();
            
            _container = new Container(Main_Container_Name);
            GlobalContainer.SetContainer(_container);

            _container.Add<LoaderManager>();

            //Conditions
            _container.Add<ConditionsController>();
            
            //UI
            _container.Add<PrimaryUIService>();
            _container.Add<UIManager>();
            _container.Add<CursorManager>();

            //Locations
            _container.Add<PrimaryLocationsService>();
            _container.Add<LocationsManager>();
            
            //Main Menu
            _container.Add<PrimaryMainMenuService>();
            _container.Add<MainMenuManager>();

            //Cameras
            _container.Add<PrimaryCameraManager>();
            _container.Add<GeneralCameraManager>();
            _container.Add<LocationsCameraManager>();
            
            //Entity Controllable
            _container.Add<EntityControllableManager>();
            
            //Characters
            _container.Add<CharactersManager>();
            
            //Character Mediator
            _container.Add<CharactersMediatorManager>();
            
            //Items
            _container.Add<ItemsManager>();
            _container.Add<ItemsSpawnManager>();
            
            //UseObjects
            _container.Add<LocationObjectsUseManager>();
            
            //Equipments
            _container.Add<EquipmentManager>();
            
            //GameAttributes
            _container.Add<GameAttributesManager>();
            
            //Inventory
            _container.Add<InventoryManager>();
            _container.Add<InventoryModel>();
            
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
            
            _loadingWatcher.Stop();
            Debug.Log($"[Loading] COMPLETED! Sec: {_loadingWatcher.Elapsed.TotalSeconds}");
        }
    }
}
