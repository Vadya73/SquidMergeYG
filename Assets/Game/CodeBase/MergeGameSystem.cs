using System.Collections.Generic;
using Game.CodeBase;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class MergeGameSystem : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private SpawnObjectPosition _spawnObjectPositionComponent;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Transform _objectsHolder;
    [SerializeField] private Transform _minPos;
    [SerializeField] private Transform _maxPos;
    [SerializeField] private Image _nextObjectSprite;
    [SerializeField] private TMP_Text _pointText;
    [SerializeField] private LevelUI _levelUI;

    private SpawnObject _nextSpawnObject;
    private int _points;
    private List<SpawnObject> _spawnObjects;

    public Transform MinPos => _minPos;
    public Transform MaxPos => _maxPos;
    public List<SpawnObject> SpawnObjects => _spawnObjects;

    private void Start()
    {
        _spawnObjects = new List<SpawnObject>();
        
        var instObject = Instantiate(_gameConfig.ObjectConfigs[0].Prefab, _spawnPosition);
        instObject.DeactivateObject();
        instObject.transform.position = _spawnPosition.position;
        instObject.SetMergeSystem(this);
        instObject.SetConfig(_gameConfig.ObjectConfigs[0]);
        
        _spawnObjectPositionComponent.SetObject(instObject);

        _nextSpawnObject = GenerateRandomObject();
        _nextObjectSprite.sprite = _nextSpawnObject.Config.Sprite;
    }

    public void SpawnNextLevelObject(ObjectConfig objectConfig, Vector3 newPos)
    {
        if (objectConfig.ObjectType == ObjectType.Sun)
        {
            ShowEndLevelScreen();
            return;
        }

        AddPoints(objectConfig.AddPoints);

        ObjectType nextSpawnObject = (ObjectType)((int)objectConfig.ObjectType + 1);
            
        SpawnObject spawnObject = Instantiate(_gameConfig.ObjectConfigs[(int)nextSpawnObject].Prefab, _objectsHolder);
        spawnObject.transform.position = newPos;
        spawnObject.SetConfig(_gameConfig.ObjectConfigs[(int)nextSpawnObject]);
        spawnObject.SetMergeSystem(this);
        spawnObject.ActivateObject();
        
        AddSpawnObjectToList(spawnObject);
    }

    public void SetActiveGame(bool active)
    {
        _spawnObjectPositionComponent.SetInputActive(active);
    }

    private void AddPoints(int p0)
    {
        _points += p0;
        _pointText.text = _points.ToString();
    }

    public void ShowEndLevelScreen()
    {
        _levelUI.EndUIObject.SetActive(true);
        Debug.Log("Show end level screen");
    }

    public void SetNextObject()
    {
        SpawnObject instObject = Instantiate(_nextSpawnObject, _spawnPosition);
        instObject.DeactivateObject();
        instObject.transform.position = _spawnPosition.transform.position;
        instObject.SetMergeSystem(this);
            
        _spawnObjectPositionComponent.SetObject(instObject);
        _nextSpawnObject = GenerateRandomObject();
        _nextObjectSprite.sprite = _nextSpawnObject.Config.Sprite;
    }

    private SpawnObject GenerateRandomObject()
    {
        int randomInt = UnityEngine.Random.Range(0, 3);
        var mergeObject = _gameConfig.ObjectConfigs[randomInt].Prefab;
        mergeObject.SetConfig(_gameConfig.ObjectConfigs[randomInt]);
        return mergeObject;
    }

    public void DeleteSpawnObjectFromList(SpawnObject spawnObject)
    {
        _spawnObjects.Remove(spawnObject);
    }

    public void AddSpawnObjectToList(SpawnObject spawnObject)
    {
        _spawnObjects.Add(spawnObject);
    }

    public void ResetGame()
    {
        foreach (var spawnObject in _spawnObjects)
        {
            Destroy(spawnObject.gameObject);
        }
        
        _spawnObjects.Clear();
        _points = 0;
        _pointText.text = _points.ToString();
    }
}