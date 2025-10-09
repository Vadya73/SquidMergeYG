using System;
using System.Collections.Generic;
using Audio;
using Game.CodeBase;
using Infrastructure;
using MyInput;
using SaveLoad;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using YG;
using Random = UnityEngine.Random;

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
    private int _score;
    private List<SpawnObject> _spawnObjects;
    
    private LoadScreen _loadScreen;
    private IObjectResolver _objectResolver;
    private AudioManager _audioManager;
    private AudioData _audioData;
     
    private Action _onLsShow;
    private Action _onLsHide;
    private LevelSaver _levelSaver;

    public Transform MinPos => _minPos;
    public Transform MaxPos => _maxPos;
    public List<SpawnObject> SpawnObjects => _spawnObjects;
    public int Score => _score;


    [Inject]
    private void Construct(IObjectResolver objectResolver, LoadScreen loadScreen, AudioManager audioManager,
        AudioData audioData)
    {
        _objectResolver = objectResolver;
        _loadScreen = loadScreen;
        _audioManager = audioManager;
        _audioData = audioData;
    }

    private void Awake()
    {
        SetActiveGame(false);
        GameState.GameFinished = false;
    }

    private void Start()
    {
        _levelSaver = _objectResolver.Resolve<LevelSaver>();
        _spawnObjects = new List<SpawnObject>();

        var instObject = _objectResolver.Instantiate(_gameConfig.ObjectConfigs[0].Prefab, _spawnPosition);
        instObject.DeactivateObject();
        instObject.transform.position = _spawnPosition.position;
        instObject.SetConfig(_gameConfig.ObjectConfigs[0]);
        
        _spawnObjectPositionComponent.SetObject(instObject);

        _nextSpawnObject = GenerateRandomObject();
        _nextObjectSprite.sprite = _nextSpawnObject.Config.Sprite;
        
        // lol, don't  do this 
        _onLsShow = () => SetActiveGame(false);
        _onLsHide = () => SetActiveGame(true);
        
        _loadScreen.OnLoadScreenShow += _onLsShow;
        _loadScreen.OnLoadScreenHide += _onLsHide;
    }

    private void OnDestroy()
    {
        _loadScreen.OnLoadScreenShow -= _onLsShow;
        _loadScreen.OnLoadScreenHide -= _onLsHide;
    }

    public void SpawnNextLevelObject(ObjectConfig objectConfig, Vector3 newPos)
    {
        AddPoints(objectConfig.AddPoints);

        ObjectType nextSpawnObject = (ObjectType)((int)objectConfig.ObjectType + 1);
            
        SpawnObject spawnObject = _objectResolver.Instantiate(_gameConfig.ObjectConfigs[(int)nextSpawnObject].Prefab, _objectsHolder);
        spawnObject.transform.position = newPos;
        spawnObject.SetConfig(_gameConfig.ObjectConfigs[(int)nextSpawnObject]);
        spawnObject.ActivateObject();
        
        _audioManager.PlaySound(_audioData.ObjectsMergeSound[Random.Range(0, _audioData.ObjectsMergeSound.Length)]);
        
        AddSpawnObjectToList(spawnObject);
    }
    
    public void SetActiveGame(bool active)
    {
        GameState.InputEnabled = active;
        _spawnObjectPositionComponent.SetInputActive(active);
    }

    private void AddPoints(int p0)
    {
        _score += p0;
        _pointText.text = _score.ToString();
    }

    public void ShowEndLevelScreen()
    {
        GameState.GameFinished = true;

        if (_score > YG2.saves.HighScore)
        {
            Debug.Log($"New High Score: {_score}, old High Score: {YG2.saves.HighScore}");
            YG2.SetLeaderboard("PointsLeaderboard", _score);
            YG2.saves.HighScore = _score;
        }

        if (!YG2.saves.IsShowReview)
        {
            YG2.ReviewShow();
            YG2.saves.IsShowReview = true;
        }
        
        _levelSaver.CleanLevelData();
        _levelUI.EndUIObject.SetActive(true);
        _audioManager.PlaySound(_audioData.EndLevelSound);
    }

    public void SetNextObject()
    {
        SpawnObject instObject = _objectResolver.Instantiate(_nextSpawnObject, _spawnPosition);
        instObject.DeactivateObject();
        instObject.transform.position = _spawnPosition.transform.position;
            
        _spawnObjectPositionComponent.SetObject(instObject);
        _nextSpawnObject = GenerateRandomObject();
        _nextObjectSprite.sprite = _nextSpawnObject.Config.Sprite;
    }

    private SpawnObject GenerateRandomObject()
    {
        int randomInt = Random.Range(0, 3);
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
        GameState.GameFinished = false;
        
        foreach (var spawnObject in _spawnObjects)
            Destroy(spawnObject.gameObject);
        
        _spawnObjects.Clear();
        _score = 0;
        _pointText.text = _score.ToString();
        SetActiveGame(true);
    }

    public void SetLoadData(List<ObjectConfig> spawnObjects, int currentScore)
    {
        var spawnObjectToList = new List<SpawnObject>();
        
        _score = currentScore;
        _pointText.text = _score.ToString();

        foreach (var spawnObject in spawnObjects)
        {
            SpawnObject pref = _gameConfig.ObjectConfigs[(int)spawnObject.ObjectType].Prefab;
            pref.SetConfig(_gameConfig.ObjectConfigs[(int)spawnObject.ObjectType]);
            
            SpawnObject obj = _objectResolver.Instantiate(pref, _objectsHolder);
            obj.SetSavedPosition();
            spawnObjectToList.Add(obj);
        }
        
        _spawnObjects = spawnObjectToList;
    }
}