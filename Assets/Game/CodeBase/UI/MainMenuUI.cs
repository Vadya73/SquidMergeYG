using Audio;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _ratingButton;
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _closeRatingButton;
        [Header("Other")]
        [SerializeField] private GameObject _ratingObject;
        [SerializeField] private Image _soundImage;

        private LoadScreen _loadScreen;
        private AudioManager _audioManager;
        private AudioData _audioData;

        [Inject]
        private void Construct(LoadScreen loadScreen, AudioManager audioManager, AudioData audioData)
        {
            _loadScreen = loadScreen;
            _audioManager = audioManager;
            _audioData = audioData;
        }
        
        private void Awake()
        {
            _startButton.onClick.AddListener(StartGame);
            _ratingButton.onClick.AddListener(ShowRating);
            _soundButton.onClick.AddListener(SwitchSoundActive);
            _closeRatingButton.onClick.AddListener(CloseRating);
            
            _ratingObject.SetActive(false);

            
            _soundImage.sprite = _audioData.SoundActive ? _audioData.SoundActiveSprite : _audioData.SoundInactiveSprite;
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(StartGame);
            _ratingButton.onClick.RemoveListener(ShowRating);
            _soundButton.onClick.RemoveListener(SwitchSoundActive);
        }

        private void StartGame()
        {
            _loadScreen.LoadScene(2);
        }

        private void SwitchSoundActive()
        {
            _audioManager.SwitchSound();

            _soundImage.sprite = _audioData.SoundActive ? _audioData.SoundActiveSprite : _audioData.SoundInactiveSprite;
        }

        private void ShowRating()
        {
            _ratingObject.SetActive(true);
        }

        private void CloseRating()
        {
            _ratingObject.SetActive(false);
        }
    }
}
