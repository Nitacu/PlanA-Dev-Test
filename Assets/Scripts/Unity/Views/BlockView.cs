using UnityEngine;
using UnityEngine.Events;
using PuzzleGame.Core.Models;

namespace PuzzleGame.Unity.Views
{
    /// <summary>
    /// BlockView implements the View component in MVP pattern for individual puzzle blocks.
    /// 
    /// Architecture Decision: Unity Events over direct method calls
    /// - Why Unity Events: Decouples view from presenter, allowing multiple subscribers
    /// - Enables runtime flexibility: Different presenters can subscribe to same events
    /// - Supports inspector configuration: Events can be wired in Unity Editor
    /// 
    /// Architecture Decision: Serialized prefab references for colors
    /// - Why not runtime loading: Prefabs provide visual consistency and performance
    /// - Why not single sprite: Different colors need distinct visual representations
    /// - Supports: Artist workflow, visual assets managed through Unity's prefab system
    /// </summary>
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private BlockColor _blockColor;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        // Serialized prefab references - architectural decision for visual management
// Why not Addressables: Prefabs are core game assets, always loaded
// Why not runtime sprite loading: Prefabs provide complete visual packages
// Supports: Artist workflow, visual consistency, performance optimization
        [Header("Color Prefabs")]
        [SerializeField] private GameObject _greenPrefab;
        [SerializeField] private GameObject _purplePrefab;
        [SerializeField] private GameObject _yellowPrefab;
        [SerializeField] private GameObject _brownPrefab;
        [SerializeField] private GameObject _pinkPrefab;
        
        public BlockColor BlockColor => _blockColor;
        public Vector2Int GridPosition { get; private set; }
        
        // Unity Event for click handling - architectural choice for loose coupling
// Benefits: Multiple listeners can subscribe, no direct presenter dependency
// Alternative considered: Direct method calls via interface - rejected for flexibility
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
