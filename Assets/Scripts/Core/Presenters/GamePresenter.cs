using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PuzzleGame.Core.Models;
using PuzzleGame.Core.Services;
using PuzzleGame.Unity.Views;
using UnityEngine;
using VContainer.Unity;

namespace PuzzleGame.Core.Presenters
{
    public class GamePresenter : IStartable, IDisposable
    {
        private readonly IGameState _gameState;
        private readonly IGridService _gridService;
        private readonly GameView _gameView;
        
        private const int GAME_COLORS_AMOUNT = 4;
        private bool _isProcessingTurn;

        public GamePresenter(IGameState gameState, IGridService gridService, GameView gameView)
        {
            _gameState = gameState;
            _gridService = gridService;
            _gameView = gameView;
        }

        public void Start()
        {
            _gameView.ReplayButton.onClick.AddListener(OnReplayClicked);
            InitializeGame();
        }

        private void InitializeGame()
        {
            _isProcessingTurn = false;
            _gameState.ResetState();
            _gridService.GenerateGrid(GAME_COLORS_AMOUNT);
            
            _gameView.ShowGameOver(false);
            UpdateView();
        }

        /// <summary>
        /// Task 3 specific logic: Collects blocks via flood fill, handles async gravity delay.
        /// Should be hooked directly to the UI block Tap/Click events.
        /// </summary>
        public async void OnBlockClicked(int x, int y)
        {
            // Validate move state to avoid issues clicking block while gravity takes place
            if (_gameState.Moves <= 0 || _isProcessingTurn) return;

            List<Vector2Int> connectedBlocks = _gridService.GetConnectedBlocks(x, y);
            
            // Require at least 2 connected block matching color (optional rule)
            if (connectedBlocks.Count >= 1) 
            {
                _isProcessingTurn = true;

                _gameState.AddScore(connectedBlocks.Count);
                _gridService.RemoveBlocks(connectedBlocks);
                
                UpdateView(); 
                
                // Wait 1 second before applying gravity execution
                await Task.Delay(1000);

                _gridService.ApplyGravity();
                _gridService.RefillGrid(GAME_COLORS_AMOUNT);

                _gameState.UseMove();
                UpdateView();

                if (_gameState.Moves <= 0)
                {
                    _gameView.ShowGameOver(true);
                }

                _isProcessingTurn = false;
            }
        }

        private void UpdateView()
        {
            _gameView.UpdateScore(_gameState.Score);
            _gameView.UpdateMoves(_gameState.Moves);
        }

        private void OnReplayClicked()
        {
            InitializeGame();
        }

        public void Dispose()
        {
            if (_gameView != null && _gameView.ReplayButton != null)
                _gameView.ReplayButton.onClick.RemoveListener(OnReplayClicked);
        }
    }
}
