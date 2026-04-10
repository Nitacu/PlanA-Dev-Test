using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using PuzzleGame.Core.Models;
using PuzzleGame.Unity.Views;

namespace PuzzleGame.Tests.EditMode.Views
{
    [TestFixture]
    public class BlockViewTests
    {
        private GameObject _testGameObject;
        private BlockView _blockView;

        [SetUp]
        public void SetUp()
        {
            // Create test GameObject and components
            _testGameObject = new GameObject("TestBlock");
            _blockView = _testGameObject.AddComponent<BlockView>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_testGameObject != null)
                Object.DestroyImmediate(_testGameObject);
        }

        [Test]
        public void Initialize_SetsCorrectBlockColorAndPosition()
        {
            // Arrange
            BlockColor expectedColor = BlockColor.GREEN;
            Vector2Int expectedPosition = new Vector2Int(2, 3);

            // Act
            _blockView.Initialize(expectedColor, expectedPosition);

            // Assert
            Assert.AreEqual(expectedColor, _blockView.BlockColor, "BlockColor should be set correctly");
            Assert.AreEqual(expectedPosition, _blockView.GridPosition, "GridPosition should be set correctly");
        }
    }
}
