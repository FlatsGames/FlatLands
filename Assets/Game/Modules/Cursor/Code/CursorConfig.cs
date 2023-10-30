using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Cursors
{
    [CreateAssetMenu(
        menuName = "FlatLands/UI/" + nameof(CursorConfig),
        fileName = nameof(CursorConfig))]
    public sealed class CursorConfig : SingletonScriptableObject<CursorConfig>
    {
        [SerializeField]
        private KeyCode _cursorButton;

        public KeyCode CursorButton => _cursorButton;
    }
}