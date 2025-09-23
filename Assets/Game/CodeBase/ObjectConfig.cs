using System;
using Game.CodeBase;
using UnityEngine;

[Serializable]
public class ObjectConfig
{
    [SerializeField] private ObjectType _objectType;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private SpawnObject _prefab;
    [SerializeField] private int _addPoints;

    public ObjectType ObjectType => _objectType;
    public Sprite Sprite => _sprite;
    public SpawnObject Prefab => _prefab;
    public int AddPoints => _addPoints;
}