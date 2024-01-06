using FlatLands.Architecture;
using FlatLands.CharacterEquipment;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlatLands.Equipments.Cheats
{
    public sealed class EquipmentItemWidgetCheat : SerializedMonoBehaviour
    {
        [Inject] private EquipmentManager _equipmentManager;
        private CharacterEquipmentProvider _characterEquipmentProvider;
		
        [SerializeField] private TMP_Text _tittleText;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _removeButton;
        
        private BaseEquipmentItemConfig _config;
		
        public void Init(BaseEquipmentItemConfig config, CharacterEquipmentProvider characterEquipmentProvider)
        {
            _config = config;
            _characterEquipmentProvider = characterEquipmentProvider;
            _addButton.onClick.AddListener(HandleAddClicked);
            _removeButton.onClick.AddListener(HandleRemoveClicked);
        }

        public void Dispose()
        {
            _addButton.onClick.RemoveListener(HandleAddClicked);
            _removeButton.onClick.RemoveListener(HandleRemoveClicked);
        }

        public void UpdateState()
        {
            _tittleText.text = _config.name;
            
            var itemConfigInSlot = _characterEquipmentProvider.GetItemConfigInSlot(_config.SlotType);
			
            var canAdd = _characterEquipmentProvider.HasSlot(_config.SlotType) && itemConfigInSlot != _config;
            _addButton.gameObject.SetActive(canAdd);

            var canRemove = itemConfigInSlot != null && itemConfigInSlot == _config;
            _removeButton.gameObject.SetActive(canRemove);
        }

        private void HandleAddClicked()
        {
            _equipmentManager.AddEquipmentItemToProvider(_config, _characterEquipmentProvider);
            Debug.Log("Click");
        }
		
        private void HandleRemoveClicked()
        {
            _equipmentManager.RemoveEquipmentItemFromProvider(_config.SlotType, _characterEquipmentProvider);
            Debug.Log("Click");
        }
    }
}