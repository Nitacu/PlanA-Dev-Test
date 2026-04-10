using NUnit.Framework;
using UnityEngine;
using PuzzleGame.Core.Services;

namespace PuzzleGame.Tests.EditMode.Services
{
    [TestFixture]
    public class GridServiceTests
    {
        private GridService _gridService;

        [SetUp]
        public void SetUp()
        {
            _gridService = new GridService();
        }

        [Test]
        public void Constructor_CreatesCorrectGridDimensions()
        {
            // Act
            GridService gridService = new GridService();

            // Assert
            Assert.AreEqual(5, gridService.Grid.GetLength(0), "Grid width should be 5");
            Assert.AreEqual(6, gridService.Grid.GetLength(1), "Grid height should be 6");
        }

        [Test]
        public void RemoveBlocks_SetsPositionsToNegativeOne()
        {
            // Arrange
            GridService gridService = new GridService();
            gridService.GenerateGrid(3);
            System.Collections.Generic.List<Vector2Int> blocksToRemove = new System.Collections.Generic.List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(2, 3) };

            // Act
            gridService.RemoveBlocks(blocksToRemove);

            // Assert
            Assert.AreEqual(-1, gridService.Grid[0, 0], "Removed block should be -1");
            Assert.AreEqual(-1, gridService.Grid[2, 3], "Removed block should be -1");
            Assert.AreNotEqual(-1, gridService.Grid[1, 1], "Non-removed block should not be -1");
        }

        [Test]
        public void GetConnectedBlocks_InvalidPosition_ReturnsEmptyList()
        {
            // Arrange
            GridService gridService = new GridService();

            // Act
            System.Collections.Generic.List<Vector2Int> result1 = gridService.GetConnectedBlocks(-1, 0);
            System.Collections.Generic.List<Vector2Int> result2 = gridService.GetConnectedBlocks(0, -1);
            System.Collections.Generic.List<Vector2Int> result3 = gridService.GetConnectedBlocks(5, 0);
            System.Collections.Generic.List<Vector2Int> result4 = gridService.GetConnectedBlocks(0, 6);

            // Assert
            Assert.AreEqual(0, result1.Count, "Invalid X should return empty list");
            Assert.AreEqual(0, result2.Count, "Invalid Y should return empty list");
            Assert.AreEqual(0, result3.Count, "Out of bounds X should return empty list");
            Assert.AreEqual(0, result4.Count, "Out of bounds Y should return empty list");
        }
    }
}
