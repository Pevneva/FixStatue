using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FigureAnimator), typeof(FigureRotater))]
[RequireComponent(typeof(FigureMerger))]
public class Figure : MonoBehaviour
{
    // [SerializeField] private GameObject _figure;
    [SerializeField] private int _subLevelReward;
    [SerializeField] private int _levelReward;
    
    private FigureAnimator _figureAnimator;
    private FigureRotater _figureRotater;
    private FigureMerger _figureMerger;
    private Player _player;

    private void OnEnable()
    {
        Debug.Log("On Enable Figure");
        _figureAnimator = GetComponent<FigureAnimator>();
        _figureRotater = GetComponent<FigureRotater>();
        _figureMerger = GetComponent<FigureMerger>();
        
        _figureAnimator.Fall();
        StartCoroutine(StartMixing(ParamsController.Figure.FallingTime + ParamsController.Figure.ShakingTime + 0.25f));
    }

    public void Init(Player player)
    {
        _player = player;
        _figureMerger.FigureMerged += OnFigureMerged;        
    }
    
    private void OnFigureMerged(GameObject mergedPart)
    {
        _player.AddStars(_subLevelReward);
    }
    
    private IEnumerator StartMixing(float delay)
    {
        yield return new WaitForSeconds(delay);
        _figureRotater.MixUpParts();
    }
}
