using System;
using Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class TetrisBlockController : MonoBehaviour
    {
        [SerializeField] private CubeTransform[] cubePositions;

        private bool isSelected = false;

        private void Check()
        {
            //checkable control
        }

        private void Place()
        {
            //placeable control
        }
    }

    [Serializable]
    public struct CubeTransform
    {
        public IncrementCubes cube;
        public int x;
        public int y;
        
    }
}

