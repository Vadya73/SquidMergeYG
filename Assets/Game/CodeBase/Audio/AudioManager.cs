using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using YG;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField, ChildGameObjectsOnly] private AudioSource _soundSource;
        [SerializeField, ChildGameObjectsOnly] private AudioSource _musicSource;
        [SerializeField, ReadOnly] private AudioData _audioData;
        
        public AudioSource SoundSource => _soundSource;

        [Inject]
        private void Construct(AudioData audioData)
        {
            _audioData = audioData;
        }

        private void Start()
        {
            _audioData.SetSoundActive(YG2.saves.SoundActive);

            _soundSource.mute = !_audioData.SoundActive;
            _musicSource.mute = !_audioData.SoundActive;
        }

        public void SwitchSound()
        {
            _audioData.SwitchSoundActive();
            
            _soundSource.mute = !_audioData.SoundActive;
            _musicSource.mute = !_audioData.SoundActive;
        }

        public void PlaySound(AudioClip clip)
        {
            if (!clip)
                return;
            
            _soundSource.PlayOneShot(clip);
        }
    }
}