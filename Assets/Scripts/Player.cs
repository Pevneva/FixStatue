using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public int StarAmount { get; private set; }
    public event UnityAction<int> StarsValueChanged; 

    private void Awake()
    {
        AddStars(100);
    }

    private void AddStars(int value)
    {
        StarAmount += value;
        StarsValueChanged?.Invoke(StarAmount);
    }
}
