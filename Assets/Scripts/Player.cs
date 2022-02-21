using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public int Stars { get; private set; }
    public event UnityAction<int> StarsAdded; 

    private void Awake()
    {
        AddStars(100);
    }

    public void AddStars(int addedValue)
    {
        Stars += addedValue;
        StarsAdded?.Invoke(addedValue);
    }
}
