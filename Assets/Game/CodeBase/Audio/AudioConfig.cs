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
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _inputReleaseSound;
        [SerializeField] private AudioClip _buttonClickSound;
        [SerializeField] private AudioClip[] _objectsMergeSound;
        [SerializeField] private AudioClip _objectCollideSound;
        [SerializeField] private AudioClip _endLevelSound;
        [SerializeField] private AudioClip _bombSound;
        [SerializeField] private AudioClip _swapTwoObjectsSound;
        [SerializeField] private AudioClip _mixObjectsSound;
        [SerializeField] private AudioClip _deleteObjectSound;
        [SerializeField] private AudioClip _selectObjectSound;
        
        public Sprite SoundActiveSprite => _soundActiveSprite;
        public Sprite SoundInactiveSprite => _soundInactiveSprite;
        public bool SoundActive => _soundActive;
        
        public AudioClip ClickSound => _clickSound;
        public AudioClip InputReleaseSound => _inputReleaseSound;
        public AudioClip ButtonClickSound => _buttonClickSound;
        public AudioClip[] ObjectsMergeSound => _objectsMergeSound;
        public AudioClip ObjectCollideSound => _objectCollideSound;
        public AudioClip EndLevelSound => _endLevelSound;
        public AudioClip BombSound => _bombSound;
        public AudioClip SwapTwoObjectsSound => _swapTwoObjectsSound;
        public AudioClip MixObjectsSound => _mixObjectsSound;
        public AudioClip DeleteObjectSound => _deleteObjectSound;
        public AudioClip SelectObjectSound => _selectObjectSound;

        public void SwitchSoundActive()
        {
            _soundActive = !_soundActive;
        }
        
        public void SetSoundActive(bool active) => _soundActive = active;
    }
}