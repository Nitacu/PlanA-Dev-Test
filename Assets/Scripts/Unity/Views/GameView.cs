using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PuzzleGame.Unity.Views
{
    public class GameView : MonoBehaviour
    {
        [Header("HUD")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _movesText;

        [Header("Test Controls (Task 2)")]
        [SerializeField] private Button _makeMoveTestButton;

        [Header("Game Over")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Button _replayButton;

        public Button MakeMoveTestButton => _makeMoveTestButton;
        public Button ReplayButton => _replayButton;

        public void UpdateScore(int score) => _scoreText.text = $"{score}";
        public void UpdateMoves(int moves) => _movesText.text = $"{moves}";
        public void ShowGameOver(bool show) => _gameOverPanel.SetActive(show);
        
        public void EnableTestButton(bool enable) => _makeMoveTestButton.interactable = enable;
        
        private void Awake()
        {
            // Validate required components
            if (_scoreText == null) Debug.LogError("Score Text is not assigned!");
            if (_movesText == null) Debug.LogError("Moves Text is not assigned!");
            if (_makeMoveTestButton == null) Debug.LogError("Make Move Test Button is not assigned!");
            if (_gameOverPanel == null) Debug.LogError("Game Over Panel is not assigned!");
            if (_replayButton == null) Debug.LogError("Replay Button is not assigned!");
        }
    }
}
