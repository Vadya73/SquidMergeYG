using System;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Audio/AudioConfig", fileName = "AudioConfig", order = 0)]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] private AudioData _audioData;
        
        public AudioData AudioData => _audioData;
    }
    
    [Serializable]
    public class AudioData
    {
        [SerializeField] private Sprite _soundActiveSprite;
        [SerializeField] private Sprite _soundInactiveSprite;
        [SerializeField] private bool _soundActive;
        
        public Sprite SoundActiveSprite => _soundActiveSprite;
        public Sprite SoundInactiveSprite => _soundInactiveSprite;
        public bool SoundActive => _soundActive;

        public void SwitchSoundActive()
        {
            _soundActive = !_soundActive;
        }
    }
}