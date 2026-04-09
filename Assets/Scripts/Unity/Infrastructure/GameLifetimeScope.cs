using PuzzleGame.Core.Models;
using PuzzleGame.Core.Presenters;
using PuzzleGame.Core.Services;
using PuzzleGame.Unity.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace PuzzleGame.Unity.Infrastructure
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameView gameView;

        protected override void Configure(IContainerBuilder builder)
        {
            // 1. Models
            builder.Register<IGameState, GameState>(Lifetime.Singleton);
            
            // 2. Services (Introduced in Task 3)
            builder.Register<IGridService, GridService>(Lifetime.Singleton);
            
            // 3. Views
            builder.RegisterComponent(gameView);
            
            // 4. Presenters
            builder.RegisterEntryPoint<GamePresenter>();
        }
    }
}
