using UnityEngine;

namespace Game.CodeBase
{
    [CreateAssetMenu(menuName = "Create GameConfig", fileName = "GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private ObjectConfig[] _objectConfigs;
        public ObjectConfig[] ObjectConfigs => _objectConfigs;
    }
}