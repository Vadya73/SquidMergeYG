using System;
using Audio;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Game.CodeBase
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpawnObject : MonoBehaviour
    {
        [SerializeField, ReadOnly] private ObjectConfig _config;
        [SerializeField] private GameObject _selectObjectBacklight;
        private MergeGameSystem _mergeGameSystem;
        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider;

        private bool _isCollided = false;
        private bool _isSelected = false;
        private AudioManager _audioManager;
        private AudioData _audioData;
        public event Action<SpawnObject> OnObjectClicked;
        
        public ObjectConfig Config => _config;
        public bool IsSelected => _isSelected;
        public Rigidbody2D Rb2D => _rigidbody2D;

        [Inject]
        private void Construct(AudioManager audioManager, AudioData audioData, MergeGameSystem mergeGameSystem)
        {
            _audioManager = audioManager;
            _audioData = audioData;
            _mergeGameSystem = mergeGameSystem;
            
        }
        
        private void Awake()
        {
            if (_rigidbody2D == null)
                _rigidbody2D = GetComponent<Rigidbody2D>();

            if (_collider == null)
                _collider = GetComponent<Collider2D>();
            
            _selectObjectBacklight.SetActive(false);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isCollided)
                return;

            if (!_audioManager.SoundSource.isPlaying)
            {
                _audioManager.PlaySound(_audioData.ObjectCollideSound);
            }
            
            if (other.gameObject.TryGetComponent(out SpawnObject spawnObject))
            {
                if (_config.ObjectType == spawnObject.Config.ObjectType)
                {
                    var newSpawnPos = (spawnObject.transform.position + transform.position) * .5f;
                    
                    SetCollided(true);
                    spawnObject.SetCollided(true);
                    
                    DeactivateObject();
                    spawnObject.DeactivateObject();

                    AnimateMoveTo(newSpawnPos);
                    spawnObject.AnimateMoveTo(newSpawnPos);
                    DOVirtual.DelayedCall(.25f, () =>
                    {
                        DestroyObject();
                        spawnObject.DestroyObject();

                        _mergeGameSystem.SpawnNextLevelObject(_config, newSpawnPos);
                    });
                }
            }
        }

        private void AnimateMoveTo(Vector3 newSpawnPos)
        {
            transform.DOMove(newSpawnPos, 0.25f);
        }

        private void OnMouseDown()
        {
            OnObjectClicked?.Invoke(this);
        }

        public void SetConfig(ObjectConfig objectConfig) => _config = objectConfig;

        public void ActivateObject()
        {
            _rigidbody2D.simulated = true;
            _collider.enabled = true;
        }

        public void DeactivateObject()
        {
            _rigidbody2D.simulated = false;
            _collider.enabled = false;
        }
        
        private void SetCollided(bool b) => _isCollided = b;

        public void DestroyObject()
        {
            _mergeGameSystem.DeleteSpawnObjectFromList(this);

            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, .5f))
                .OnComplete(() => Destroy(this.gameObject));
        }

        public void SetSelected(bool b)
        {
            _isSelected = b;
            _selectObjectBacklight.SetActive(b);
            _audioManager.PlaySound(_audioData.SelectObjectSound);
        }
    }
}