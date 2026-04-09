using System;
using PuzzleGame.Core.Models;
using PuzzleGame.Unity.Views;
using VContainer.Unity;

namespace PuzzleGame.Core.Presenters
{
    public class GamePresenter : IStartable, IDisposable
    {
        private readonly IGameState _gameState;
        private readonly GameView _gameView;
        
        private bool _isProcessingTurn;
        private const int TEST_SCORE_INCREMENT = 10;

        public GamePresenter(IGameState gameState, GameView gameView)
        {
            _gameState = gameState;
            _gameView = gameView;
        }

        public void Start()
        {
            _gameView.ReplayButton.onClick.AddListener(OnReplayClicked);
            _gameView.MakeMoveTestButton.onClick.AddListener(OnMakeMoveTestClicked);

            InitializeGame();
        }

        private void InitializeGame()
        {
            _gameState?.ResetState();
            _gameView?.ShowGameOver(false);
            _gameView?.EnableTestButton(true);
            UpdateView();
        }

        /// <summary>
        /// Task 2 specific logic: Decrease move by 1, increase score by 10.
        /// </summary>
        private void OnMakeMoveTestClicked()
        {
            if (_gameState?.Moves <= 0) return;

            _gameState.AddScore(TEST_SCORE_INCREMENT);
            _gameState.UseMove();
            UpdateView();

            if (_gameState.Moves <= 0)
            {
                _gameView?.ShowGameOver(true);
                _gameView?.EnableTestButton(false);
            }
        }

        private void UpdateView()
        {
            if (_gameView != null && _gameState != null)
            {
                _gameView.UpdateScore(_gameState.Score);
                _gameView.UpdateMoves(_gameState.Moves);
            }
        }

        private void OnReplayClicked()
        {
            InitializeGame();
        }

        public void Dispose()
        {
            if (_gameView != null)
            {
                if (_gameView.ReplayButton != null) _gameView.ReplayButton.onClick.RemoveListener(OnReplayClicked);
                if (_gameView.MakeMoveTestButton != null) _gameView.MakeMoveTestButton.onClick.RemoveListener(OnMakeMoveTestClicked);
            }
        }
    }
}
