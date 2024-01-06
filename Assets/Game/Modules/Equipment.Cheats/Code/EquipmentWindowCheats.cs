using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.CharacterEquipment;
using FlatLands.Characters;
using FlatLands.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Equipments.Cheats
{
	public sealed class EquipmentWindowCheats : UIWindow
	{
		[Inject] private Container _container;
		[Inject] private CharactersManager _characterManager;
		[Inject] private EquipmentManager _equipmentManager;
		
		[SerializeField, BoxGroup("Cheat Settings")] private EquipmentItemWidgetCheat _prefab;
		[SerializeField, BoxGroup("Cheat Settings")] private Transform _characterContent;
		[SerializeField, BoxGroup("Cheat Settings")] private Transform _allContent;

		private List<EquipmentItemWidgetCheat> _characterWidgets;
		private List<EquipmentItemWidgetCheat> _allWidgets;
		
		public override void Init()
		{
			_characterWidgets = new List<EquipmentItemWidgetCheat>();
			_allWidgets = new List<EquipmentItemWidgetCheat>();

			_equipmentManager.OnEquipmentItemAdded += HandleEquipmentItemChanged;
			_equipmentManager.OnEquipmentItemRemoved += HandleEquipmentItemChanged;

			CreateCharacterWidgets();
			CreateAllWidgets();
		}

		public override void Dispose()
		{
			_equipmentManager.OnEquipmentItemAdded -= HandleEquipmentItemChanged;
			_equipmentManager.OnEquipmentItemRemoved -= HandleEquipmentItemChanged;
			
			DestroyCharacterWidgets();
			DestroyAllWidgets();
		}

		private void CreateCharacterWidgets()
		{
			var characterEquipmentProvider = _characterManager.CurrentCharacter.GetProvider<CharacterEquipmentProvider>();
			foreach (var (slotType, (config, behaviour)) in characterEquipmentProvider.EquipmentSlots)
			{
				var createdWidget = Instantiate(_prefab, _characterContent);
				createdWidget.Init(config, characterEquipmentProvider);
				_container.InjectAt(createdWidget);
				_characterWidgets.Add(createdWidget);
				createdWidget.UpdateState();
			}
		}

		private void CreateAllWidgets()
		{
			var characterEquipmentProvider = _characterManager.CurrentCharacter.GetProvider<CharacterEquipmentProvider>();
			var allEquipments = BaseEquipmentItemConfig.Objects;
			foreach (var equipmentItemConfig in allEquipments)
			{
				var createdWidget = Instantiate(_prefab, _allContent);
				createdWidget.Init(equipmentItemConfig, characterEquipmentProvider);
				_container.InjectAt(createdWidget);
				_allWidgets.Add(createdWidget);
				createdWidget.UpdateState();
			}
		}

		private void DestroyCharacterWidgets()
		{
			foreach (var widget in _characterWidgets)
			{
				Destroy(widget.gameObject);
			}
			
			_characterWidgets.Clear();
		}
		
		private void DestroyAllWidgets()
		{
			foreach (var widget in _allWidgets)
			{
				Destroy(widget.gameObject);
			}
			
			_allWidgets.Clear();
		}

		private void HandleEquipmentItemChanged(EquipmentSlotType slotType)
		{
			DestroyCharacterWidgets();
			CreateCharacterWidgets();
			
			foreach (var widget in _allWidgets)
			{
				widget.UpdateState();
			}
		}
	}
}
