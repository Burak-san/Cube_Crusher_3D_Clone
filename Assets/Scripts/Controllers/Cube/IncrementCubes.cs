using System;
using Data;
using UnityEngine;

namespace Controllers
{
    public class IncrementCubes : MonoBehaviour
    {
        public CubeData CubeData;
        public TetrisBlockController parentTetrisBlock;
        public bool IsPlaceable;

        private void Awake()
        {
            parentTetrisBlock = GetComponentInParent<TetrisBlockController>();
        }
    }
}