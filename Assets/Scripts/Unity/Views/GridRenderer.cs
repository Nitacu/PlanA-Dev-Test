using System.Collections.Generic;
using UnityEngine;
using PuzzleGame.Core.Models;
using PuzzleGame.Core.Services;

namespace PuzzleGame.Unity.Views
{
    public class GridRenderer : MonoBehaviour
    {
        [Header("Grid Configuration")]
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private Transform _gridParent;
        
        private const int GRID_WIDTH = 6;
        private const int GRID_HEIGHT = 5;
        private const float CELL_WIDTH = 1.28f; // 128px at 100px/unit
        private const float CELL_HEIGHT = 1.12f; // 112px at 100px/unit
        private const float BLOCK_SIZE = 1.28f; // 128px at 100px/unit
        
        private BlockView[,] _blockViews;

        public UnityEvent<Vector2Int> OnBlockClicked = new UnityEvent<Vector2Int>();

        private void Awake()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            _blockViews = new BlockView[GRID_WIDTH, GRID_HEIGHT];
            
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    Vector3 position = new Vector3(x * CELL_WIDTH, y * CELL_HEIGHT, 0);
                    GameObject blockObject = Instantiate(_blockPrefab, position, Quaternion.identity, _gridParent);
                    
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
