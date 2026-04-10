using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame.Core.Services
{
    /// <summary>
    /// GridService implements core puzzle logic - pure algorithms without Unity dependencies.
    /// 
    /// Architecture Decision: Interface for testability
    /// - Why IGridService: Enables mocking in unit tests, supports different implementations
    /// - Why not static class: Dependency injection, state management, testability
    /// - Supports: Clean architecture, unit testing, future algorithm variations
    /// 
    /// Architecture Decision: 2D array for grid representation
    /// - Why 2D array: Direct mapping to visual grid, excellent performance
    /// - Why not List<List<int>>: Memory overhead, unnecessary complexity
    /// - Why not custom Grid class: 2D array provides all needed functionality
    /// - Supports: Performance, simplicity, direct coordinate access
    /// </summary>
    public interface IGridService
    {
        int[,] Grid { get; }
        void GenerateGrid(int numColors);
        List<Vector2Int> GetConnectedBlocks(int x, int y);
        void RemoveBlocks(List<Vector2Int> blocks);
        void ApplyGravity();
        void RefillGrid(int numColors);
    }

    public class GridService : IGridService
    {
        private const int GRID_WIDTH = 5;
        private const int GRID_HEIGHT = 6;
        
        // 2D array grid representation - architectural decision for performance
// Benefits: O(1) access by coordinates, memory efficient, cache-friendly
// Alternative considered: Dictionary<Vector2Int, int> - rejected for performance
// Supports: Direct visual mapping, flood fill algorithm efficiency
        public int[,] Grid { get; private set; }

        public GridService()
        {
            Grid = new int[GRID_WIDTH, GRID_HEIGHT];
        }

        public void GenerateGrid(int numColors)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    Grid[x, y] = Random.Range(0, numColors);
                }
            }
        }

        public List<Vector2Int> GetConnectedBlocks(int startX, int startY)
        {
            List<Vector2Int> connectedBlocks = new List<Vector2Int>();
            if (!IsValidPosition(startX, startY)) return connectedBlocks;

            int targetColor = Grid[startX, startY];
            bool[,] visited = new bool[GRID_WIDTH, GRID_HEIGHT];

            FloodFillRecursive(startX, startY, targetColor, visited, connectedBlocks);
            return connectedBlocks;
        }

        private void FloodFillRecursive(int x, int y, int targetColor, bool[,] visited, List<Vector2Int> connectedBlocks)
        {
            if (!IsValidPosition(x, y) || visited[x, y] || Grid[x, y] != targetColor) return;

            visited[x, y] = true;
            connectedBlocks.Add(new Vector2Int(x, y));

            FloodFillRecursive(x, y + 1, targetColor, visited, connectedBlocks); // Up
            FloodFillRecursive(x, y - 1, targetColor, visited, connectedBlocks); // Down
            FloodFillRecursive(x - 1, y, targetColor, visited, connectedBlocks); // Left
            FloodFillRecursive(x + 1, y, targetColor, visited, connectedBlocks); // Right
        }

        public void RemoveBlocks(List<Vector2Int> blocks)
        {
            foreach (var pos in blocks) Grid[pos.x, pos.y] = -1; // -1 represents empty
        }

        public void ApplyGravity()
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                int emptyCount = 0;
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    if (Grid[x, y] == -1) emptyCount++;
                    else if (emptyCount > 0)
                    {
                        Grid[x, y - emptyCount] = Grid[x, y];
                        Grid[x, y] = -1;
                    }
                }
            }
        }

        public void RefillGrid(int numColors)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                for (int y = 0; y < GRID_HEIGHT; y++)
                {
                    if (Grid[x, y] == -1) Grid[x, y] = Random.Range(0, numColors);
                }
            }
        }

        private bool IsValidPosition(int x, int y) => x >= 0 && x < GRID_WIDTH && y >= 0 && y < GRID_HEIGHT;
    }
}
