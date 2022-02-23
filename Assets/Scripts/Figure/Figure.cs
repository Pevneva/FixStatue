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
    private StarsHandler _starsHandler;
    private Player _player;
    private RectTransform _starIcon;
    private RectTransform _uiStarContainer;

    private void OnEnable()
    {
        Debug.Log("On Enable Figure");
        _figureAnimator = GetComponent<FigureAnimator>();
        _figureRotater = GetComponent<FigureRotater>();
        _figureMerger = GetComponent<FigureMerger>();
        _starsHandler = GetComponentInChildren<StarsHandler>();
        
        _figureAnimator.Fall();
        StartCoroutine(StartMixing(ParamsController.Figure.FallingTime + ParamsController.Figure.ShakingTime + 0.25f));
    }

    public void Init(Player player, RectTransform starIcon, RectTransform uiStarContainer)
    {
        _player = player;
        _starIcon = starIcon;
        _uiStarContainer = uiStarContainer;
        _figureMerger.FigureMerged += OnFigureMerged;        
    }
    
    private void OnFigureMerged(GameObject mergedPart)
    {
        float offsetY = 0.15f;
        _player.AddStars(_subLevelReward);
        _figureAnimator.ShowStarsEffect(mergedPart.transform.position.y + offsetY, _starIcon, _uiStarContainer);
    }
    
    private IEnumerator StartMixing(float delay)
    {
        yield return new WaitForSeconds(delay);
        _figureRotater.MixUpParts();
    }
}
