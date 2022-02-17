using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput))]
public class FigureRotater : MonoBehaviour
{
    private readonly float _speed = 0.65f;
    private float _angle;
    private Quaternion _startRotation;
    private Transform _partToRotate;
    private PlayerInput _playerInput;
    private FigureMerger _figureMerger;
    private List<GameObject> _removedParts = new List<GameObject>();

    public event UnityAction<GameObject> RotationEnded;

    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        _figureMerger = GetComponent<FigureMerger>();

        foreach (var partWithCollider in GetComponentsInChildren<PartWithCollider>())
            partWithCollider.ColliderPartClicked += OnPartWithColliderClicked;

        _playerInput.MouseUpped += OnMouseUpped;
        _figureMerger.FigureMerged += OnFigureMerged;
    }

    private void OnDisable()
    {
        foreach (var partWithCollider in GetComponentsInChildren<PartWithCollider>())
            partWithCollider.ColliderPartClicked -= OnPartWithColliderClicked;

        _playerInput.MouseUpped -= OnMouseUpped;
        _figureMerger.FigureMerged -= OnFigureMerged;
    }

    private void OnFigureMerged(GameObject mergedFigure)
    {
        Debug.Log("AAA Removed figure : " + mergedFigure);
        _removedParts.Add(mergedFigure);
    }

    public void TryRotate(RotateDirection direction)
    {
        if (_partToRotate == null)
            return;

        if (_removedParts.Contains(_partToRotate.gameObject))
            return;

        _partToRotate.rotation = _startRotation * Quaternion.Euler(_partToRotate.rotation.x,
            _partToRotate.rotation.y + _angle, _partToRotate.rotation.z);
        _angle = direction == RotateDirection.LEFT ? _angle + _speed : _angle - _speed;
    }

    private void OnPartWithColliderClicked(GameObject partWithCollider)
    {
        SetPartToRotate(partWithCollider);
    }

    private void SetPartToRotate(GameObject partWithCollider)
    {
        _partToRotate = GetParent(partWithCollider.transform);
        _startRotation = _partToRotate.rotation;
    }

    public Transform GetParent(Transform transform)
    {
        return transform.parent.gameObject.transform;
    }

    private void OnMouseUpped()
    {
        EndRotation();
    }

    private void EndRotation()
    {
        _angle = 0;
        _startRotation = _partToRotate.rotation;
        RotationEnded?.Invoke(_partToRotate.gameObject);
    }
}