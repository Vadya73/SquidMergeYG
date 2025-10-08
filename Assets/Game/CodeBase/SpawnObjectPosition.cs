using Audio;
using DG.Tweening;
using Game.CodeBase;
using MyInput;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using System.Collections.Generic;

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
    private void Construct(AudioManager audioManager, AudioData audioData, MergeGameSystem mergeGameSystem)
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
        if (!_inputActive || !GameState.InputEnabled)
            return;

#if UNITY_WEBGL && !UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (IsPointerOverUI(Input.mousePosition))
                return;

            _isDragging = true;
            MoveObject(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnInputReleased();
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (IsPointerOverUI(touch.position))
                return;

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
            if (IsPointerOverUI(Input.mousePosition))
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
#endif
    }

    private void OnInputReleased()
    {
        if (!_isDragging) return;

        _inputActive = false;
        _isDragging = false;

        if (_currentObject != null)
        {
            _currentObject.transform.position = new Vector3(
                _currentObject.transform.position.x, 
                _currentObject.transform.position.y, 
                transform.position.z
            );

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
    
    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}
