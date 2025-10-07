using System.Collections;
using Infrastructure;
using UnityEngine;

public class CameraEffectsSystem
{
    private readonly MonoHelper _monoHelper;
    private readonly Camera _camera;
    private Vector3 _originalPos;
    private Coroutine _shakeRoutine;

    public CameraEffectsSystem(MonoHelper monoHelper, Camera camera)
    {
        _monoHelper = monoHelper;
        _camera = camera;
    }

    public void Shake(float duration, float magnitude)
    {
        if (_shakeRoutine != null)
            _monoHelper.StopCoroutine(_shakeRoutine);

        _shakeRoutine = _monoHelper.StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        _originalPos = _camera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            _camera.transform.localPosition = _originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _camera.transform.localPosition = _originalPos;
        _shakeRoutine = null;
    }
}