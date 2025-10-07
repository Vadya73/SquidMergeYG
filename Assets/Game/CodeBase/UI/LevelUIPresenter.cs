using System;
using DG.Tweening;
using SaveLoad;
using VContainer;
using VContainer.Unity;
using YG;

namespace UI
{
    public sealed class LevelUIPresenter : IInitializable, IDisposable
    {
        private readonly IObjectResolver _objectResolver;
        private readonly LevelUI _levelUI;
        private readonly LoadScreen _loadScreen;
        private readonly LevelSaver _levelSaver;
        private MergeGameSystem _mergeGameSystem;

        private bool _hintIsAnimated;
        private bool _hintActive;

        public LevelUIPresenter(IObjectResolver objectResolver,LevelUI levelUI, LoadScreen loadScreen, LevelSaver levelSaver)
        {
            _objectResolver = objectResolver;
            _levelUI = levelUI;
            _loadScreen = loadScreen;
            _levelSaver = levelSaver;
        }

        public void Initialize()
        {
            _mergeGameSystem = _objectResolver.Resolve<MergeGameSystem>();
            
            _levelUI.HintButton.onClick.AddListener(ShowPlanetsHint);
            _levelUI.ExitButton.onClick.AddListener(LoadMainMenu);
            _levelUI.ExitFromEndUIButton.onClick.AddListener(LoadMainMenuFromEnd);
            _levelUI.RetryGameButton.onClick.AddListener(ResetGame);

            _levelUI.HintObject.gameObject.SetActive(false);
            _levelUI.BombCooldownImage.gameObject.SetActive(false);
            _levelUI.SwapTwoObjectsCooldownImage.gameObject.SetActive(false);
            _levelUI.MixAllObjectsCooldownImage.gameObject.SetActive(false);
            _levelUI.DeleteObjectCooldownImage.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _levelUI.HintButton.onClick.RemoveListener(ShowPlanetsHint);
            _levelUI.ExitButton.onClick.RemoveListener(LoadMainMenu);
            _levelUI.ExitFromEndUIButton.onClick.RemoveListener(LoadMainMenu);
            _levelUI.RetryGameButton.onClick.RemoveListener(ResetGame);
        }

        private void ResetGame()
        {
            YG2.InterstitialAdvShow();
            _mergeGameSystem.ResetGame();
            _levelUI.EndUIObject.SetActive(false);
        }

        private void LoadMainMenu()
        {
            _levelSaver.SaveLevel();
            _loadScreen.LoadScene(1);
        }

        private void LoadMainMenuFromEnd()
        {
            YG2.InterstitialAdvShow();
            _levelSaver.CleanLevelData();
            _loadScreen.LoadScene(1);
        }

        private void ShowPlanetsHint()
        {
            if (_hintIsAnimated)
                return;
            
            if (_hintActive)
            {
                HidePlanetsHint();
                return;
            }
            
            _levelUI.HintObject.position = _levelUI.HiddenHintPosition.position;
            _levelUI.HintObject.gameObject.SetActive(true);
            _hintIsAnimated = true;
            _levelUI.HintObject.transform.DOMove(_levelUI.DefaultHintPosition.position, .5f).OnComplete(() => _hintIsAnimated = false);
            _hintActive = true;
        }

        private void HidePlanetsHint()
        {
            if (!_hintActive)
                return;
            
            _hintActive = false;
            _hintIsAnimated = true;
            _levelUI.HintObject.DOMove(_levelUI.HiddenHintPosition.position, .5f)
                .OnComplete(() =>
                {
                    _levelUI.HintObject.gameObject.SetActive(false);
                    _hintIsAnimated = false;
                });
        }
    }
}