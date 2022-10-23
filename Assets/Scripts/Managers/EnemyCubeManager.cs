using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers.Cube;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class EnemyCubeManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyCube> enemyCubeList = new List<EnemyCube>();
        [SerializeField] private Transform enemyCubeHolder;

        public int LeftCubeCount { get; private set; } = 30;
        public int SpawnCubeCount { get; private set; }
        private GridManager _gridManager;
        
        private ObjectPooler _objectPooler;
        
        private void Awake()
        {
            _objectPooler = FindObjectOfType<ObjectPooler>();
            _gridManager = FindObjectOfType<GridManager>();
            SpawnCubeCount = LeftCubeCount;
        }

        private void Start()
        {
            EnemyCubeGetFromPool();
            UISignals.Instance.onSetLeftText?.Invoke(LeftCubeCount);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
            EnemyCubeSignals.Instance.onHitEnemyCube += OnHitEnemyCube;
        }
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
            EnemyCubeSignals.Instance.onHitEnemyCube -= OnHitEnemyCube;
        }
        
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void OnChangeGameState(GameStates currentState)
        {
            if (currentState == GameStates.EnemyMovePhase)
            {
                if (enemyCubeList.Count == 0)
                {
                    EnemySpawnPhase();
                    return;
                }
                EnemyCubeMove();
                EnemySpawnPhase();
            }
             
            if (currentState == GameStates.EnemySpawnPhase)
            {
                EnemyCubeGetFromPool();
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Playing);
            }
        }

        private async void EnemySpawnPhase()
        {
            await Task.Delay(1000);
            CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.EnemySpawnPhase);
        }

        private void EnemyCubeMove()
        {
            for (int i = 0; i < enemyCubeList.Count; i++)
            {
                enemyCubeList[i].GetComponent<EnemyCubeMovementController>().Move();
                enemyCubeList.TrimExcess();
            }
        }

        private void OnHitEnemyCube(Transform enemyCube)
        {
            enemyCube.GetComponentInChildren<EnemyCubeMeshController>().ArmyHitEnemyCube();
            enemyCubeList.TrimExcess();
        }

        public void RemoveEnemyCubeList(EnemyCube enemyCube)
        {
            if (enemyCube.EnemyCubeTilePosition.y <=4)
            {
                _gridManager.Nodes[enemyCube.EnemyCubeTilePosition.x, enemyCube.EnemyCubeTilePosition.y].IsPlaceable = true;
                _gridManager.Nodes[enemyCube.EnemyCubeTilePosition.x, enemyCube.EnemyCubeTilePosition.y].IsEnemyTile = false;
            }
            _gridManager.Nodes[enemyCube.EnemyCubeTilePosition.x, enemyCube.EnemyCubeTilePosition.y].HeldCube = null;
            enemyCubeList.Remove(enemyCube);
            LeftCubeCount--;
            UISignals.Instance.onSetLeftText?.Invoke(LeftCubeCount);
            if (LeftCubeCount <= 0 && SpawnCubeCount <= 0)
            {
                //Game end signal here
            }
        }

        private void EnemyCubeGetFromPool()
        {
            for (int i = 0; i < _gridManager.Nodes.GetLength(0); i++)
            {
                if (SpawnCubeCount <= 0) return;
                
                int spawnPointY = _gridManager.Nodes.GetLength(1) - 1;
                EnemyCube EnemyCube = _objectPooler.SpawnFromPool(
                    "EnemyCube",
                    Vector3.up,
                    Quaternion.identity,
                    _gridManager.Nodes[i, spawnPointY].transform).GetComponent<EnemyCube>();

                EnemyCube.EnemyCubeTilePosition = new Vector2Int(i, spawnPointY);
                _gridManager.Nodes[i, spawnPointY].HeldCube = EnemyCube;
                _gridManager.Nodes[i, spawnPointY].IsPlaceable = false;
                _gridManager.Nodes[i, spawnPointY].SnapPoint();
                enemyCubeList.Add(EnemyCube);
                SpawnCubeCount--;
            }
        }
        
        public void ReturnToPoolArmy(GameObject enemyCube)
        {
            _objectPooler.ReturnToPool("EnemyCube",enemyCube);
        }
    }
}