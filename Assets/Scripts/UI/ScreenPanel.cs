using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _starAmount;
    [SerializeField] private Player _player;
    [SerializeField] private RectTransform _targetStarImage;

    private float _countTime = 2.1f;
    private bool _isCounting;
    private int _startStars;
    private int _addedStars;
    private float _tempAddedStars;
    private float _interpolationValue = 0.0f;

    public RectTransform TargetStarImage => _targetStarImage;
    
    private void Start()
    {
        _starAmount.text = _player.Stars.ToString();
        _player.StarsAdded += OnStarsChanged;
    }
    
    private void Update()
    {
        TryCount();
    }

    private void OnStarsChanged(int addedValue)
    {
        _startStars = _player.Stars - addedValue;
        _addedStars = addedValue;
        
        Invoke(nameof(StartCounting), 1);
    }

    private void StartCounting()
    {
        _isCounting = true;
    }

    private void TryCount()
    {
        if (_isCounting)
        {
            _tempAddedStars = _startStars;
            _tempAddedStars = Mathf.Lerp(_tempAddedStars, _startStars + _addedStars, _interpolationValue);
            _starAmount.text = Mathf.Round(_tempAddedStars).ToString();

            _interpolationValue += 1.5f * Time.deltaTime;

            if (Mathf.Round(_tempAddedStars) == _startStars + _addedStars)
            {
                _isCounting = false;
                _interpolationValue = 0;
            }
        }
    }    
}
