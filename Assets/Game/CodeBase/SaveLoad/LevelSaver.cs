using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using YG;

namespace SaveLoad
{
    public class LevelSaver : IInitializable
    {
        private MergeGameSystem _mergeGameSystem;
        private int _currentScore;
        private List<ObjectConfig> _spawnObjects;

        public LevelSaver(MergeGameSystem mergeGameSystem)
        {
            _mergeGameSystem = mergeGameSystem;
        }

        public void Initialize()
        {
            LoadLevel();
        }

        public void SaveLevel()
        {
            _spawnObjects = new List<ObjectConfig>();
            
            foreach (var spawnObject in _mergeGameSystem.SpawnObjects)
            {
                spawnObject.SavePosition();
                _spawnObjects.Add(spawnObject.Config);
                Debug.Log(spawnObject.Config.Prefab);
            }
            
            YG2.saves.SpawnObjects = _spawnObjects;
            YG2.saves.CurrentScore = _mergeGameSystem.Score;

            if (YG2.saves.HighScore < _mergeGameSystem.Score)
            {
                YG2.saves.HighScore = _mergeGameSystem.Score;
            }
            
            YG2.SaveProgress();
            Debug.Log($"Level Saved, objects: {_spawnObjects.Count}, score: {_mergeGameSystem.Score}" );
        }

        private void LoadLevel()
        {
            _spawnObjects = YG2.saves.SpawnObjects;
            _currentScore = YG2.saves.CurrentScore;
            
            if (_spawnObjects == null ||_spawnObjects.Count == 0)
                return;
            
            _mergeGameSystem.SetLoadData(_spawnObjects, _currentScore);
            Debug.Log("Level Loaded, spawned: " + _spawnObjects.Count + " objects, Score: " + _currentScore);
        }

        public void CleanLevelData()
        {
            YG2.saves.SpawnObjects = null;
            YG2.saves.CurrentScore = 0;
            YG2.SaveProgress();
        }
    }
}
