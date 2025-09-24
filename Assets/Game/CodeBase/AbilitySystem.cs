using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.CodeBase;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

public class AbilitySystem : IStartable, IInitializable, IDisposable
{
    private readonly IObjectResolver _objectResolver;
    private readonly BombAbilityObject _bombAbilityObjectPrefab;
    private readonly LevelUI _levelUI;
    private readonly MergeGameSystem _mergeGameSystem;

    private BombAbilityObject _currentBomb;
    private List<SpawnObject> _selectedObjects;
    
    public AbilitySystem(IObjectResolver objectResolver,BombAbilityObject bombAbilityObjectPrefab, LevelUI levelUI, 
        MergeGameSystem mergeGameSystem)
    {
        _objectResolver = objectResolver;
        _bombAbilityObjectPrefab = bombAbilityObjectPrefab;
        _levelUI = levelUI;
        _mergeGameSystem = mergeGameSystem;
    }

    public void Initialize()
    {
        _levelUI.BombButton.onClick.AddListener(EnableBombAbility);
        _levelUI.SwapTwoObjectsButton.onClick.AddListener(EnableSwapAbility);
        _levelUI.MixAllObjectsButton.onClick.AddListener(EnableMixAbility);
        _levelUI.DeleteObjectButton.onClick.AddListener(EnableDeleteObjectAbility);
    }

    public void Dispose()
    {
        _levelUI.BombButton.onClick.RemoveListener(EnableBombAbility);
        _levelUI.SwapTwoObjectsButton.onClick.RemoveListener(EnableSwapAbility);
        _levelUI.MixAllObjectsButton.onClick.RemoveListener(EnableMixAbility);
        _levelUI.DeleteObjectButton.onClick.RemoveListener(EnableDeleteObjectAbility);
    }

    void IStartable.Start()
    {
        _selectedObjects = new List<SpawnObject>();
        
        _currentBomb = _objectResolver.Instantiate(_bombAbilityObjectPrefab);
        _currentBomb.HideBomb();
        _currentBomb.SetActivateGame(() => _mergeGameSystem.SetActiveGame(true));
    }

    private void EnableBombAbility()
    {
        _mergeGameSystem.SetActiveGame(false);
        _currentBomb.ShowBomb();
    }

    private void EnableSwapAbility()
    {
        _mergeGameSystem.SetActiveGame(false);

        foreach (var spawnObject in _mergeGameSystem.SpawnObjects)
            spawnObject.OnObjectClicked += SelectObjectToSwap;
    }

    private void SelectObjectToSwap(SpawnObject spawnObject)
    {
        if (spawnObject.IsSelected)
        {
            spawnObject.SetSelected(false);
            _selectedObjects.Remove(spawnObject);
            return;
        }
        
        _selectedObjects.Add(spawnObject);
        spawnObject.SetSelected(true);

        if (_selectedObjects.Count == 2)
        {
            Vector3 firstPos = _selectedObjects[0].transform.position;
            Vector3 secondPos = _selectedObjects[1].transform.position;
            
            _selectedObjects[0].transform.position = secondPos;
            _selectedObjects[1].transform.position = firstPos;

            _selectedObjects[0].SetSelected(false);
            _selectedObjects[1].SetSelected(false);
            
            _selectedObjects.Clear();
            
            foreach (var spawnObjectL in _mergeGameSystem.SpawnObjects)
                spawnObjectL.OnObjectClicked += SelectObjectToSwap;
            
            DOVirtual.DelayedCall(.5f, () => _mergeGameSystem.SetActiveGame(true));
        }
    }

    private void EnableMixAbility()
    {
        _mergeGameSystem.SetActiveGame(false);

        List<Vector3> positions = new List<Vector3>();
        foreach (var spawnObject in _mergeGameSystem.SpawnObjects)
            positions.Add(spawnObject.transform.position);

        for (int i = positions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (positions[i], positions[j]) = (positions[j], positions[i]);
        }

        for (int i = 0; i < _mergeGameSystem.SpawnObjects.Count; i++)
            _mergeGameSystem.SpawnObjects[i].transform.position = positions[i];

        DOVirtual.DelayedCall(.5f, () => _mergeGameSystem.SetActiveGame(true));
    }


    private void EnableDeleteObjectAbility()
    {
        _mergeGameSystem.SetActiveGame(false);

        foreach (var spawnObject in  _mergeGameSystem.SpawnObjects)
            spawnObject.OnObjectClicked += DeleteSpawnObject;
    }

    private void DeleteSpawnObject(SpawnObject obj)
    {
        foreach (var spawnObject in  _mergeGameSystem.SpawnObjects)
            spawnObject.OnObjectClicked -= DeleteSpawnObject;
        
        obj.DestroyObject();

        DOVirtual.DelayedCall(.5f, () => _mergeGameSystem.SetActiveGame(true));
    }
}