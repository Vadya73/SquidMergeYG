using Infrastructure;
using UI;
using UnityEngine;
using VContainer;

public class GameBootstrapper : MonoBehaviour
{
    private LoadScreen _loadScreen;

    [Inject]
    private void Construct(LoadScreen loadScreen)
    {
        _loadScreen = loadScreen;
    }

    private void Start()
    {
        _ = _loadScreen.LoadScene(1);
    }
}