using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerInput))]
public class FigureRotater : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    private static int s_rotateCounter;
    private readonly float _autoRotateTime = 0.65f;
    private readonly float _stepSmoothRotation = 0.15f;
    private float _backRotationAngle = 5f;
    private float _angle;
    private Quaternion _startRotation;
    private Transform _partToRotate;
    private PlayerInput _playerInput;
    private FigureMerger _figureMerger;
    private List<GameObject> _removedParts = new List<GameObject>();
    private PartWithCollider[] _partsWithColliders;
    private RotateDirection _rotateDirection;

    public event UnityAction<GameObject> RotationEnded;

    private void OnEnable()
    {
        _speed = 6f;
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
        _removedParts.Add(mergedFigure);
    }

    public void TryRotate(RotateDirection direction)
    {
        if (_partToRotate == null)
            return;

        if (_removedParts.Contains(_partToRotate.gameObject))
            return;

        Quaternion rotation = _startRotation * Quaternion.Euler(_partToRotate.rotation.x,
            _partToRotate.rotation.y + _angle, _partToRotate.rotation.z);

        _partToRotate.rotation = rotation;
        // _partToRotate.rotation = Quaternion.Lerp(_partToRotate.rotation, rotation, _stepSmoothRotation);
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

    public Transform GetParent(Transform transform)
    {
        return transform.parent.gameObject.transform;
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
        BackRotate();
    }

    private void BackRotate()
    {
        Sequence backRotating = DOTween.Sequence();
        var angle = _rotateDirection == RotateDirection.LEFT ? -_backRotationAngle : _backRotationAngle;
        var backTime = ParamsController.Figure.BackRotationTime;
        backRotating.Append(_partToRotate
                .DOLocalRotate(_partToRotate.rotation.eulerAngles + new Vector3(0, angle, 0), 2 * backTime / 3))
            .SetEase(Ease.OutCirc);
        backRotating.Append(_partToRotate
                .DOLocalRotate(_partToRotate.rotation.eulerAngles + new Vector3(0, angle / 2, 0), backTime / 3))
            .SetEase(Ease.OutCirc);
        backRotating.OnComplete(() =>
        {
            RotationEnded?.Invoke(_partToRotate.gameObject);
        });
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