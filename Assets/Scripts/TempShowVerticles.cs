using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TempShowVerticles : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;

    private void Start()
    {
        
        // _meshFilter.mesh.triangles.
        foreach (var verticle in _meshFilter.mesh.vertices)
        {
            Debug.Log(verticle);
        }
    }
}
