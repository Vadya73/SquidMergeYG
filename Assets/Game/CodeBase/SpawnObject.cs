using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

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
        public event Action<SpawnObject> OnObjectClicked;
        
        public ObjectConfig Config => _config;
        public bool IsSelected => _isSelected;

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
            
            if (other.gameObject.TryGetComponent(out SpawnObject spawnObject))
            {
                if (_config.ObjectType == spawnObject.Config.ObjectType)
                {
                    Debug.Log("Collided");
                    SetCollided(true);
                    spawnObject.SetCollided(true);
                    
                    DeactivateObject();
                    spawnObject.DeactivateObject();

                    DestroyObject();
                    spawnObject.DestroyObject();

                    _mergeGameSystem.SpawnNextLevelObject(_config, transform.position);
                }
            }
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

        public void SetMergeSystem(MergeGameSystem mergeGameSystem)
        {
            _mergeGameSystem = mergeGameSystem;
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
        }
    }
}