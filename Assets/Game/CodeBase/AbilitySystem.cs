using System;
using System.Collections.Generic;
using Audio;
using DG.Tweening;
using Game.CodeBase;
using Infrastructure;
using Timer;
using UI;
using UnityEngine;
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
    private readonly CameraEffectsSystem _cameraEffectsSystem;

    private BombAbilityObject _currentBomb;
    private List<SpawnObject> _selectedObjects;

    private CountDownTimer _bombCooldownTimer;
    private CountDownTimer _swapCooldownTimer;
    private CountDownTimer _mixCooldownTimer;
    private CountDownTimer _deleteCooldownTimer;
    
    public AbilitySystem(IObjectResolver objectResolver,BombAbilityObject bombAbilityObjectPrefab, LevelUI levelUI, 
        MergeGameSystem mergeGameSystem, AudioManager audioManager, AudioData audioData, MonoHelper monoHelper, AbilityData abilityData,
        CameraEffectsSystem cameraEffectsSystem)
    {
        _objectResolver = objectResolver;
        _bombAbilityObjectPrefab = bombAbilityObjectPrefab;
        _levelUI = levelUI;
        _mergeGameSystem = mergeGameSystem;
        _audioManager = audioManager;
        _audioData = audioData;
        _monoHelper = monoHelper;
        _abilityData = abilityData;
        _cameraEffectsSystem = cameraEffectsSystem;
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
        _bombCooldownTimer.OnTimeChanged += UpdateBombTimeView;
        _bombCooldownTimer.OnEnded += ActiveBombButton;

        _swapCooldownTimer.OnStarted += DisableSwapButton;
        _swapCooldownTimer.OnTimeChanged += UpdateSwapTimeView;
        _swapCooldownTimer.OnEnded += ActiveSwapButton;

        _mixCooldownTimer.OnStarted += DisableMixButton;
        _mixCooldownTimer.OnTimeChanged += UpdateMixTimeView;
        _mixCooldownTimer.OnEnded += ActiveMixButton;

        _deleteCooldownTimer.OnStarted += DisableDeleteButton;
        _deleteCooldownTimer.OnTimeChanged += UpdateDeleteTimeView;
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
        _levelUI.DeleteObjectCooldownImage.gameObject.SetActive(false);
    }

    private void DisableDeleteButton()
    {
        _levelUI.DeleteObjectButton.interactable = false;
        _levelUI.DeleteObjectCooldownImage.gameObject.SetActive(true);
        _levelUI.DeleteObjectCooldownImage.fillAmount = 1f;
    }

    private void UpdateDeleteTimeView()
    {
        _levelUI.DeleteObjectCooldownImage.fillAmount = 1f - _deleteCooldownTimer.Progress;
    }
    
    private void ActiveMixButton()
    {
        _levelUI.MixAllObjectsButton.interactable = true;
        _levelUI.MixAllObjectsCooldownImage.gameObject.SetActive(false);
    }

    private void DisableMixButton()
    {
        _levelUI.MixAllObjectsButton.interactable = false;
        _levelUI.MixAllObjectsCooldownImage.gameObject.SetActive(true);
        _levelUI.MixAllObjectsCooldownImage.fillAmount = 1f;
    }

    private void UpdateMixTimeView()
    {
        _levelUI.MixAllObjectsCooldownImage.fillAmount = 1f - _mixCooldownTimer.Progress;
    }

    private void ActiveSwapButton()
    {
        _levelUI.SwapTwoObjectsButton.interactable = true;
        _levelUI.SwapTwoObjectsCooldownImage.gameObject.SetActive(false);
    }

    private void DisableSwapButton()
    {
        _levelUI.SwapTwoObjectsButton.interactable = false;
        _levelUI.SwapTwoObjectsCooldownImage.gameObject.SetActive(true);
        _levelUI.SwapTwoObjectsCooldownImage.fillAmount = 1f;
    }

    private void UpdateSwapTimeView()
    {
        _levelUI.SwapTwoObjectsCooldownImage.fillAmount = 1f - _swapCooldownTimer.Progress;
    }

    private void ActiveBombButton()
    {
        _levelUI.BombButton.interactable = true;
        _levelUI.BombCooldownImage.gameObject.SetActive(false);
    }

    private void DisableBombButton()
    {
        _levelUI.BombButton.interactable = false;
        _levelUI.BombCooldownImage.gameObject.SetActive(true);
        _levelUI.BombCooldownImage.fillAmount = 1f;
    }

    private void UpdateBombTimeView()
    {
        _levelUI.BombCooldownImage.fillAmount = 1f - _bombCooldownTimer.Progress;
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
        
        _cameraEffectsSystem.Shake(.5f, 0.1f);
        
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