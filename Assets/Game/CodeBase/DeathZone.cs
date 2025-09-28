using Game.CodeBase;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private MergeGameSystem _mergeGameSystem;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TryGetComponent(out SpawnObject spawnObject))
        {
            _mergeGameSystem.SetActiveGame(false);
            _mergeGameSystem.ShowEndLevelScreen();
        }
    }
}
