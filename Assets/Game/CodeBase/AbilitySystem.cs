using System;
using System.Collections.Generic;
using Audio;
using DG.Tweening;
using Game.CodeBase;
using Infrastructure;
using Timer;
using UI;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using YG;
using Random = UnityEngine.Random;

public class AbilitySystem : IStartable, IInitializable, IDisposable
{
    private readonly IObjectResolver _objectResolver;
    private readonly BombAbilityObject _bombAbilityObjectPrefab;
    private readonly LevelUI _levelUI;
    private readonly MergeGameSystem _mergeGameSystem;
    private readonly AudioManager _audioManager;
    private readonly AudioData _audioData;
    private readonly MonoHelper _monoHelper;
    private readonly AbilityData _abilityData;

    private BombAbilityObject _currentBomb;
    private List<SpawnObject> _selectedObjects;

    private CountDownTimer _bombCooldownTimer;
    private CountDownTimer _swapCooldownTimer;
    private CountDownTimer _mixCooldownTimer;
    private CountDownTimer _deleteCooldownTimer;
    
    public AbilitySystem(IObjectResolver objectResolver,BombAbilityObject bombAbilityObjectPrefab, LevelUI levelUI, 
        MergeGameSystem mergeGameSystem, AudioManager audioManager, AudioData audioData, MonoHelper monoHelper, AbilityData abilityData)
    {
        _objectResolver = objectResolver;
        _bombAbilityObjectPrefab = bombAbilityObjectPrefab;
        _levelUI = levelUI;
        _mergeGameSystem = mergeGameSystem;
        _audioManager = audioManager;
        _audioData = audioData;
        _monoHelper = monoHelper;
        _abilityData = abilityData;
    }

    public void Initialize()
    {
        _levelUI.BombButton.onClick.AddListener(ShowAdAndUseBomb);
        _levelUI.SwapTwoObjectsButton.onClick.AddListener(ShowAdAndUseSwap);
        _levelUI.MixAllObjectsButton.onClick.AddListener(ShowAdAndUseMix);
        _levelUI.DeleteObjectButton.onClick.AddListener(ShowAdAndUseDelete);

        _bombCooldownTimer = new CountDownTimer(_monoHelper, _abilityData.BombCooldown);
        _swapCooldownTimer = new CountDownTimer(_monoHelper, _abilityData.SwapCooldown);
        _mixCooldownTimer = new CountDownTimer(_monoHelper, _abilityData.MixCooldown);
        _deleteCooldownTimer = new CountDownTimer(_monoHelper, _abilityData.DeleteCooldown);

        _bombCooldownTimer.OnStarted += DisableBombButton;
        _bombCooldownTimer.OnEnded += ActiveBombButton;

        _swapCooldownTimer.OnStarted += DisableSwapButton;
        _swapCooldownTimer.OnEnded += ActiveSwapButton;

        _mixCooldownTimer.OnStarted += DisableMixButton;
        _mixCooldownTimer.OnEnded += ActiveMixButton;

        _deleteCooldownTimer.OnStarted += DisableDeleteButton;
        _deleteCooldownTimer.OnEnded += ActiveDeleteButton;
    }
    
    public void Dispose()
    {
        _levelUI.BombButton.onClick.RemoveListener(ShowAdAndUseBomb);
        _levelUI.SwapTwoObjectsButton.onClick.RemoveListener(ShowAdAndUseSwap);
        _levelUI.MixAllObjectsButton.onClick.RemoveListener(ShowAdAndUseMix);
        _levelUI.DeleteObjectButton.onClick.RemoveListener(ShowAdAndUseDelete);
        
        _bombCooldownTimer.OnStarted -= DisableBombButton;
        _bombCooldownTimer.OnEnded -= ActiveBombButton;

        _swapCooldownTimer.OnStarted -= DisableSwapButton;
        _swapCooldownTimer.OnEnded -= ActiveSwapButton;

        _mixCooldownTimer.OnStarted -= DisableMixButton;
        _mixCooldownTimer.OnEnded -= ActiveMixButton;

        _deleteCooldownTimer.OnStarted -= DisableDeleteButton;
        _deleteCooldownTimer.OnEnded -= ActiveDeleteButton;
    }

    private void ActiveDeleteButton()
    {
        _levelUI.DeleteObjectButton.interactable = true;
        _levelUI.DeleteObjectImage.sprite = _abilityData.DeleteActiveSprite;
    }

    private void DisableDeleteButton()
    {
        _levelUI.DeleteObjectButton.interactable = false;
        _levelUI.DeleteObjectImage.sprite = _abilityData.DeleteInactiveSprite;
    }

    private void ActiveMixButton()
    {
        _levelUI.MixAllObjectsButton.interactable = true;
        _levelUI.MixAllObjectsImage.sprite = _abilityData.MixActiveSprite;
    }

    private void DisableMixButton()
    {
        _levelUI.MixAllObjectsButton.interactable = false;
        _levelUI.MixAllObjectsImage.sprite = _abilityData.MixInactiveSprite;
    }

    private void ActiveSwapButton()
    {
        _levelUI.SwapTwoObjectsButton.interactable = true;
        _levelUI.SwapTwoObjectsImage.sprite = _abilityData.SwapActiveSprite;
    }

    private void DisableSwapButton()
    {
        _levelUI.SwapTwoObjectsButton.interactable = false;
        _levelUI.SwapTwoObjectsImage.sprite = _abilityData.SwapInactiveSprite;
    }

    private void ActiveBombButton()
    {
        _levelUI.BombButton.interactable = true;
        _levelUI.BombImage.sprite = _abilityData.BombActiveSprite;
    }

    private void DisableBombButton()
    {
        _levelUI.BombButton.interactable = false;
        _levelUI.BombImage.sprite = _abilityData.BombInactiveSprite;
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
        
        _bombCooldownTimer.ResetTime();
        _bombCooldownTimer.Play();
    }

    private void EnableSwapAbility()
    {
        _mergeGameSystem.SetActiveGame(false);

        _swapCooldownTimer.ResetTime();
        _swapCooldownTimer.Play();
        
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
                spawnObjectL.OnObjectClicked -= SelectObjectToSwap;
            
            _audioManager.PlaySound(_audioData.SwapTwoObjectsSound);
            
            DOVirtual.DelayedCall(.5f, () => _mergeGameSystem.SetActiveGame(true));
        }
    }

    private void EnableMixAbility()
    {
        _mergeGameSystem.SetActiveGame(false);
        
        _mixCooldownTimer.ResetTime();
        _mixCooldownTimer.Play();

        foreach (var spawnObject in _mergeGameSystem.SpawnObjects)
            spawnObject.Rb2D.simulated = false;
        
        
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

        foreach (var spawnObject in _mergeGameSystem.SpawnObjects)
            spawnObject.Rb2D.simulated = true;
        
        _audioManager.PlaySound(_audioData.MixObjectsSound);
        
        DOVirtual.DelayedCall(.5f, () => _mergeGameSystem.SetActiveGame(true));
    }


    private void EnableDeleteObjectAbility()
    {
        _mergeGameSystem.SetActiveGame(false);
        
        _deleteCooldownTimer.ResetTime();
        _deleteCooldownTimer.Play();

        foreach (var spawnObject in  _mergeGameSystem.SpawnObjects)
            spawnObject.OnObjectClicked += DeleteSpawnObject;
    }

    private void DeleteSpawnObject(SpawnObject obj)
    {
        foreach (var spawnObject in  _mergeGameSystem.SpawnObjects)
            spawnObject.OnObjectClicked -= DeleteSpawnObject;
        
        obj.DestroyObject();
        _audioManager.PlaySound(_audioData.DeleteObjectSound);

        DOVirtual.DelayedCall(.5f, () => _mergeGameSystem.SetActiveGame(true));
    }

    private void ShowAdAndUseBomb()
    {
        YG2.RewardedAdvShow("bomb", EnableBombAbility);
    }

    private void ShowAdAndUseSwap()
    {
        YG2.RewardedAdvShow("swapTwoObjects", EnableSwapAbility);
    }

    private void ShowAdAndUseMix()
    {
        YG2.RewardedAdvShow("mixAllObjects", EnableMixAbility);
    }

    private void ShowAdAndUseDelete()
    {
        YG2.RewardedAdvShow("deleteObject", EnableDeleteObjectAbility);
    }
}