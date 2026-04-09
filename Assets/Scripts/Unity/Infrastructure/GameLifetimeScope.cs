using PuzzleGame.Core.Models;
using PuzzleGame.Core.Presenters;
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
            
            // 2. Views
            builder.RegisterComponent(gameView);
            
            // 3. Presenters
            builder.RegisterEntryPoint<GamePresenter>();
        }
    }
}
