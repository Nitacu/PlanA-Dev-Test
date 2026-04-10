using PuzzleGame.Core.Models;
using PuzzleGame.Core.Presenters;
using PuzzleGame.Core.Services;
using PuzzleGame.Unity.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace PuzzleGame.Unity.Infrastructure
{
    /// <summary>
    /// VContainer configuration root - architectural foundation for dependency injection.
    /// 
    /// Architecture Decision: VContainer over other DI frameworks
    /// - Why VContainer: Unity-native, proper lifetime management, performance optimized
    /// - Why not Zenject/VContainer older versions: Modern API, better Unity integration
    /// - Supports: Automatic dependency resolution, proper disposal, testability
    /// 
    /// Architecture Decision: Component registration for Unity objects
    /// - Why RegisterComponent: Handles Unity lifecycle automatically
    /// - Why not RegisterInstance: Components may need Unity-specific initialization
    /// - Supports: Proper MonoBehaviour lifecycle, scene management
    /// </summary>
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameView gameView;

        protected override void Configure(IContainerBuilder builder)
        {
            // VContainer registration strategy - architectural dependency graph
// Models: Singleton lifetime - shared game state across application
// Services: Singleton lifetime - consistent grid logic
// Views: Component lifetime - tied to Unity GameObject lifecycle
// Presenters: Entry point lifetime - managed by Unity's PlayMode
            // 1. Models
            builder.Register<IGameState, GameState>(Lifetime.Singleton);
            
            // 2. Services (Introduced in Task 3)
            builder.Register<IGridService, GridService>(Lifetime.Singleton);
            
            // 3. Views
            builder.RegisterComponent(gameView);
            builder.RegisterComponent(gameView.GridRenderer);
            
            // 4. Presenters
            builder.RegisterEntryPoint<GamePresenter>();
        }
    }
}
