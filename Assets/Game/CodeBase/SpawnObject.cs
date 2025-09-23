using DG.Tweening;
using UnityEngine;

namespace Game.CodeBase
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpawnObject : MonoBehaviour
    {
        [SerializeField] private ObjectConfig _config;
        [SerializeField] private MergeGameSystem _mergeGameSystem;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Collider2D _collider;

        private bool _isCollided = false;
        
        public ObjectConfig Config => _config;

        private void Awake()
        {
            if (_rigidbody2D == null)
                _rigidbody2D = GetComponent<Rigidbody2D>();

            if (_collider == null)
                _collider = GetComponent<Collider2D>();
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

        private void SetCollided(bool b) => _isCollided = b;

        public void DestroyObject()
        {
            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, .5f))
                .OnComplete(() => Destroy(this.gameObject));
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
    }
}