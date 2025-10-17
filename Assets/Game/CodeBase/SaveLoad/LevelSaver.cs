using System.Collections.Generic;
using Game.CodeBase;
using UnityEngine;
using VContainer.Unity;
using YG;

namespace SaveLoad
{
    public class LevelSaver : IInitializable
    {
        private MergeGameSystem _mergeGameSystem;

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
            YG2.saves.SpawnObjectsData.Clear();
            var objectsData = new List<SpawnObjectData>();

            foreach (var spawnObject in _mergeGameSystem.SpawnObjects)
            {
                var pos = spawnObject.transform.position;
                objectsData.Add(new SpawnObjectData
                {
                    ObjectType = spawnObject.Config.ObjectType,
                    X = pos.x,
                    Y = pos.y,
                    Z = pos.z
                });
            }

            YG2.saves.SpawnObjectsData = objectsData;
            YG2.saves.CurrentScore = _mergeGameSystem.Score;
            YG2.SaveProgress();

            Debug.Log($"Level Saved, objects: {objectsData.Count}, score: {_mergeGameSystem.Score}" );
        }

        private void LoadLevel()
        {
            var objectsData = YG2.saves.SpawnObjectsData;
            var currentScore = YG2.saves.CurrentScore;
            
            if (objectsData == null ||objectsData.Count == 0)
                return;
            
            _mergeGameSystem.SetLoadData(objectsData, currentScore);
            Debug.Log("Level Loaded, spawned: " + objectsData.Count + " objects, Score: " + currentScore);
        }

        public void CleanLevelData()
        {
            YG2.saves.SpawnObjectsData.Clear();
            YG2.saves.CurrentScore = 0;
            YG2.SaveProgress();
        }
    }
}
