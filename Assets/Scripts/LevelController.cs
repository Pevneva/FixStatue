using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private RectTransform _starIcon;
    [SerializeField] private RectTransform _starUiContainer;

    private readonly float _delayBeforeNewLevel = 2;
    private readonly float _delayBeforeEndLevel = 3f;
    private Figure _currentFigure;
    private int _currentIndex;

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
        _currentFigure.Init(_player, _starUiContainer, _starIcon);
        _currentFigure.GetComponent<FigureChecker>().LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(GameObject mergedPart, Animator animator)
    {
        if (_currentIndex < _figureTemplates.Count - 1)
        {
            _currentFigure.AddStars(_currentFigure.LevelReward, mergedPart);
            animator.enabled = true;
            StartCoroutine(EndLevel(_delayBeforeEndLevel));
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
}