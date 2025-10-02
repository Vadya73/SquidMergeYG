using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelUI : MonoBehaviour
    {
        [Header("Abilitys")]
        [SerializeField, ChildGameObjectsOnly] private Button _bombButton;
        [SerializeField, ChildGameObjectsOnly] private Image _bombImage;
        [SerializeField, ChildGameObjectsOnly] private Button _swapTwoObjectsButton;
        [SerializeField, ChildGameObjectsOnly] private Image _swapTwoObjectsImage;
        [SerializeField, ChildGameObjectsOnly] private Button _mixAllObjectsButton;
        [SerializeField, ChildGameObjectsOnly] private Image _mixAllObjectsImage;
        [SerializeField, ChildGameObjectsOnly] private Button _deleteObjectButton;
        [SerializeField, ChildGameObjectsOnly] private Image _deleteObjectImage;
        [Header("Hint")]
        [SerializeField, ChildGameObjectsOnly] private Button _hintButton;
        [SerializeField] private Transform _hintObject;
        [SerializeField] private Transform _defaultHintPosition;
        [SerializeField] private Transform _hiddenHintPosition;
        [Header("ExitButton")]
        [SerializeField, ChildGameObjectsOnly] private Button _exitButton;
        [Header("EndUI")]
        [SerializeField, ChildGameObjectsOnly] private GameObject _endUIObject;
        [SerializeField, ChildGameObjectsOnly] private Button _retryGameButton;
        [SerializeField, ChildGameObjectsOnly] private Button _exitFromEndUIButton;
        
        public Button BombButton => _bombButton;
        public Image BombImage => _bombImage;
        public Button SwapTwoObjectsButton => _swapTwoObjectsButton;
        public Image SwapTwoObjectsImage => _swapTwoObjectsImage;
        public Button MixAllObjectsButton => _mixAllObjectsButton;
        public Image MixAllObjectsImage => _mixAllObjectsImage;
        public Button DeleteObjectButton => _deleteObjectButton;
        public Image DeleteObjectImage => _deleteObjectImage;
        public Button HintButton => _hintButton;
        public Button ExitButton => _exitButton;
        public Button RetryGameButton => _retryGameButton;
        public Button ExitFromEndUIButton => _exitFromEndUIButton;
        public Transform HintObject => _hintObject;
        public Transform DefaultHintPosition => _defaultHintPosition;
        public Transform HiddenHintPosition => _hiddenHintPosition;
        public GameObject EndUIObject => _endUIObject;
    }
}
