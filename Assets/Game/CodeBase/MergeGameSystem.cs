using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.CodeBase
{
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

        public void SpawnNextLevelObject(ObjectType configObjectType, Vector3 newPos)
        {
            if (configObjectType == ObjectType.Sun)
            {
                ShowEndLevelScreen();
                return;
            }

            switch (configObjectType)
            {
                case ObjectType.Moon:
                    AddPoints(5);
                    break;
                case ObjectType.Mercury:
                    AddPoints(10);
                    break;
                case ObjectType.Pluton:
                    AddPoints(15);
                    break;
                case ObjectType.Mars:
                    AddPoints(20);
                    break;
                case ObjectType.Venus:
                    AddPoints(25);
                    break;
                case ObjectType.Earth:
                    AddPoints(30);
                    break;
                case ObjectType.Neptune:
                    AddPoints(35);
                    break;
                case ObjectType.Uran:
                    AddPoints(40);
                    break;
                case ObjectType.Saturn:
                    AddPoints(40);
                    break;
                case ObjectType.Jupiter:
                    AddPoints(40);
                    break;
            }

            ObjectType nextSpawnObject = (ObjectType)((int)configObjectType + 1);
            SpawnObject spawnObject = Instantiate(_gameConfig.ObjectConfigs[(int)nextSpawnObject].Prefab, _objectsHolder);
            spawnObject.transform.position = newPos;
            spawnObject.SetConfig(_gameConfig.ObjectConfigs[(int)nextSpawnObject]);
            spawnObject.SetMergeSystem(this);
            spawnObject.ActivateObject();
        }

        private void AddPoints(int p0)
        {
            _points += p0;
            _pointText.text = _points.ToString();
        }

        private void ShowEndLevelScreen()
        {
            throw new NotImplementedException();
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
}