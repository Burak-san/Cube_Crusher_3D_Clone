using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class TetrisBlockController : MonoBehaviour
    {
        [SerializeField] public CubeTransform[] cubePositions;

        private GridManager _gridManager;
        private bool isSelected = false;

        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
        }

        public bool Check(Vector2 checkingTileIndex)
        {
            foreach (CubeTransform cubeTransform in cubePositions)
            {
                Tile checkingTile = _gridManager._nodes[
                    (int)checkingTileIndex.x + cubeTransform.x,
                    (int)checkingTileIndex.y + cubeTransform.y
                ];

                if (checkingTile.IsPlaceable) return true;
            }
            
            return false;
        }

        public void Place(Vector2 checkingTile)
        {
            //place
        }

        public void RemoveCubesFromTiles()
        {
            foreach (CubeTransform cubeTransform in cubePositions)
            {
                _gridManager._nodes[cubeTransform.x, cubeTransform.y] = null;
            }
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

