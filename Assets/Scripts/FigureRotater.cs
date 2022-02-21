using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerInput))]
public class FigureRotater : MonoBehaviour
{
    private static int s_rotateCounter;
    private readonly float _speed = 1f;
    private readonly float _autoRotateTime = 0.65f;
    private float _angle;
    private Quaternion _startRotation;
    private Transform _partToRotate;
    private PlayerInput _playerInput;
    private FigureMerger _figureMerger;
    private List<GameObject> _removedParts = new List<GameObject>();
    private PartWithCollider[] _partsWithColliders;

    public event UnityAction<GameObject> RotationEnded;

    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        _figureMerger = GetComponent<FigureMerger>();
        _partsWithColliders = GetComponentsInChildren<PartWithCollider>();

        Debug.Log("Enable : " + _partsWithColliders[0]);
        foreach (var partWithCollider in _partsWithColliders)
            partWithCollider.ColliderPartClicked += OnPartWithColliderClicked;

        _playerInput.MouseUpped += OnMouseUpped;
        _figureMerger.FigureMerged += OnFigureMerged;
    }

    private void OnDisable()
    {
        foreach (var partWithCollider in _partsWithColliders)
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
        if (_partToRotate == null)
            return;
        
        _angle = 0;
        _startRotation = _partToRotate.rotation;
        RotationEnded?.Invoke(_partToRotate.gameObject);
    }

    public void MixUpParts()
    {
        Debug.Log("Mix : " + _partsWithColliders[0]);

        foreach (var part in _partsWithColliders)
        {
            RandomRotate(GetParent(part.transform));
        }
    }

    private void RandomRotate(Transform partToRotate)
    {
        var angle = s_rotateCounter % 2 == 0 ? Random.Range(-90, 0) : Random.Range(0, 90);
        s_rotateCounter++;

        partToRotate.DORotate(new Vector3(partToRotate.rotation.x,
            partToRotate.rotation.y + angle, partToRotate.rotation.z), _autoRotateTime);
    }
}