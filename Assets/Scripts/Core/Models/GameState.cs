namespace PuzzleGame.Core.Models
{
    /// <summary>
    /// GameState implements the Model in MVP pattern - pure game logic without Unity dependencies.
    /// 
    /// Architecture Decision: Interface segregation
    /// - Why IGameState: Enables testability, supports multiple implementations
    /// - Why not direct class usage: Mocking for unit tests, future extensibility
    /// - Supports: Clean architecture, dependency injection, testability
    /// 
    /// Architecture Decision: Immutable properties with method-based state changes
    /// - Why properties read-only: Prevents external state corruption
    /// - Why methods for changes: Encapsulates state transition logic
    /// - Supports: Predictable state management, debugging, validation
    /// </summary>
    public interface IGameState
    {
        // Interface-based design - architectural pattern for loose coupling
// Benefits: Testability with mocks, multiple implementations possible
// Supports: Dependency injection, clean architecture, future extensibility
// Note: All methods are pure logic - no Unity dependencies for testability
        int Score { get; }
        int Moves { get; }
        void AddScore(int blocksDestroyed);
        void UseMove();
        void ResetState();
    }

    public class GameState : IGameState
    {
        public int Score { get; private set; }
        public int Moves { get; private set; }
        
        private const int INITIAL_MOVES = 5;

        public GameState()
        {
            ResetState();
        }

        public void AddScore(int blocksDestroyed)
        {
            if (blocksDestroyed <= 0) return;
            
            // Formula for sum of 1 to N is: N * (N + 1) / 2
            int pointsEarned = (blocksDestroyed * (blocksDestroyed + 1)) / 2;
            Score += pointsEarned;
        }

        public void UseMove()
        {
            if (Moves > 0) Moves--;
        }

        public void ResetState()
        {
            Score = 0;
            Moves = INITIAL_MOVES;
        }
    }
}
