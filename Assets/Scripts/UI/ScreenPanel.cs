using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _starAmount;
    [SerializeField] private Player _player;

    private void Start()
    {
        _starAmount.text = _player.StarAmount.ToString();
        _player.StarsValueChanged += OnStarsChanged;
    }

    private void OnStarsChanged(int value)
    {
        _starAmount.text = value.ToString();
    }
}
