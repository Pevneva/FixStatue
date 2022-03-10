using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StarAnimation : MonoBehaviour
{
    private readonly float _scaleKoef = 1.45f;
    private readonly float _increasingTime = 0.35f;
    private readonly float _decreasingTime = 0.25f;
    private readonly int _pulsingAmount = 3;
    private RectTransform _rectTransform;
    private Vector2 _startSize;
    private Vector2 _increasedSize;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startSize = _rectTransform.sizeDelta;
        _increasedSize = _startSize * _scaleKoef;
    }

    public void DoPulse()
    {
        Sequence starAnimation = DOTween.Sequence();
        starAnimation.Append(_rectTransform.DOSizeDelta(_increasedSize, (_increasingTime)/_pulsingAmount)).SetEase(Ease.Flash).SetLoops(_pulsingAmount);
        starAnimation.Append(_rectTransform.DOSizeDelta(_startSize, (_decreasingTime)/_pulsingAmount)).SetEase(Ease.Linear).SetLoops(_pulsingAmount);
    }
}