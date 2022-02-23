using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FigureAnimator : MonoBehaviour
{
    private readonly float _offsetY = 4.75f;
    private readonly float _miniOffsetY = 0.05f;
    
    public void Fall()
    {
        Sequence animateFigure = DOTween.Sequence();
        animateFigure.Append(transform.DOMove(transform.position - new Vector3(0, _offsetY, 0), ParamsController.Figure.FallingTime).SetEase(Ease.InQuad));
        animateFigure.Append(transform.DOMove(transform.position - new Vector3(0,_offsetY + _miniOffsetY, 0), ParamsController.Figure.ShakingTime/2).SetEase(Ease.InBounce));
        animateFigure.Append(transform.DOMove(transform.position - new Vector3(0, _offsetY, 0), ParamsController.Figure.ShakingTime/2).SetEase(Ease.InQuad));
    }
}
