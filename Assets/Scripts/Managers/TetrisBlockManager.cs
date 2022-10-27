using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Cube;
using DG.Tweening;
using Enums;
using Signals;
using UnityEngine;
using Data.ValueObject;

namespace Managers
{
    public class TetrisBlockManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] public CubeTransform[] cubePositions;
        [SerializeField] private Vector3[] pathList = new Vector3[3];

        #endregion

        #region Private Variables

        private GridManager _gridManager;
        private List<int> _fullRowIndexList = new List<int>();
        private PathType _pathType = PathType.Linear;

        #endregion

        #endregion
        
        private void Awake()
        {
            GetData();
        }

        private void GetData()
        {
            _gridManager = FindObjectOfType<GridManager>();
        }

        #region Event Subscriptions

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

        #endregion

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
                
                TileData checkingTileData = _gridManager.Nodes[xIndex, yIndex];

                control = checkingTileData.IsPlaceable;
                if (checkingTileData.IsEnemyTile) return false;
                if (checkingTileData.IsBaseTile) return false;
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
                
                TileData checkingTileData = _gridManager.Nodes[xIndex, yIndex];

                checkingTileData.HeldCube = cubeTransform.cube;
                cubeTransform.cube.transform.SetParent(checkingTileData.transform);
                checkingTileData.IsPlaceable = false;
                checkingTileData.SnapPoint();
            }
        }

        private void CheckMerge()
        {
            bool isRowFull = false;

            for (int y = 0; y < _gridManager.Nodes.GetLength(1); y++)
            {
                for (int x = 0; x < _gridManager.Nodes.GetLength(0); x++)
                {
                    TileData checkingTileData = _gridManager.Nodes[x, y];
                    if (checkingTileData.IsEnemyTile) return;
                    
                    if (checkingTileData.IsPlaceable == false && checkingTileData.IsBaseTile == false)
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
                    _fullRowIndexList.Add(y);
            }
        }

        private IEnumerator MergeRows()
        {
            CheckMerge();

            if (_fullRowIndexList.Count == 0)
            {
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.AttackPhase);
                Destroy(gameObject);
                yield break;
            }

            for (var ındex = 0; ındex < _fullRowIndexList.Count; ındex++)
            {
                int index = _fullRowIndexList[ındex];
                
                for (int x = 0; x < _gridManager.Nodes.GetLength(0); x++)
                {
                    var x1 = x;
                    
                    
                    Vector3 midDistance = _gridManager.BaseCubeList[x].transform.position
                                          -_gridManager.Nodes[x, index].HeldCube.transform.position;
                    
                    pathList[0] = new Vector3(
                        _gridManager.Nodes[x, index].HeldCube.transform.position.x,
                        _gridManager.Nodes[x, index].HeldCube.transform.position.y,
                        _gridManager.Nodes[x, index].HeldCube.transform.position.z);
                    
                    pathList[1] = new Vector3(
                        _gridManager.Nodes[x, index].HeldCube.transform.position.x + midDistance.x/2,
                        _gridManager.Nodes[x, index].HeldCube.transform.position.y + midDistance.y/2+1,
                        _gridManager.Nodes[x, index].HeldCube.transform.position.z + midDistance.z/2);
                    
                    pathList[2] = new Vector3(
                        _gridManager.BaseCubeList[x].transform.position.x,
                        _gridManager.BaseCubeList[x].transform.position.y,
                        _gridManager.BaseCubeList[x].transform.position.z);
                    
                    _gridManager.Nodes[x, index].HeldCube.transform.DOPath(pathList, 1, _pathType).OnComplete(
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

