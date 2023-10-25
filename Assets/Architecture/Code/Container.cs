using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlatLands.Architecture
{
	public sealed class Container
	{
		public IReadOnlyList<IShared> SharedObjects => _shareds.Values.ToList();

		private Dictionary<Type, IShared> _shareds;

		public string Name   { get; }
		public bool Active { get; private set; }

		public Container(string name)
		{
			Name = name;
			
			Active   = false;
			_shareds = new Dictionary<Type, IShared>();
		}

		public T Add<T>() where T : IShared, new()
		{
			return Add(new T());
		}

		public T Add<T>(T shared) where T : IShared
		{
			Type type = typeof(T);
			if (_shareds.ContainsKey(type))
				throw new Exception($"[Architecture] Container already contains instance of Type '{type.Name}'");
			_shareds.Add(type, shared);
			
			Type inter = typeof(ISharedInterface);
			var subtypes = @type.GetInterfaces();
			foreach (var sub in subtypes)
			{
				if (sub == null || sub == inter || sub == typeof(IShared))
					continue;
				
				if (!inter.IsAssignableFrom(sub))
					continue;
				
				if (_shareds.ContainsKey(sub))
					throw new Exception($"[Architecture] Container already contains instance of Interface '{sub.Name}'");
				_shareds.Add(sub, shared);
			}
			
			return shared;
		}

		public T Get<T>() where T : ISharedInterface
		{
			Type type = typeof(T);
			if (!_shareds.TryGetValue(type, out var shared))
				throw new Exception($"[Architecture] Container doesn't contains element of Type '{type.Name}'");
			
			return (T) shared;
		}

		public object Get(Type type)
		{
			if (!_shareds.TryGetValue(type, out var shared))
				throw new Exception($"[Architecture] Container doesn't contains element of Type '{type.Name}'");
			
			return shared;
		}

		public List<I> GetAll<I>(List<I> objects = null)
		{
			Type target = typeof(I);

			if (objects == null)
				objects = new List<I>();
			

			foreach (var shared in _shareds.Values)
			{
				if (shared.GetType().GetInterfaces().Contains(target))
					objects.Add((I) shared);
			}

			return objects;
		}

		public void ApplyDependencies()
		{
			var containerType = typeof(Container);

			foreach (var pair in _shareds)
			{
				IShared sharedObject = pair.Value;
				sharedObject.SetContainer(this);
				
				Type type = pair.Key;
				var fields = type.GetAllFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (var field in fields)
				{
					Attribute attr = Attribute.GetCustomAttribute(field, typeof(Inject));
					if (attr != null)
					{
						object other = (field.FieldType == containerType) ? this : Get(field.FieldType);
						field.SetValue(sharedObject, other);
					}
				}
			}
		}

		public void PrimaryInit(Action sharedInitialized = null)
		{
			var primaryObjects = _shareds.Values.Where(v => v is PrimarySharedObject);
			foreach (var shared in primaryObjects)
			{
				shared.Init();
				sharedInitialized?.Invoke();
			}
		}
		
		public void Init(Action sharedInitialized = null)
		{
			var sharedObjects =  _shareds.Values.Where(v => v is not PrimarySharedObject);
			foreach (var shared in sharedObjects)
			{
				shared.Init();
				sharedInitialized?.Invoke();
			}

			Active = true;
		}

		public void InjectAt(object target)
		{
			var type          = target.GetType();
			var containerType = typeof(Container);

			var fields = type.GetAllFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (var field in fields)
			{
				Attribute attr = Attribute.GetCustomAttribute(field, typeof(Inject));

				if (attr != null)
				{
					object other = (field.FieldType == containerType) ? this : Get(field.FieldType);
					field.SetValue(target, other);
				}
			}
		}

		public void Dispose()
		{
			Active = false;

			foreach (var pair in _shareds)
			{
				var shared = pair.Value;
				shared.Dispose();
			}
		}
	}
}