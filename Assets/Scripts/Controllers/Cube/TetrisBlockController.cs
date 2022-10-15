using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class TetrisBlockController : MonoBehaviour
    {
        [SerializeField] public CubeTransform[] cubePositions;

        private GridManager _gridManager;
        
        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
        }

        public bool Check(Vector2Int checkingTileIndex)
        {
            bool control = false;
            
            foreach (CubeTransform cubeTransform in cubePositions)
            {
                int xIndex = checkingTileIndex.x + cubeTransform.x;
                int yIndex = checkingTileIndex.y + cubeTransform.y;

                if (xIndex < 0 || 
                    yIndex < 0 || 
                    xIndex >= _gridManager._nodes.GetLength(0) || 
                    yIndex >= _gridManager._nodes.GetLength(1)) return false;
                
                Tile checkingTile = _gridManager._nodes[xIndex, yIndex];

                control = checkingTile.IsPlaceable;
                
                if (control == false) return false;
            }
            
            return true;
        }

        public void Place(Vector2Int checkingTileIndex)
        {
            foreach (CubeTransform cubeTransform in cubePositions)
            {
                int xIndex = checkingTileIndex.x + cubeTransform.x;
                int yIndex = checkingTileIndex.y + cubeTransform.y;
                
                Tile checkingTile = _gridManager._nodes[xIndex, yIndex];

                checkingTile.HeldCube = cubeTransform.cube;
                cubeTransform.cube.transform.SetParent(checkingTile.transform);
                checkingTile.IsPlaceable = false;
                checkingTile.SnapPoint();
            }
            Destroy(gameObject);
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

