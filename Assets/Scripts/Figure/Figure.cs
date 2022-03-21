using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FigureAnimator), typeof(FigureRotater))]
[RequireComponent(typeof(FigureMerger))]
public class Figure : MonoBehaviour
{
    [SerializeField] private int _subLevelReward;
    [SerializeField] private int _levelReward;

    private readonly float _delayBeforMixing = 0.25f;
    private FigureAnimator _figureAnimator;
    private FigureRotater _figureRotater;
    private FigureMerger _figureMerger;
    private Player _player;
    private RectTransform _starIcon;
    private RectTransform _uiStarContainer;

    public int LevelReward => _levelReward;

    private void OnEnable()
    {
        _figureAnimator = GetComponent<FigureAnimator>();
        _figureRotater = GetComponent<FigureRotater>();
        _figureMerger = GetComponent<FigureMerger>();

        _figureAnimator.Fall();
        StartCoroutine(StartMixing(ParamsController.Figure.FallingTime + _delayBeforMixing));
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
        AddStars(_subLevelReward, mergedPart);
        _figureMerger.FigureMerged -= OnFigureMerged;
    }

    public void AddStars(int reward, GameObject mergedPart)
    {
        _player.AddStars(reward);
        _figureAnimator.ShowStarsEffect(mergedPart, _starIcon, _uiStarContainer);          
    }

    private IEnumerator StartMixing(float delay)
    {
        yield return new WaitForSeconds(delay);
        _figureRotater.MixUpParts();
    }
}
