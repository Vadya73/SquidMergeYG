using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField, ChildGameObjectsOnly] private Button _bombButton;
        [SerializeField, ChildGameObjectsOnly] private Button _swapTwoObjectsButton;
        [SerializeField, ChildGameObjectsOnly] private Button _mixAllObjectsButton;
        [SerializeField, ChildGameObjectsOnly] private Button _deleteObjectButton;
    
        public Button BombButton => _bombButton;
        public Button SwapTwoObjectsButton => _swapTwoObjectsButton;
        public Button MixAllObjectsButton => _mixAllObjectsButton;
        public Button DeleteObjectButton => _deleteObjectButton;
    }
}
