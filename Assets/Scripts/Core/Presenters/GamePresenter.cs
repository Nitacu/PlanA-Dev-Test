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
    /// <summary>
    /// GamePresenter implements the Presenter in MVP pattern, coordinating between Model and View.
    /// 
    /// Architecture Decision: Constructor injection via VContainer
    /// - Why constructor injection: Ensures all dependencies are available at initialization
    /// - Why VContainer: Unity-native DI container with proper lifetime management
    /// - Supports: Testability, loose coupling, automatic dependency resolution
    /// 
    /// Architecture Decision: Async operations with Task.Delay
    /// - Why async: Prevents UI blocking during gravity/refill animations
    /// - Why not coroutines: Task-based async integrates better with modern C# patterns
    /// - Supports: Smooth gameplay experience, proper animation timing
    /// </summary>
    public class GamePresenter : IStartable, IDisposable
    {
        private readonly IGameState _gameState;
        private readonly IGridService _gridService;
        private readonly GameView _gameView;
        
        private const int GAME_COLORS_AMOUNT = 5;
        private bool _isProcessingTurn;

        // VContainer dependency injection - architectural choice for loose coupling
// Benefits: Automatic dependency resolution, proper lifetime management
// Testability: Mocks can be injected for unit testing
// Alternative considered: Service Locator pattern - rejected for explicit dependencies
        public GamePresenter(IGameState gameState, IGridService gridService, GameView gameView)
        {
            _gameState = gameState;
            _gridService = gridService;
            _gameView = gameView;
        }

        public void Start()
        {
            // Unity Event subscription - architectural pattern for view communication
// Why events: Decouples presenter from view implementation details
// Supports: Multiple view types, runtime flexibility, testability
// Note: Proper cleanup in Dispose() prevents memory leaks
            _gameView.ReplayButton.onClick.AddListener(OnReplayClicked);
            _gameView.GridRenderer.OnBlockClicked.AddListener(OnBlockClicked);
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
        public async void OnBlockClicked(Vector2Int gridPosition)
        {
            // Validate move state to avoid issues clicking block while gravity takes place
            if (_gameState.Moves <= 0 || _isProcessingTurn) return;

            List<Vector2Int> connectedBlocks = _gridService.GetConnectedBlocks(gridPosition.x, gridPosition.y);
            
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
            _gameView.GridRenderer.UpdateGrid(_gridService);
        }

        private void OnReplayClicked()
        {
            InitializeGame();
        }

        public void Dispose()
        {
            if (_gameView != null)
            {
                if (_gameView.ReplayButton != null) 
                    _gameView.ReplayButton.onClick.RemoveListener(OnReplayClicked);
                if (_gameView.GridRenderer != null) 
                    _gameView.GridRenderer.OnBlockClicked.RemoveListener(OnBlockClicked);
            }
        }
    }
}
