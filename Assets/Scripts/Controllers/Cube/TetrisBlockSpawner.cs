using System.Collections.Generic;
using Enums;
using Managers;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using Data.ValueObject;

namespace Controllers.Cube
{
    public class TetrisBlockSpawner : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<TetrisBlockManager> tetrisBlockList;
        [SerializeField] private Transform tetrisCubeHolder;

        #endregion

        #region Private Variables

        private GridManager _gridManager;
        private List<TetrisBlockManager> _spawnList = new List<TetrisBlockManager>();
        private GameStates _gameStates;
        [ShowInInspector]private TetrisBlockManager _spawningObject;
        
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

        void Start()
        {
            DetectSpawnableBlocks();
            RandomSpawnBlock();
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
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void OnChangeGameState(GameStates currentState)
        {
            if (currentState == GameStates.GameStop)
            {
                _gameStates = currentState;
            }
            
            if (currentState == GameStates.EnemySpawnPhase)
            {
                DetectSpawnableBlocks();
                RandomSpawnBlock();
            }
        }

        private void DetectSpawnableBlocks()
        {
            _spawnList.Clear();
            for (int i = 0; i < tetrisBlockList.Count; i++)
            {
                foreach (TileData tile in _gridManager.Nodes)
                {
                    if (SpawnCheck(tile.CellIndex,tetrisBlockList[i].cubePositions))
                    {
                        _spawnList.Add(tetrisBlockList[i]);
                        break;
                    }
                }
            }
        }
        
        public bool SpawnCheck(Vector2Int checkingTileIndex, CubeTransform[] cubePositions)
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
                if (checkingTileData.IsBaseTile) return false;
                if (checkingTileData.IsEnemyTile) return false;
                if (control == false) return false;
            }
            
            return true;
        }
        
        private void RandomSpawnBlock()
        {
            if (_gameStates == GameStates.GameStop)
            {
                return;
            }
            _spawningObject = Instantiate(_spawnList[Random.Range(0, _spawnList.Count)]); 
            _spawningObject.transform.position = transform.position;
            _spawningObject.transform.SetParent(tetrisCubeHolder);
        }
    }
}
