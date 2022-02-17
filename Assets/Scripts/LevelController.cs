using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    // [SerializeField] private List<GameObject> _figures;
    [SerializeField] private List<GameObject> _figureTemplates;
    [SerializeField] private Transform _createPoint;
    [SerializeField] private Transform _figuresContainer;
    [SerializeField] private List<Material> _backMaterials;
    [SerializeField] private GameObject _background;

    private readonly float _delayBeforeNewLevel = 2;
    private readonly float _fallingTime = 2;
    private readonly float _offsetY = 5;
    private GameObject _currentFigure;
    private int _currentIndex;
    private FigureRotater _figureRotater;

    private void Start()
    {
        if (_figureTemplates.Count == 0)
            return;
        
        _currentIndex = 0;
        
        ExecuteLevel(_currentIndex);
    }

    private void ExecuteLevel(int index)
    {
        InitRandomBackground();
        CreateFigure();
        AnimateFigure();
        StartCoroutine(StartMixing(_fallingTime + 0.25f));
    }

    private void InitRandomBackground()
    {
        MeshRenderer mesh = _background.GetComponent<MeshRenderer>();
        var materials = _backMaterials.Except(_backMaterials.Where(m => m == mesh.sharedMaterial)).ToList();
        mesh.sharedMaterial = materials[Random.Range(0, materials.Count)];
    }
    
    private void CreateFigure()
    {
        _currentFigure = Instantiate(_figureTemplates[_currentIndex], _createPoint.position, Quaternion.identity, _figuresContainer);
        _figureRotater = _currentFigure.GetComponent<FigureRotater>();
        _currentFigure.GetComponent<FigureChecker>().LevelCompleted += OnLevelCompleted;
    }    
    
    private void AnimateFigure()
    {
        _currentFigure.transform.DOMove(_currentFigure.transform.position - new Vector3(0, _offsetY, 0), _fallingTime);
    }
    
    private IEnumerator StartMixing(float delay)
    {
        yield return new WaitForSeconds(delay);
        MixUpPartsFigure();
    }
    
    private void MixUpPartsFigure()
    {
        _figureRotater.MixUpParts();
    }

    private void OnLevelCompleted()
    {
        if (_currentIndex < _figureTemplates.Count - 1)
        {
            Destroy(_currentFigure, 0.5f);
            ExecuteLevel(++_currentIndex);
        }
        else
        {
            Debug.Log("Game Completed");
        }
    }
}