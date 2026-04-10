using NUnit.Framework;
using PuzzleGame.Core.Models;

namespace PuzzleGame.Tests.EditMode.Models
{
    [TestFixture]
    public class GameStateTests
    {
        private GameState _gameState;

        [SetUp]
        public void SetUp()
        {
            _gameState = new GameState();
        }

        [Test]
        public void Constructor_InitializesWithDefaultValues()
        {
            // Act
            GameState gameState = new GameState();

            // Assert
            Assert.AreEqual(0, gameState.Score, "Score should start at 0");
            Assert.AreEqual(5, gameState.Moves, "Moves should start at INITIAL_MOVES");
        }

        [Test]
        public void AddScore_ValidBlocksDestroyed_IncreasesScoreCorrectly()
        {
            // Arrange
            GameState gameState = new GameState();
            
            // Act
            gameState.AddScore(3);

            // Assert
            // Formula: 3 * (3 + 1) / 2 = 6
            Assert.AreEqual(6, gameState.Score, "Score should use triangular number formula");
        }

        [Test]
        public void AddScore_ZeroOrNegativeBlocks_DoesNotChangeScore()
        {
            // Arrange
            GameState gameState = new GameState();
            int initialScore = gameState.Score;

            // Act
            gameState.AddScore(0);
            gameState.AddScore(-1);

            // Assert
            Assert.AreEqual(initialScore, gameState.Score, "Score should not change for invalid input");
        }

        [Test]
        public void UseMove_DecreasesMovesWhenPositive()
        {
            // Arrange
            GameState gameState = new GameState();
            int initialMoves = gameState.Moves;

            // Act
            gameState.UseMove();

            // Assert
            Assert.AreEqual(initialMoves - 1, gameState.Moves, "Moves should decrease by 1");
        }

        [Test]
        public void UseMove_ZeroMoves_DoesNotGoNegative()
        {
            // Arrange
            GameState gameState = new GameState();
            
            // Use all moves
            for (int i = 0; i < 5; i++) gameState.UseMove();

            // Act
            gameState.UseMove(); // Try to use one more

            // Assert
            Assert.AreEqual(0, gameState.Moves, "Moves should not go below 0");
        }

        [Test]
        public void ResetState_ResetsToInitialValues()
        {
            // Arrange
            GameState gameState = new GameState();
            gameState.AddScore(10);
            gameState.UseMove();

            // Act
            gameState.ResetState();

            // Assert
            Assert.AreEqual(0, gameState.Score, "Score should reset to 0");
            Assert.AreEqual(5, gameState.Moves, "Moves should reset to INITIAL_MOVES");
        }
    }
}
