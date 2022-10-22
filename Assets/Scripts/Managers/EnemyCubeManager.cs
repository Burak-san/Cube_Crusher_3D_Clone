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
            if (enemyCube.tilePosition.y <=3)
            {
                _gridManager._nodes[enemyCube.tilePosition.x, enemyCube.tilePosition.y].IsPlaceable = true;
                _gridManager._nodes[enemyCube.tilePosition.x, enemyCube.tilePosition.y].IsEnemyTile = false;
            }
            _gridManager._nodes[enemyCube.tilePosition.x, enemyCube.tilePosition.y].HeldCube = null;
            enemyCubeList.Remove(enemyCube);
        }
        

        private void EnemyCubeGetFromPool()
        {
            for (int i = 0; i < _gridManager._nodes.GetLength(0); i++)
            {
                int spawnPointY = _gridManager._nodes.GetLength(1) - 1;
                EnemyCube EnemyCube = _objectPooler.SpawnFromPool(
                    "EnemyCube",
                    Vector3.up,
                    Quaternion.identity,
                    _gridManager._nodes[i, spawnPointY].transform).GetComponent<EnemyCube>();

                EnemyCube.tilePosition = new Vector2Int(i, spawnPointY);
                _gridManager._nodes[i, spawnPointY].HeldCube = EnemyCube;
                _gridManager._nodes[i, spawnPointY].IsPlaceable = false;
                _gridManager._nodes[i, spawnPointY].SnapPoint();
                enemyCubeList.Add(EnemyCube);
            }
        }
        
        public void ReturnToPoolArmy(GameObject enemyCube)
        {
            _objectPooler.ReturnToPool("EnemyCube",enemyCube);
        }
    }
}