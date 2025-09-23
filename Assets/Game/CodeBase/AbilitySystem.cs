using System;
using TMPro;
using UI;
using VContainer;
using VContainer.Unity;

public class AbilitySystem : IStartable, IInitializable, IDisposable
{
    private readonly IObjectResolver _objectResolver;
    private readonly BombAbilityObject _bombAbilityObjectPrefab;
    private readonly LevelUI _levelUI;
    private readonly MergeGameSystem _mergeGameSystem;

    private BombAbilityObject _currentBomb;
    
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
        _currentBomb = _objectResolver.Instantiate(_bombAbilityObjectPrefab);
        _currentBomb.HideBomb();
        _currentBomb.SetActivateGame(() => _mergeGameSystem.SetActiveGame(true));
    }

    private void EnableBombAbility()
    {
        _mergeGameSystem.SetActiveGame(false);
        _currentBomb.ShowBomb();
    }

    public void EnableSwapAbility()
    {
        
    }

    public void EnableMixAbility()
    {
        
    }

    public void EnableDeleteObjectAbility()
    {
        
    }
}