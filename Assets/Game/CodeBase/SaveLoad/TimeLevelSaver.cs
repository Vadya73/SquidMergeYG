using System;
using MyInput;
using UnityEngine;
using VContainer;

namespace SaveLoad
{
    public class TimeLevelSaver : MonoBehaviour
    {
        private const float TimeToSave = 30f;
        private float _currentTimeToSave;
        private LevelSaver _levelSaver;

        [Inject]
        private void Construct(LevelSaver levelSaver)
        {
            _levelSaver = levelSaver;
        }

        private void Start()
        {
            _currentTimeToSave = TimeToSave;
        }

        private void FixedUpdate()
        {
            if (GameState.GameFinished)
                return;
            
            _currentTimeToSave -= Time.fixedDeltaTime;
            
            if (_currentTimeToSave <= 0)
            {
                _currentTimeToSave = TimeToSave;
                _levelSaver.SaveLevel();
            }
        }
    }
}