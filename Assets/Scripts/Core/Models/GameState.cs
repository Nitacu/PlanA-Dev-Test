using System;

namespace PuzzleGame.Core.Models
{
    public interface IGameState
    {
        int Score { get; }
        int Moves { get; }
        void AddScore(int points);
        void UseMove();
        void ResetState();
    }

    public class GameState : IGameState
    {
        public int Score { get; private set; }
        public int Moves { get; private set; }
        
        private const int INITIAL_MOVES = 5;
        private const int TEST_SCORE_INCREMENT = 10;

        public GameState()
        {
            ResetState();
        }

        public void AddScore(int points)
        {
            Score += points;
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
