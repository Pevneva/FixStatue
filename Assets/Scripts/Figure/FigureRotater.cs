using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerInput), typeof(FigureMerger), typeof(FigureAnimator))]

public class FigureRotater : MonoBehaviour
{
    private readonly float _speed = 6f;
    private readonly float _maxRandomAngle = 110f;
    private readonly float _minRandomAngle = 10f;
    private static int s_rotateCounter;
    private readonly float _autoRotateTime = 0.95f;
    private float _backRotationAngle = 5f;
    private float _angle;
    private Quaternion _startRotation;
    private Transform _partToRotate;
    private PlayerInput _playerInput;
    private FigureMerger _figureMerger;
    private FigureAnimator _figureAnimator;
    private List<GameObject> _removedParts = new List<GameObject>();
    private PartWithCollider[] _partsWithColliders;
    private RotateDirection _rotateDirection;
    
    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        _figureMerger = GetComponent<FigureMerger>();
        _figureAnimator = GetComponent<FigureAnimator>();
        _partsWithColliders = GetComponentsInChildren<PartWithCollider>();

        foreach (var partWithCollider in _partsWithColliders)
            partWithCollider.ColliderPartClicked += OnPartWithColliderClicked;

        _playerInput.MouseUpped += OnMouseUpped;
        _figureMerger.FigureMerged += OnFigureMerged;
    }

    private void OnFigureMerged(GameObject mergedFigure)
    {
        _removedParts.Add(mergedFigure);
    }

    public void TryRotate(RotateDirection direction)
    {
        if (_partToRotate == null)
            return;

        if (_removedParts.Contains(_partToRotate.gameObject))
            return;
        
        _partToRotate.rotation = _startRotation * Quaternion.Euler(_partToRotate.rotation.x,
            _partToRotate.rotation.y + _angle, _partToRotate.rotation.z);;
        _angle = direction == RotateDirection.LEFT ? _angle + _speed : _angle - _speed;
        _rotateDirection = direction;
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

    public Transform GetParent(Transform transformChild)
    {
        return transformChild.parent.gameObject.transform;
    }

    private void OnMouseUpped()
    {
        EndManualRotation();
    }

    private void EndManualRotation()
    {
        if (_partToRotate == null)
            return;

        _angle = 0;
        _startRotation = _partToRotate.rotation;
        _figureAnimator.RotateBack(_rotateDirection, _partToRotate, _backRotationAngle);
    }

    public void MixUpParts()
    {
        foreach (var part in _partsWithColliders)
            RandomRotate(GetParent(part.transform));
    }

    private void RandomRotate(Transform partToRotate)
    {
        var angle = s_rotateCounter % 2 == 0 ? Random.Range(-_maxRandomAngle, -_minRandomAngle) : Random.Range(_minRandomAngle, _maxRandomAngle);
        s_rotateCounter++;

        partToRotate.DORotate(new Vector3(partToRotate.rotation.x,
            partToRotate.rotation.y + angle, partToRotate.rotation.z), _autoRotateTime).SetEase(Ease.Linear);
    }
}