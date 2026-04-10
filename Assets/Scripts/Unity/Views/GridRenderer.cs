using System.Collections.Generic;
using UnityEngine;
using PuzzleGame.Core.Models;
using PuzzleGame.Core.Services;
using UnityEngine.Events;

namespace PuzzleGame.Unity.Views
{
    /// <summary>
    /// GridRenderer manages visual grid representation - architectural bridge between data and visuals.
    /// 
    /// Architecture Decision: Unity Events for block click communication
    /// - Why events: Decouples grid rendering from input handling logic
    /// - Supports: Multiple input systems, different interaction patterns
    /// - Enables: Runtime flexibility, testability, clean separation of concerns
    /// 
    /// Architecture Decision: In-place block updates over object pooling
    /// - Why in-place updates: Simpler implementation, sufficient for current scale
    /// - Why not pooling: 6x5 grid (30 blocks) doesn't warrant complexity
    /// - Supports: Code maintainability, development speed, adequate performance
    /// </summary>
    public class GridRenderer : MonoBehaviour
    {
        [Header("Grid Configuration")]
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private Transform _gridParent;
        [SerializeField] private Transform _gridCenter;

        [SerializeField] private const int GRID_WIDTH = 5;
        [SerializeField] private const int GRID_HEIGHT = 6;
        [SerializeField] private const float CELL_WIDTH = 0.67368421f; // 128px / 190px per unit
        [SerializeField] private const float CELL_HEIGHT = 0.67368421f; // 128px / 190px per unit
        [SerializeField] private const float BLOCK_SIZE = 0.67368421f; // 128px / 190px per unit
        [SerializeField] private float rowSizeReduction = 0f;
        [SerializeField] private int baseSortingOrder = 0;
        
        private BlockView[,] _blockViews;

        // Unity Event for block clicks - architectural choice for loose coupling
// Benefits: GridRenderer doesn't need to know about input handling
// Supports: Different input methods (mouse, touch, keyboard), future flexibility
// Note: Passes grid coordinates for presenter logic processing
        public UnityEvent<Vector2Int> OnBlockClicked = new UnityEvent<Vector2Int>();

        private void Awake()
        {
            if (_gridCenter == null) Debug.LogError("Grid Center transform is not assigned!");
            CreateGrid();
        }

        private void CreateGrid()
        {
            _blockViews = new BlockView[GRID_WIDTH, GRID_HEIGHT];
            
            // Calculate grid center offset with row size reduction
            float gridWidthUnits = (GRID_WIDTH - 1) * CELL_WIDTH;
            float gridHeightUnits = (GRID_HEIGHT - 1) * (CELL_HEIGHT - rowSizeReduction);
            Vector3 centerOffset = new Vector3(-gridWidthUnits / 2f, -gridHeightUnits / 2f, 0);
            Vector3 basePosition = _gridCenter != null ? _gridCenter.position : Vector3.zero;
            
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    float adjustedCellHeight = CELL_HEIGHT - rowSizeReduction;
                    Vector3 position = basePosition + centerOffset + new Vector3(x * CELL_WIDTH, y * adjustedCellHeight, 0);
                    GameObject blockObject = Instantiate(_blockPrefab, position, Quaternion.identity, _gridParent);
                    
                    // Set sprite renderer sorting order - higher values render on top
                    SpriteRenderer spriteRenderer = blockObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        // Start at layer 2 and increase from bottom to top
                        int sortingOrder = 2 + y;
                        spriteRenderer.sortingOrder = sortingOrder;
                    }
                    
                    BlockView blockView = blockObject.GetComponent<BlockView>();
                    BlockColor randomColor = (BlockColor)Random.Range(0, (int)BlockColor.PINK + 1);
                    blockView.Initialize(randomColor, new Vector2Int(x, y));
                    blockView.OnBlockClicked.AddListener(OnBlockViewClicked);
                    
                    _blockViews[x, y] = blockView;
                }
            }
        }

        public void UpdateGrid(IGridService gridService)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    int colorIndex = gridService.Grid[x, y];
                    if (colorIndex >= 0 && colorIndex < (int)BlockColor.PINK + 1)
                    {
                        BlockColor blockColor = (BlockColor)colorIndex;
                        _blockViews[x, y].UpdateColor(blockColor);
                        _blockViews[x, y].gameObject.SetActive(true);
                    }
                    else
                    {
                        _blockViews[x, y].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void OnBlockViewClicked(BlockView clickedBlock)
        {
            OnBlockClicked?.Invoke(clickedBlock.GridPosition);
        }
    }
}
