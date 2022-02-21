using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<GameObject> _figureTemplates;
    [SerializeField] private Transform _createPoint;
    [SerializeField] private Transform _figuresContainer;
    [SerializeField] private List<Material> _backMaterials;
    [SerializeField] private GameObject _background;

    private readonly float _delayBeforeNewLevel = 2;
    private readonly float _fallingTime = 2;
    private readonly float _shakingTime = 0.15f;
    private readonly float _offsetY = 5;
    private readonly float _miniOffsetY = 0.05f;
    private readonly float _endLevelDelay = 0.5f;
    private readonly int _reward = 50;
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
        StartCoroutine(StartMixing(_fallingTime + _shakingTime + 0.25f));
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
        _currentFigure.GetComponent<FigureMerger>().FigureMerged += OnFigureMerged;
    }

    private void AnimateFigure()
    {
        Sequence animateFigure = DOTween.Sequence();
        animateFigure.Append(_currentFigure.transform.DOMove(_currentFigure.transform.position - new Vector3(0, _offsetY, 0), _fallingTime).SetEase(Ease.InQuad));
        animateFigure.Append(_currentFigure.transform.DOMove(_currentFigure.transform.position - new Vector3(0,_offsetY + _miniOffsetY, 0), _shakingTime/2).SetEase(Ease.InBounce));
        animateFigure.Append(_currentFigure.transform.DOMove(_currentFigure.transform.position - new Vector3(0, _offsetY, 0), _shakingTime/2).SetEase(Ease.InQuad));
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
            StartCoroutine(EndLevel(_endLevelDelay));
        }
        else
        {
            Debug.Log("Game Completed");
        }
    }

    private IEnumerator EndLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(_currentFigure);
        ExecuteLevel(++_currentIndex);       
    }
    
    
    private void OnFigureMerged(GameObject arg0)
    {
        _player.AddStars(_reward);
    }
}