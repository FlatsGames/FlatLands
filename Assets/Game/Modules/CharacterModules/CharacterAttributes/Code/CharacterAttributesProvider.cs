using System.Collections.Generic;
using FlatLands.Characters;
using FlatLands.GameAttributes;

namespace FlatLands.CharacterAttributes
{
	public sealed class CharacterAttributesProvider : IGameAttributeHolder, ICharacterProvider
	{
		private Dictionary<GameAttributeType, BaseAttributeData> _attributes;
		public IReadOnlyDictionary<GameAttributeType, BaseAttributeData> GetAttributes() => _attributes;

		public CharacterAttributesProvider()
		{
			_attributes = new Dictionary<GameAttributeType, BaseAttributeData>()
			{
				{GameAttributeType.Health, new AttributeRangedData(GameAttributeType.Health, 0, 200, 200)},
				{GameAttributeType.Stamina, new AttributeRegeneratedData(GameAttributeType.Stamina, 0, 100, 100, 1, 10)}
			};
		}

		public void Init()
		{
			
		}

		public void Dispose()
		{
			
		}

		public void HandleUpdate()
		{
			
		}

		public void HandleFixedUpdate()
		{
			
		}
	}
}
