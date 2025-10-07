using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float _amplitude = 20f;
    [SerializeField] private float _frequency = 1f;

    private Vector3 _startPos;
    private float _phaseOffset;

    private void Start()
    {
        _startPos = transform.localPosition;
        _phaseOffset = transform.position.x * 0.5f; 
    }

    private void Update()
    {
        float y = Mathf.Sin(Time.time * _frequency + _phaseOffset) * _amplitude;
        transform.localPosition = _startPos + new Vector3(0, y, 0);
    }
}