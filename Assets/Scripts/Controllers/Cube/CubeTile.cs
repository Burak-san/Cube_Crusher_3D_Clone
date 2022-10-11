using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Data;
using UnityEngine;

public class CubeTile : MonoBehaviour
{
    [SerializeField] public Vector3 OffSetVector;

    private Camera mainCam;
    public IncrementCubes HoldCube;
    public int TileIndex;
    public bool IsEmpty;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void ReturnToSpawnPoint()
    {
        HoldCube.transform.position = transform.position + OffSetVector;
    }
}
