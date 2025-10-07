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
    [SerializeField] private Vector2 _savedPosition;

    public ObjectType ObjectType => _objectType;
    public Sprite Sprite => _sprite;
    public SpawnObject Prefab => _prefab;
    public int AddPoints => _addPoints;
    public Vector2 SavedPosition => _savedPosition;

    public void SavePosition(Vector3 transformPosition) => _savedPosition = transformPosition;
}