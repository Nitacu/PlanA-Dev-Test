using UnityEngine;
using UnityEngine.Events;
using PuzzleGame.Core.Models;

namespace PuzzleGame.Unity.Views
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private BlockColor _blockColor;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public BlockColor BlockColor => _blockColor;
        public Vector2Int GridPosition { get; private set; }
        
        public UnityEvent<BlockView> OnBlockClicked = new UnityEvent<BlockView>();

        public void Initialize(BlockColor color, Vector2Int gridPosition)
        {
            _blockColor = color;
            GridPosition = gridPosition;
            UpdateVisual();
        }

        public void UpdateColor(BlockColor newColor)
        {
            _blockColor = newColor;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            // Update sprite based on color - will be implemented with sprite references
        }

        private void OnMouseDown()
        {
            OnBlockClicked?.Invoke(this);
        }

        private void Awake()
        {
            if (_spriteRenderer == null) Debug.LogError("SpriteRenderer is not assigned!");
        }
    }
}
