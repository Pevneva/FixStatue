using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FigureChecker))]
public class FigureAnimator : MonoBehaviour
{
    [SerializeField] private GameObject _flyingStarsFx;
    [SerializeField] private Animator _winAnimator;

    private readonly float _fallOffsetY = 3.5f;
    private readonly float _scaleFactor = 0.15f;
    private readonly float _fallScaleChangedStepTime = 0.075f;
    private readonly float _initStarOffset = 0.35f;
    private readonly float _pulsingZOffset = 0.4f;
    private FigureChecker _figureChecker;

    public Animator WinAnimator => _winAnimator;
    public event UnityAction<GameObject> BackRotationEnded;

    private void OnEnable()
    {
        _figureChecker = GetComponent<FigureChecker>();
        _figureChecker.InBetweenMerged += OnInBetweenMerged;
        _winAnimator.enabled = false;
    }

    public void Fall()
    {
        var startScale = transform.localScale;
        Sequence animateFigure = DOTween.Sequence();
        animateFigure.Append(transform
            .DOMove(transform.position - new Vector3(0, _fallOffsetY, 0), ParamsController.Figure.FallingTime)
            .SetEase(Ease.InQuad));
        animateFigure.Append(transform
            .DOScale(new Vector3(startScale.x + _scaleFactor, startScale.y - _scaleFactor, startScale.z + _scaleFactor),
                _fallScaleChangedStepTime).SetEase(Ease.Linear));
        animateFigure.Append(transform.DOScale(startScale, _fallScaleChangedStepTime).SetEase(Ease.Linear));
        animateFigure.Append(transform
            .DOScale(new Vector3(startScale.x - _scaleFactor, startScale.y + _scaleFactor, startScale.z - _scaleFactor),
                _fallScaleChangedStepTime).SetEase(Ease.Linear));
        animateFigure.Append(transform.DOScale(startScale, _fallScaleChangedStepTime).SetEase(Ease.Linear));
    }

    public void ShowStarsEffect(GameObject mergedPart, RectTransform uiContainer, RectTransform starIcon)
    {
        var position = _flyingStarsFx.transform.position;
        var newPosition = new Vector3(position.x, mergedPart.transform.position.y + _initStarOffset, position.z);
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
        Sequence pulsing = DOTween.Sequence();
        pulsing.Append(neighbor.transform.DOMoveZ(neighbor.transform.position.z - _pulsingZOffset,
            ParamsController.Figure.DelayMerging / 2));
        pulsing.Append(neighbor.transform.DOMoveZ(neighbor.transform.position.z,
            ParamsController.Figure.DelayMerging / 2));
    }

    public void RotateBack(RotateDirection direction, Transform partToRotate, float backRotationAngle)
    {
        Sequence backRotating = DOTween.Sequence();
        var angle = direction == RotateDirection.LEFT ? -backRotationAngle : backRotationAngle;
        var backTime = ParamsController.Figure.BackRotationTime;
        backRotating.Append(partToRotate
                .DOLocalRotate(partToRotate.rotation.eulerAngles + new Vector3(0, angle, 0), 2 * backTime / 3))
            .SetEase(Ease.OutCirc);
        backRotating.Append(partToRotate
                .DOLocalRotate(partToRotate.rotation.eulerAngles + new Vector3(0, angle / 2, 0), backTime / 3))
            .SetEase(Ease.OutCirc);
        backRotating.OnComplete(() => { BackRotationEnded?.Invoke(partToRotate.gameObject); });
    }
}