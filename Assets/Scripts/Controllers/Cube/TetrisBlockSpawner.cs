using System;
using System.Collections.Generic;
using Enums;
using Managers;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Cube
{
    public class TetrisBlockSpawner : MonoBehaviour
    {
        [SerializeField] private List<TetrisBlockManager> tetrisBlockList;
        private GridManager _gridManager;
        private List<TetrisBlockManager> _spawnList = new List<TetrisBlockManager>();
        [ShowInInspector]private TetrisBlockManager spawningObject;
        private GameStates _gameStates;

        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
        }

        void Start()
        {
            DetectSpawnableBlocks();
            RandomSpawnBlock();
        }

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
                
                Tile checkingTile = _gridManager.Nodes[xIndex, yIndex];

                control = checkingTile.IsPlaceable;
                if (checkingTile.IsBaseTile) return false;
                if (checkingTile.IsEnemyTile) return false;
                if (control == false) return false;
            }
            
            return true;
        }

        private void DetectSpawnableBlocks()
        {
            _spawnList.Clear();
            for (int i = 0; i < tetrisBlockList.Count; i++)
            {
                foreach (Tile tile in _gridManager.Nodes)
                {
                    if (SpawnCheck(tile.CellIndex,tetrisBlockList[i].cubePositions))
                    {
                        _spawnList.Add(tetrisBlockList[i]);
                        break;
                    }
                }
            }
        }

        private void RandomSpawnBlock()
        {
            if (_gameStates == GameStates.GameStop)
            {
                return;
            }
            spawningObject = Instantiate(_spawnList[Random.Range(0, _spawnList.Count)]); 
            spawningObject.transform.position = transform.position;
        }
    }
}
