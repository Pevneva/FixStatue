using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[RequireComponent(typeof(FigureRotater), typeof(FigureMerger))]
public class FigureChecker : MonoBehaviour
{
    private readonly float _angleSpread = 5;
    private FigureRotater _figureRotater;
    private FigureMerger _figureMerger;
    private List<GameObject> _neighboringRotateParts; 

    public event UnityAction LevelCompleted;

    private void OnEnable()
    {
        _figureRotater = GetComponent<FigureRotater>();
        _figureMerger = GetComponent<FigureMerger>();
        _figureRotater.RotationEnded += OnRotationEnded;

        if (_neighboringRotateParts == null)
            _neighboringRotateParts = new List<GameObject>();
        else
            _neighboringRotateParts.Clear();

        foreach (var partWithCollider in GetComponentsInChildren<PartWithCollider>())
            _neighboringRotateParts.Add(_figureRotater.GetParent(partWithCollider.gameObject.transform).gameObject);
    }

    private void OnRotationEnded(GameObject rotatedFigure)
    {
        Debug.Log("On RE");
        var _neighbors = _neighboringRotateParts.Except(_neighboringRotateParts
            .Where(p => p == rotatedFigure)).ToList();

        foreach (var neighbor in _neighbors)
        {
            var differenceAngle = Quaternion.Angle(rotatedFigure.transform.rotation, neighbor.transform.rotation);
            if (differenceAngle < _angleSpread)
            {
                _figureMerger.Merge(neighbor, rotatedFigure);
                _neighboringRotateParts.Remove(rotatedFigure);
                if (_neighboringRotateParts.Count == 1)
                    LevelCompleted?.Invoke();
                return;
            }
        }
    }
}