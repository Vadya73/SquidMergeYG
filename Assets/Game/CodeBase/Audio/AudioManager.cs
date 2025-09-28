using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField, ChildGameObjectsOnly] private AudioSource _soundSource;
        [SerializeField, ChildGameObjectsOnly] private AudioSource _musicSource;
        [SerializeField, ReadOnly] private AudioData _audioData;

        [Inject]
        private void Construct(AudioData audioData)
        {
            _audioData = audioData;
        }
        
        public void SwitchSound()
        {
            _audioData.SwitchSoundActive();
            
            _soundSource.mute = !_audioData.SoundActive;
            _musicSource.mute = !_audioData.SoundActive;
        }
    }
}