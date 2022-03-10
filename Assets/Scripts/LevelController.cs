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
    [SerializeField] private GameObject _confettiFx;
    [SerializeField] private WinTextDisplayer _winTextDisplayer;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioSource _audioSource;

    private readonly float _delayBeforeNewLevel = 2;
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
        _confettiFx.SetActive(false);
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
            _confettiFx.SetActive(true);
            animator.enabled = true;
            _winTextDisplayer.ShowRandomWinText();
            _audioSource.clip = _winSound;
            _audioSource.Play();
            _currentFigure.AddStars(_currentFigure.LevelReward, mergedPart);
            StartCoroutine(EndLevel(ParamsController.Level.DelayBeforeEndLevel));
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