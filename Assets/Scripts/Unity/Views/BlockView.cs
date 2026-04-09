using UnityEngine;
using UnityEngine.Events;
using PuzzleGame.Core.Models;

namespace PuzzleGame.Unity.Views
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private BlockColor _blockColor;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [Header("Color Prefabs")]
        [SerializeField] private GameObject _greenPrefab;
        [SerializeField] private GameObject _purplePrefab;
        [SerializeField] private GameObject _yellowPrefab;
        [SerializeField] private GameObject _brownPrefab;
        [SerializeField] private GameObject _pinkPrefab;
        
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
            GameObject targetPrefab = _blockColor switch
            {
                BlockColor.GREEN => _greenPrefab,
                BlockColor.PURPLE => _purplePrefab,
                BlockColor.YELLOW => _yellowPrefab,
                BlockColor.BROWN => _brownPrefab,
                BlockColor.PINK => _pinkPrefab,
                _ => _greenPrefab
            };

            if (targetPrefab != null)
            {
                // Get sprite from the prefab's SpriteRenderer
                SpriteRenderer prefabRenderer = targetPrefab.GetComponent<SpriteRenderer>();
                if (prefabRenderer != null && _spriteRenderer != null)
                {
                    _spriteRenderer.sprite = prefabRenderer.sprite;
                    _spriteRenderer.color = prefabRenderer.color;
                }
            }
        }

        private void OnMouseDown()
        {
            OnBlockClicked?.Invoke(this);
        }

        private void Awake()
        {
            if (_spriteRenderer == null) Debug.LogError("SpriteRenderer is not assigned!");
            if (GetComponent<BoxCollider2D>() == null) Debug.LogError("BoxCollider2D is not assigned!");
            ValidatePrefabs();
        }

        private void ValidatePrefabs()
        {
            if (_greenPrefab == null) Debug.LogError("Green Prefab is not assigned!");
            if (_purplePrefab == null) Debug.LogError("Purple Prefab is not assigned!");
            if (_yellowPrefab == null) Debug.LogError("Yellow Prefab is not assigned!");
            if (_brownPrefab == null) Debug.LogError("Brown Prefab is not assigned!");
            if (_pinkPrefab == null) Debug.LogError("Pink Prefab is not assigned!");
        }
    }
}
