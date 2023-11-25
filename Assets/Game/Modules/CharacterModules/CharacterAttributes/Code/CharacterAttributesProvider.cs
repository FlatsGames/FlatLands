using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.Characters;
using FlatLands.GameAttributes;
using FlatLands.UI;

namespace FlatLands.CharacterAttributes
{
	public sealed class CharacterAttributesProvider : ICharacterProvider, IGameAttributeHolder
	{
		[Inject] private GameAttributesManager _gameAttributesManager;
		[Inject] private UIManager _uiManager;

		public string HolderId => "CharacterAttributes";

		private CharacterAttributesConfig _config;

		public void Init()
		{
			_config = CharacterAttributesConfig.Instance;
			
			_gameAttributesManager.RegisterHolder(this);
			
			SetHudElementHealth();
			SetHudElementStamina();
		}

		public void Dispose()
		{
			_gameAttributesManager.RemoveHolder(this);
		}

		public void HandleUpdate()
		{
			
		}

		public void HandleFixedUpdate()
		{
			
		}

		public IReadOnlyDictionary<GameAttributeType, AttributeData> GetAttributes()
		{
			return _config.Attributes;
		}

		public T GetAttribute<T>(GameAttributeType type) where T : AttributeData
		{
			if (!_config.Attributes.TryGetValue(type, out var attributeData))
				return default;
			
			return (T) attributeData;
		}

		private void SetHudElementHealth()
		{
			if(!_config.Attributes.TryGetValue(GameAttributeType.Health, out var attributeData))
				return;
			
			var hudElement =  _uiManager.GetHudElement<UIHealthHudElement>(UIHudElementType.Health);
			var regeneratedAttribute = attributeData as AttributeRangedData;
			hudElement.SetAttribute(regeneratedAttribute);
		}
		
		private void SetHudElementStamina()
		{
			if(!_config.Attributes.TryGetValue(GameAttributeType.Stamina, out var attributeData))
				return;
			
			var hudElement =  _uiManager.GetHudElement<UIStaminaHudElement>(UIHudElementType.Stamina);
			var regeneratedAttribute = attributeData as AttributeRegeneratedData;
			hudElement.SetAttribute(regeneratedAttribute);
		}
	}
}
