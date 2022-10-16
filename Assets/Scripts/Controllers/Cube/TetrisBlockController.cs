using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Managers;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class TetrisBlockController : MonoBehaviour
    {
        [SerializeField] public CubeTransform[] cubePositions;

        private GridManager _gridManager;
        
        private List<int> fullRowIndexList = new List<int>();

        [SerializeField] private Vector3[] PathList = new Vector3[3];

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
            Debug.Log("KAYDOLDUM");
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
                Debug.Log("Merge");
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
        }

        private void CheckMerge()
        {
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
                
                for (int x = 0; x < _gridManager._nodes.GetLength(0); x++)
                {
                    var x1 = x;
                    
                    
                    Vector3 midDistance = _gridManager.BaseCubeList[x].transform.position
                                          -_gridManager._nodes[x, index].HeldCube.transform.position;
                    
                    PathList[0] = new Vector3(
                        _gridManager._nodes[x, index].HeldCube.transform.position.x,
                        _gridManager._nodes[x, index].HeldCube.transform.position.y,
                        _gridManager._nodes[x, index].HeldCube.transform.position.z);
                    
                    PathList[1] = new Vector3(
                        _gridManager._nodes[x, index].HeldCube.transform.position.x + midDistance.x/2,
                        _gridManager._nodes[x, index].HeldCube.transform.position.y + midDistance.y/2+1,
                        _gridManager._nodes[x, index].HeldCube.transform.position.z + midDistance.z/2);
                    
                    PathList[2] = new Vector3(
                        _gridManager.BaseCubeList[x].transform.position.x,
                        _gridManager.BaseCubeList[x].transform.position.y,
                        _gridManager.BaseCubeList[x].transform.position.z);
                    
                    _gridManager._nodes[x, index].HeldCube.transform.DOPath(PathList, 1, _pathType).OnComplete(
                        () =>
                        {
                            _gridManager.BaseCubeList[x1].IncreaseCubeValue(_gridManager._nodes[x1, index].HeldCube.CubeValue);
                            _gridManager._nodes[x1, index].IsPlaceable = true;
                            Destroy(_gridManager._nodes[x1, index].HeldCube.gameObject);
                            _gridManager._nodes[x1, index].HeldCube = null;
                        }).SetEase(Ease.InOutSine);

                    #region Old DOPaths

                    ////////////////////////////////
                    // PathList[0] = new Vector3(
                    //     _gridManager._nodes[x, index].HeldCube.transform.position.x,
                    //     _gridManager._nodes[x, index].HeldCube.transform.position.y,
                    //     _gridManager._nodes[x, index].HeldCube.transform.position.z);
                    //
                    // PathList[1] = new Vector3(
                    //     transform.position.x, 
                    //     transform.position.y+1,
                    //     transform.position.z);
                    //
                    // PathList[2] = new Vector3(
                    //     _gridManager.BaseCubeList[x].transform.position.x,
                    //     _gridManager.BaseCubeList[x].transform.position.y,
                    //     _gridManager.BaseCubeList[x].transform.position.z);
                    //
                    //
                    // _gridManager._nodes[x, index].HeldCube.transform.DOPath(PathList, 1, _pathType).OnComplete(
                    //     () =>
                    //     {
                    //         _gridManager.BaseCubeList[x1].IncreaseCubeValue(_gridManager._nodes[x1, index].HeldCube.CubeValue);
                    //         _gridManager._nodes[x1, index].IsPlaceable = true;
                    //         Destroy(_gridManager._nodes[x1, index].HeldCube.gameObject);
                    //         _gridManager._nodes[x1, index].HeldCube = null;
                    //     });
                    ///////////////////////////////
                    // _gridManager._nodes[x, index].HeldCube.transform.DOMove(_gridManager.BaseCubeList[x].transform.position, 1f).OnComplete(
                    //     () =>
                    //     {
                    //         _gridManager.BaseCubeList[x1].IncreaseCubeValue(_gridManager._nodes[x1, index].HeldCube.CubeValue);
                    //         _gridManager._nodes[x1, index].IsPlaceable = true;
                    //         Destroy(_gridManager._nodes[x1, index].HeldCube.gameObject);
                    //         _gridManager._nodes[x1, index].HeldCube = null;
                    //     });

                        #endregion
                    
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

