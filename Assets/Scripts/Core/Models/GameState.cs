namespace PuzzleGame.Core.Models
{
    public interface IGameState
    {
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
