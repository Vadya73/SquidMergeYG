using Audio;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Infrastructure
{
    public class ButtonSoundSetter : MonoBehaviour
    {
        private AudioManager _audioManager;
        private AudioData _audioData;
    
        private Button[] _allButtons;

        [Inject]
        private void Construct(AudioManager audioManager, AudioData audioData)
        {
            _audioManager = audioManager;
            _audioData = audioData;
        }

        private void Awake()
        {
            _allButtons = FindObjectsByType<Button>(sortMode: FindObjectsSortMode.None);

            for (var index = 0; index < _allButtons.Length; index++)
            {
                var button = _allButtons[index];
                button.onClick.AddListener(PlayButtonSound);
            }
        }

        private void OnDestroy()
        {
            for (var index = 0; index < _allButtons.Length; index++)
            {
                var button = _allButtons[index];
                button.onClick.RemoveListener(PlayButtonSound);
            }
        }

        private void PlayButtonSound()
        {
            _audioManager.PlaySound(_audioData.ButtonClickSound);
        }
    }
}