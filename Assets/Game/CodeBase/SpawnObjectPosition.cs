using Audio;
using DG.Tweening;
using Game.CodeBase;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class SpawnObjectPosition : MonoBehaviour
{
    [SerializeField] private Transform _objectsHolder;

    private MergeGameSystem _mergeGameSystem;
    private SpawnObject _currentObject; 
    private LineRenderer _lineRenderer;
    private bool _isDragging = false;
    private bool _inputActive = false;
    private AudioManager _audioManager;
    private AudioData _audioData;

    [Inject]
    private void Construct(AudioManager audioManager,AudioData audioData, MergeGameSystem mergeGameSystem)
    {
        _audioManager = audioManager;
        _audioData = audioData;
        _mergeGameSystem = mergeGameSystem;
    }
        
    private void Update()
    {
        if (_currentObject == null)
            return;
            
        HandleInput();
    }

    public void SetObject(SpawnObject prefab)
    {
        _currentObject = prefab;
        prefab.transform.localScale = Vector3.zero;
        prefab.transform.position = new Vector3(prefab.transform.position.x, prefab.transform.position.y, -1f);
        DOTween.Sequence()
            .AppendInterval(0.2f)
            .Append(prefab.transform.DOScale(Vector3.one, .5f))
            .OnComplete(() => _inputActive = true);
    }

    public void SetInputActive(bool active) => _inputActive = active;

    private void HandleInput()
    {
        if (!_inputActive)
            return;
            
        bool isTouching = Input.touchCount > 0;
    
        if (isTouching)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
                
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                _isDragging = true;
                MoveObject(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                OnInputReleased();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) 
                return;
                
            _isDragging = true;
            MoveObject(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnInputReleased();
        }
        else if (_isDragging)
        {
            OnInputReleased();
        }
    }

    private void OnInputReleased()
    {
        if (!_isDragging) return;

        _inputActive = false;
    
        _isDragging = false;
        if (_currentObject != null)
        {
            _currentObject.transform.position = new Vector3(_currentObject.transform.position.x, _currentObject.transform.position.y, transform.position.z);
            _audioManager.PlaySound(_audioData.InputReleaseSound);
            _mergeGameSystem.AddSpawnObjectToList(_currentObject);
            _currentObject.ActivateObject();
            _currentObject.transform.SetParent(_objectsHolder);
            _currentObject = null;
            _mergeGameSystem.SetNextObject();
        }
    }

    private void MoveObject(Vector2 screenPosition)
    {
        if (!_isDragging) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPos.z = transform.position.z;

        float clampedX = Mathf.Clamp(
            worldPos.x,
            _mergeGameSystem.MinPos.position.x,
            _mergeGameSystem.MaxPos.position.x
        );

        transform.position = new Vector3(clampedX, transform.position.y, worldPos.z);
    }
}