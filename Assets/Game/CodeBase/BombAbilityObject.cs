using System;
using System.Collections.Generic;
using MyInput;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Collider2D))]
public class BombAbilityObject : MonoBehaviour
{
    private const float _explosionForce = 10f;
    private const float _explosionRadius = 5f;
    private Transform _objectTransform;
    private List<Collider2D> _collidedObjects = new();
    private bool _isActive;
    private bool _isMoving;
    private ProjectInput _input;

    private Action _activateGameInput;

    [Inject]
    private void Construct(ProjectInput projectInput)
    {
        _input = projectInput;
    }
    
    private void Awake()
    {
        _objectTransform = GetComponent<Transform>();
        _isActive = false;
        _isMoving = false;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        if (_input.GetPointerPosition() is { } pointerPosition)
        {
            _objectTransform.position = new Vector3(pointerPosition.x,pointerPosition.y, transform.position.z);
            _isMoving = true;
            return;
        }

        if (_isMoving)
        {
            Explode();
            _isMoving = false;
        }
    }

    public void SetActivateGame(Action action)
    {
        _activateGameInput = action;
    }

    private void Explode()
    {
        foreach (Collider2D collider in _collidedObjects)
        {
            var objRb = collider.attachedRigidbody;
            if (!objRb)
                continue;
            
            Vector2 direction = (objRb.worldCenterOfMass - (Vector2)_objectTransform.position).normalized;
            
            float dist = Vector2.Distance(_objectTransform.position, objRb.worldCenterOfMass);
            float falloff = 1f - Mathf.Clamp01(dist / _explosionRadius);

            Vector2 force = direction * (_explosionForce * falloff);

            objRb.AddForce(force, ForceMode2D.Impulse);
        }
        HideBomb();
        _activateGameInput?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other) => _collidedObjects.Add(other);

    private void OnTriggerExit2D(Collider2D other) => _collidedObjects.Remove(other);
    
    public void HideBomb()
    {
        _collidedObjects.Clear();
        _isActive = false;
        gameObject.SetActive(false);
    }

    public void ShowBomb()
    {
        _isActive = true;
        gameObject.SetActive(true);
    }
}