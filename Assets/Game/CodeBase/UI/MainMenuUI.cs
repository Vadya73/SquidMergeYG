using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.CodeBase.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settingsButton;
        [Header("Other")]
        [SerializeField] private GameObject _settingsObject;

        private void Awake()
        {
            _startButton.onClick.AddListener(StartGame);
            _settingsButton.onClick.AddListener(ShowSettings);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(StartGame);
            _settingsButton.onClick.RemoveListener(ShowSettings);
        }

        private void StartGame()
        {
            LoadScreen.Instance.Show(() => SceneManager.LoadSceneAsync(sceneBuildIndex: 1));
        }

        private void ShowSettings()
        {
            
        }
    }
}
