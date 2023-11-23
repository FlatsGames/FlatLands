using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.Characters;
using FlatLands.GameAttributes;
using FlatLands.UI;
using UnityEngine;

namespace FlatLands.CharacterAttributes
{
	public sealed class CharacterAttributesProvider : ICharacterProvider
	{
		[Inject] private GameAttributesManager _gameAttributesManager;
		[Inject] private UIManager _uiManager;
		
		private CharacterAttributesConfig _config;
		
		public void Init()
		{
			_config = CharacterAttributesConfig.Instance;
			
			_gameAttributesManager.RegisterHolder(_config);
			
			SetHudElementHealth();
			//SetHudElementStamina();
		}

		public void Dispose()
		{
			_gameAttributesManager.RegisterHolder(_config);
		}

		public void HandleUpdate()
		{
			if (Input.GetKeyDown(KeyCode.B))
			{
				if(!_config.GetAttributes().TryGetValue(GameAttributeType.Health, out var attributeData))
					return;
				
				attributeData.AddValue(10);
			}
			
			if (Input.GetKeyDown(KeyCode.N))
			{
				if(!_config.GetAttributes().TryGetValue(GameAttributeType.Health, out var attributeData))
					return;
				
				attributeData.RemoveValue(10);
			}
		}

		public void HandleFixedUpdate()
		{
			
		}

		private void SetHudElementHealth()
		{
			if(!_config.GetAttributes().TryGetValue(GameAttributeType.Health, out var attributeData))
				return;
			
			var hudElement =  _uiManager.GetHudElement<UIAttributeRangedHudElement>(UIHudElementType.Health);
			var regeneratedAttribute = attributeData as AttributeRangedData;
			hudElement.SetAttribute(regeneratedAttribute);
		}
		
		private void SetHudElementStamina()
		{
			if(!_config.GetAttributes().TryGetValue(GameAttributeType.Stamina, out var attributeData))
				return;
			
			var hudElement =  _uiManager.GetHudElement<UIAttributeRangedHudElement>(UIHudElementType.Stamina);
			var regeneratedAttribute = attributeData as AttributeRangedData;
			hudElement.SetAttribute(regeneratedAttribute);
		}
	}
}
