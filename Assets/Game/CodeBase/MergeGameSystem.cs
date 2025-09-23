using Game.CodeBase;
using TMPro;
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

    private SpawnObject _nextSpawnObject;
    private int _points;
        
    public Transform MinPos => _minPos;
    public Transform MaxPos => _maxPos;

    private void Start()
    {
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

    private void ShowEndLevelScreen()
    {
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
}