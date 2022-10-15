using System;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Signals;
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
            
            MergeRows();

            Destroy(gameObject);
            CoreGameSignals.Instance.onTetrisBlockPlace?.Invoke();
        }

        private List<int> CheckMerge()
        {
            List<int> fullRowIndexList = new List<int>();
            
            bool isRowFull = false;

            for (int y = 0; y < _gridManager._nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _gridManager._nodes.GetLength(0); x++)
                {
                    Tile checkingTile = _gridManager._nodes[x, y];
                    
                    if (checkingTile.IsPlaceable == false)
                    {
                        isRowFull = true;
                        
                    }
                    else
                    {
                        isRowFull = false;
                        break;
                    }
                }

                if (isRowFull)
                    fullRowIndexList.Add(y);
                
            }
            
            return fullRowIndexList;
        }

        private void MergeRows()
        {
            List<int> mergeIndexList = CheckMerge();

            if (mergeIndexList.Count == 0) 
            {
                Debug.Log("There is no row to merge");
                return;
            }
            else
            {
                Debug.Log("Merging layers");
                foreach (int y in mergeIndexList)
                    for (int x = 0; x < _gridManager._nodes.GetLength(0); x++)
                    {
                        _gridManager._nodes[x, y].HeldCube.transform.DOMove(_gridManager.BaseCubeList[x].transform.position, 1f);
                    }
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

