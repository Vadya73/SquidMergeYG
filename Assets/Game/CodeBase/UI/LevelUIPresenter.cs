using System;
using DG.Tweening;
using VContainer.Unity;

namespace UI
{
    public sealed class LevelUIPresenter : IInitializable, IDisposable
    {
        private readonly LevelUI _levelUI;
        private readonly LoadScreen _loadScreen;

        private bool _hintIsAnimated;
        private bool _hintActive;

        public LevelUIPresenter(LevelUI levelUI, LoadScreen loadScreen)
        {
            _levelUI = levelUI;
            _loadScreen = loadScreen;
        }

        public void Initialize()
        {
            _levelUI.HintButton.onClick.AddListener(ShowPlanetsHint);
            _levelUI.ExitButton.onClick.AddListener(LoadMainMenu);
            _levelUI.ExitFromEndUIButton.onClick.AddListener(LoadMainMenu);
            
            _levelUI.HintObject.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _levelUI.HintButton.onClick.RemoveListener(ShowPlanetsHint);
            _levelUI.ExitButton.onClick.RemoveListener(LoadMainMenu);
            _levelUI.ExitFromEndUIButton.onClick.RemoveListener(LoadMainMenu);
        }

        private void LoadMainMenu()
        {
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