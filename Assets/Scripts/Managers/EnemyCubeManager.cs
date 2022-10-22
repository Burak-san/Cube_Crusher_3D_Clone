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
        [SerializeField] private List<Transform> enemyCubeSpawnTransformList = new List<Transform>();
        [SerializeField] private Transform enemyCubeHolder;
        private GridManager _gridManager;
        
        private ObjectPooler _objectPooler;
        
        private void Awake()
        {
            _objectPooler = FindObjectOfType<ObjectPooler>();
            _gridManager = FindObjectOfType<GridManager>();
        }

        private void Start()
        {
            EnemyCubeGetFromPool();
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
                EnemyCubeMove();
                EnemySpawnPhaseSignal();
                
            }
             
            if (currentState == GameStates.EnemySpawnPhase)
            {
                EnemyCubeGetFromPool();
                CoreGameSignals.Instance.onChangeGameState?.Invoke(GameStates.Playing);
            }
        }

        private async void EnemySpawnPhaseSignal()
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
            if (enemyCube.EnemyCubeTilePosition.y <=3)
            {
                _gridManager.Nodes[enemyCube.EnemyCubeTilePosition.x, enemyCube.EnemyCubeTilePosition.y].IsPlaceable = true;
                _gridManager.Nodes[enemyCube.EnemyCubeTilePosition.x, enemyCube.EnemyCubeTilePosition.y].IsEnemyTile = false;
            }
            _gridManager.Nodes[enemyCube.EnemyCubeTilePosition.x, enemyCube.EnemyCubeTilePosition.y].HeldCube = null;
            enemyCubeList.Remove(enemyCube);
        }
        

        private void EnemyCubeGetFromPool()
        {
            for (int i = 0; i < _gridManager.Nodes.GetLength(0); i++)
            {
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
            }
        }
        
        public void ReturnToPoolArmy(GameObject enemyCube)
        {
            _objectPooler.ReturnToPool("EnemyCube",enemyCube);
        }
    }
}