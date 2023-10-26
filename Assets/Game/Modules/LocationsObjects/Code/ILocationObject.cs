using UnityEngine;

namespace FlatLands.LocationsObjects
{ 
	public interface ILocationObject
	{
		public string Id { get; }
		public GameObject LocationObject { get; }
	}
}
