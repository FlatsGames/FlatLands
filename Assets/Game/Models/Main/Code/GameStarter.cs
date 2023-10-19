using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Main
{
    public sealed class GameStarter : MonoBehaviour
    {
        private const string Main_Container_Name = "MainContainer";
        
        private Container _container;

        private async void Start()
        {
            _container = new Container(Main_Container_Name);
            
            
            _container.Init();
        }
    }
}
