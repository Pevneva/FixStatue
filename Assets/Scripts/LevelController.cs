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
    [SerializeField] private List<Figure> _figureTemplates;
    [SerializeField] private Transform _createPoint;
    [SerializeField] private Transform _figuresContainer;
    [SerializeField] private List<Material> _backMaterials;
    [SerializeField] private GameObject _background;

    private readonly float _delayBeforeNewLevel = 2;
    private readonly float _endLevelDelay = 3f;
    // private readonly float _fallingTime = 2;
    // private readonly float _shakingTime = 0.15f;
    // private readonly float _offsetY = 4.75f;
    // private readonly float _miniOffsetY = 0.05f;
    private Figure _currentFigure;
    private int _currentIndex;
    // private FigureRotater _figureRotater;
    // private FigureAnimator _figureAnimator;

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
        SetFigure();
        // AnimateFigure();
        // StartCoroutine(StartMixing(_fallingTime + _shakingTime + 0.25f));
        // StartCoroutine(StartMixing(2.15f + 0.25f));
    }

    private void InitRandomBackground()
    {
        MeshRenderer mesh = _background.GetComponent<MeshRenderer>();
        var materials = _backMaterials.Except(_backMaterials.Where(m => m == mesh.sharedMaterial)).ToList();
        mesh.sharedMaterial = materials[Random.Range(0, materials.Count)];
    }
    
    private void SetFigure()
    {
        _currentFigure = Instantiate(_figureTemplates[_currentIndex], _createPoint.position, Quaternion.identity, _figuresContainer);
        _currentFigure.Init(_player);
        
        _currentFigure.GetComponent<FigureChecker>().LevelCompleted += OnLevelCompleted;
        
        // _currentFigure.gameObject.GetComponent<FigureAnimator>().Animate();
        // _figureRotater = _currentFigure.gameObject.GetComponent<FigureRotater>();
        // _figureAnimator = _currentFigure.gameObject.GetComponent<FigureAnimator>();
        // _currentFigure.GetComponent<FigureMerger>().FigureMerged += OnFigureMerged;
    }

    // private void AnimateFigure()
    // {
    //     Sequence animateFigure = DOTween.Sequence();
    //     animateFigure.Append(_currentFigure.transform.DOMove(_currentFigure.transform.position - new Vector3(0, _offsetY, 0), _fallingTime).SetEase(Ease.InQuad));
    //     animateFigure.Append(_currentFigure.transform.DOMove(_currentFigure.transform.position - new Vector3(0,_offsetY + _miniOffsetY, 0), _shakingTime/2).SetEase(Ease.InBounce));
    //     animateFigure.Append(_currentFigure.transform.DOMove(_currentFigure.transform.position - new Vector3(0, _offsetY, 0), _shakingTime/2).SetEase(Ease.InQuad));
    // }
    // private IEnumerator StartMixing(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     MixUpPartsFigure();
    // }
    //
    // private void MixUpPartsFigure()
    // {
    //     _figureRotater.MixUpParts();
    // }

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
        Destroy(_currentFigure.gameObject);
        ExecuteLevel(++_currentIndex);       
    }
    
    // private void OnFigureMerged(GameObject arg0)
    // {
    //     _player.AddStars(_reward);
    // }
}