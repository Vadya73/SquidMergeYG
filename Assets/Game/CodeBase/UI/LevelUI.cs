using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelUI : MonoBehaviour
    {
        [Header("Abilitys")]
        [SerializeField, ChildGameObjectsOnly] private Button _bombButton;
        [SerializeField, ChildGameObjectsOnly] private Button _swapTwoObjectsButton;
        [SerializeField, ChildGameObjectsOnly] private Button _mixAllObjectsButton;
        [SerializeField, ChildGameObjectsOnly] private Button _deleteObjectButton;
        [Header("Other Buttons")]
        [SerializeField, ChildGameObjectsOnly] private Button _hintButton;
        [SerializeField, ChildGameObjectsOnly] private Button _exitButton;
        [Header("EndUI")]
        [SerializeField, ChildGameObjectsOnly] private Button _retryGameButton;
        [SerializeField, ChildGameObjectsOnly] private Button _exitFromEndUIButton;
        
        public Button BombButton => _bombButton;
        public Button SwapTwoObjectsButton => _swapTwoObjectsButton;
        public Button MixAllObjectsButton => _mixAllObjectsButton;
        public Button DeleteObjectButton => _deleteObjectButton;
        public Button HintButton => _hintButton;
        public Button ExitButton => _exitButton;
        public Button RetryGameButton => _retryGameButton;
        public Button ExitFromEndUIButton => _exitFromEndUIButton;
    }

    public class LevelUIPresenter
    {
        private readonly LevelUI _levelUI;

        public LevelUIPresenter(LevelUI levelUI)
        {
            _levelUI = levelUI;
        }
    }
}
