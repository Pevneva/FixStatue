using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(FigureChecker))]
public class FigureAnimator : MonoBehaviour
{
    [SerializeField] private GameObject _flyingStarsFx;
    [SerializeField] private Animator _winAnimator;

    public Animator WinAnimator => _winAnimator;
    
    private readonly float _offsetY = 3.5f;
    private readonly float _miniOffsetY = 0.075f;
    private readonly float _scaleFactor = 1.25f;
    private FigureChecker _figureChecker;

    private void OnEnable()
    {
        _figureChecker = GetComponent<FigureChecker>();
        _figureChecker.InBetweenMerged += OnInBetweenMerged;
        _winAnimator.enabled = false;
    }

    public void Fall()
    {
        Sequence animateFigure = DOTween.Sequence();
        animateFigure.Append(transform
            .DOMove(transform.position - new Vector3(0, _offsetY, 0), ParamsController.Figure.FallingTime)
            .SetEase(Ease.InQuad));
        animateFigure.Append(transform.DOMove(transform.position - new Vector3(0, _offsetY + _miniOffsetY, 0),
            ParamsController.Figure.ShakingTime / 2).SetEase(Ease.InBounce));
        animateFigure.Append(transform
            .DOMove(transform.position - new Vector3(0, _offsetY, 0), ParamsController.Figure.ShakingTime / 2)
            .SetEase(Ease.InQuad));
    }

    public void ShowStarsEffect(GameObject mergedPart, RectTransform uiContainer, RectTransform starIcon)
    {
        var position = _flyingStarsFx.transform.position;
        var newPosition = new Vector3(position.x, mergedPart.transform.position.y + 0.35f, position.z);
        _flyingStarsFx.transform.position = newPosition;

        if (_flyingStarsFx.activeSelf)
            _flyingStarsFx.SetActive(false);

        _flyingStarsFx.SetActive(true);
        _flyingStarsFx.GetComponentInChildren<StarsHandler>().Init(uiContainer, starIcon);
    }

    private void OnInBetweenMerged(GameObject neighbor)
    {
        PulsePartFigure(neighbor);
    }

    private void PulsePartFigure(GameObject neighbor)
    {
        Debug.Log("BBB PULSE !!! neighbor : " + neighbor);
        Sequence pulsing = DOTween.Sequence();
        pulsing.Append(neighbor.transform.DOMoveZ(neighbor.transform.position.z - 0.4f,
            ParamsController.Figure.DelayMerging / 2));
        pulsing.Append(neighbor.transform.DOMoveZ(neighbor.transform.position.z,
            ParamsController.Figure.DelayMerging / 2));
    }
}