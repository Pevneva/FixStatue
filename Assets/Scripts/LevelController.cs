using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _figures;
    [SerializeField] private List<Material> _backMaterials;
    [SerializeField] private GameObject _background;

    private GameObject _currentFigure;
    private int _currentIndex;

    private void Start()
    {
        _currentIndex = 0;

        if (_figures.Count > 0)
        {
            InitFigure(_currentIndex);
            DisableFiguresExcept(_currentFigure);
        }

        InitBackground();
    }

    private void InitFigure(int index)
    {
        Debug.Log("Init " + index);
        _figures[index].gameObject.SetActive(true);
        _currentFigure = _figures[index];
        _figures[index].GetComponent<FigureChecker>().LevelCompleted += OnLevelCompleted;
    }

    private void InitBackground()
    {
        MeshRenderer mesh = _background.GetComponent<MeshRenderer>();
        var materials = _backMaterials.Except(_backMaterials.Where(m => m == mesh.sharedMaterial)).ToList();
        mesh.sharedMaterial = materials[Random.Range(0, materials.Count)];
    }

    private void DisableFiguresExcept(GameObject figure)
    {
        var inactiveList = _figures.Except(_figures.Where(f => f == figure)).ToList();

        foreach (var inactiveFigure in inactiveList)
            inactiveFigure.SetActive(false);
    }

    private void OnLevelCompleted()
    {
        if (_currentIndex < _figures.Count - 1)
        {
            Destroy(_figures[_currentIndex]);
            InitFigure(++_currentIndex);
            InitBackground();
        }
        else
        {
            Debug.Log("Game Completed");
        }
    }
}