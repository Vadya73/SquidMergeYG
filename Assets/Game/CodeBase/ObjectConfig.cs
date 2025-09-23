using System;
using UnityEngine;

namespace Game.CodeBase
{
    [Serializable]
    public class ObjectConfig
    {
        [SerializeField] private ObjectType _objectType;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private SpawnObject _prefab;
        
        public ObjectType ObjectType => _objectType;
        public Sprite Sprite => _sprite;
        public SpawnObject Prefab => _prefab;
    }
}