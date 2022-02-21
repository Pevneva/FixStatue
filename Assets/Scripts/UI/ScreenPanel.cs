using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _starAmount;
    [SerializeField] private Player _player;

    private float _countTime = 2.1f;
    private bool _isCounting;
    private int _startStars;
    private int _addedStars;
    private float _tempAddedStars;
    private float _interpolationValue = 0.0f;
    
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
        
        Debug.Log(" _addedStars : " + _addedStars);
        Debug.Log(" _startStars : " + _startStars);
        Debug.Log(" result : " + (_startStars + _addedStars));
        
        Invoke(nameof(StartCounting), 1);
        
        // if (Int32.TryParse(_starAmount.text, out _startStars))
        // {
        //     _starAmount.text = addedValue.ToString();
        //     _addedStars = addedValue;
        //     
        //     Invoke(nameof(StartCounting), 1);
        // }
    }

    private void StartCounting()
    {
        _isCounting = true;
    }

    private void TryCount()
    {
        if (_isCounting)
        {
            Debug.Log("_interpolationValue : " + _interpolationValue);
            _tempAddedStars = _startStars;
            _tempAddedStars = Mathf.Lerp(_tempAddedStars, _startStars + _addedStars, _interpolationValue);
            _starAmount.text = Mathf.Round(_tempAddedStars).ToString();

            _interpolationValue += 1.25f * Time.deltaTime;

            if (_interpolationValue > 1)
            {
                _isCounting = false;
                _interpolationValue = 0;
            }
        }
    }    
}
