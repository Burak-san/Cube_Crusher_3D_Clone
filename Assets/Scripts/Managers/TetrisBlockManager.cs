using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Cube;
using DG.Tweening;
using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Managers
{
    public class TetrisBlockManager : MonoBehaviour
    {
        [SerializeField] public CubeTransform[] cubePositions;
        [SerializeField] private Vector3[] PathList = new Vector3[3];
        
        private GridManager _gridManager;
        private List<int> fullRowIndexList = new List<int>();
        private PathType _pathType = PathType.Linear;
        
        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void OnChangeGameState(GameStates currentState)
        {
            if (currentState == GameStates.MergePhase)
            {
                StartCoroutine(MergeRows());
                
            }
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
                    xIndex >= _gridManager.Nodes.GetLength(0) || 
                    yIndex >= _gridManager.Nodes.GetLength(1)) return false;
                
                Tile checkingTile = _gridManager.Nodes[xIndex, yIndex];

                control = checkingTile.IsPlaceable;
                if (checkingTile.IsEnemyTile) return false;
                if (checkingTile.IsBaseTile) return false;
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
                
                Tile checkingTile = _gridManager.Nodes[xIndex, yIndex];

                checkingTile.HeldCube = cubeTransform.cube;
                cubeTransform.cube.transform.SetParent(checkingTile.transform);
                checkingTile.IsPlaceable = false;
                checkingTile.SnapPoint();
            }
        }

        private void CheckMerge()
        {
            bool isRowFull = false;

            for (int y = 0; y < _gridManager.Nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _gridManager.Nodes.GetLength(0); x++)
                {
                    Tile checkingTile = _gridManager.Nodes[x, y];
                    if (checkingTile.IsEnemyTile) return;
                    
                    if (checkingTile.IsPlaceable == false && checkingTile.IsBaseTile == false)
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
        }

        private IEnumerator MergeRows()
        {
            CheckMerge();

            if (fullRowIndexList.Count == 0)
            {
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.AttackPhase);
                Destroy(gameObject);
                yield break;
            }

            for (var ındex = 0; ındex < fullRowIndexList.Count; ındex++)
            {
                int index = fullRowIndexList[ındex];
                
                for (int x = 0; x < _gridManager.Nodes.GetLength(0); x++)
                {
                    var x1 = x;
                    
                    
                    Vector3 midDistance = _gridManager.BaseCubeList[x].transform.position
                                          -_gridManager.Nodes[x, index].HeldCube.transform.position;
                    
                    PathList[0] = new Vector3(
                        _gridManager.Nodes[x, index].HeldCube.transform.position.x,
                        _gridManager.Nodes[x, index].HeldCube.transform.position.y,
                        _gridManager.Nodes[x, index].HeldCube.transform.position.z);
                    
                    PathList[1] = new Vector3(
                        _gridManager.Nodes[x, index].HeldCube.transform.position.x + midDistance.x/2,
                        _gridManager.Nodes[x, index].HeldCube.transform.position.y + midDistance.y/2+1,
                        _gridManager.Nodes[x, index].HeldCube.transform.position.z + midDistance.z/2);
                    
                    PathList[2] = new Vector3(
                        _gridManager.BaseCubeList[x].transform.position.x,
                        _gridManager.BaseCubeList[x].transform.position.y,
                        _gridManager.BaseCubeList[x].transform.position.z);
                    
                    _gridManager.Nodes[x, index].HeldCube.transform.DOPath(PathList, 1, _pathType).OnComplete(
                        () =>
                        {
                            _gridManager.BaseCubeList[x1].IncreaseCubeValue(_gridManager.Nodes[x1, index].
                                HeldCube.GetComponent<IncrementCubes>().CubeValue);
                            _gridManager.Nodes[x1, index].IsPlaceable = true;
                            Destroy(_gridManager.Nodes[x1, index].HeldCube.gameObject);
                            
                            _gridManager.Nodes[x1, index].HeldCube = null;
                        }).SetEase(Ease.InOutSine);
                    
                    yield return new WaitForSeconds(.1f);
                }
            }

            yield return new WaitForSeconds(1f);
            
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.AttackPhase);
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

